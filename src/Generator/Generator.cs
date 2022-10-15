using System.Collections.Immutable;
using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace TupleOverloadGenerator;

[Generator]
public sealed class Generator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(static ctx =>
        {
            ctx.AddSource(
                $"{Helper.AttributeName}.g.cs",
                SourceText.From(Helper.AttributeSource, Encoding.UTF8)
            );
            ctx.AddSource(
                $"{Helper.ExtensionName}.g.cs",
                SourceText.From(Helper.ExtensionSource, Encoding.UTF8)
            );
        }
        );

        var typeContexts = context.SyntaxProvider
           .CreateSyntaxProvider(
                static (s, _) => IsTypeContextCandidate(s),
                static (ctx, ct) => GetTypeContext(ctx, ct)
            )
           .Where(static t => !t.IsDefault)
           .Collect();

        IncrementalValueProvider<(Compilation Compilation, ImmutableArray<TypeContext> Contexts)> compilationAndEnums
            = context.CompilationProvider.Combine(typeContexts);
        context.RegisterSourceOutput(compilationAndEnums,
            static (spc, source) => Execute(source.Contexts, spc));
    }

    private static bool IsTypeContextCandidate(SyntaxNode node)
    {
        // static partial type decl with a method which has a body with a argument with a attribute
        // its a pretty good heuristic to reduce the number of affected types
        return node is TypeDeclarationSyntax decl
         && decl.Modifiers.Any(static id => id.Text == "partial")
         && decl.Members.Any(static member
             => member is MethodDeclarationSyntax { Body: { } } method
             && method.ParameterList.Parameters.Any(static param
                 => param.AttributeLists.Attributes().Any()));
    }

    private static TypeContext GetTypeContext(GeneratorSyntaxContext ctx, CancellationToken ct)
    {
        var typeDecl = (TypeDeclarationSyntax)ctx.Node;
        var methodsWithAttributes = typeDecl.Members.FilterMap(static member
            => member is MethodDeclarationSyntax { Body: { } } method
            && method.ParameterList.Parameters.Any(static param
                => param.AttributeLists.Any()) ? method : default);

        var methods = ImmutableArray.CreateBuilder<MethodContext>();
        foreach (MethodDeclarationSyntax method in methodsWithAttributes)
        {
            methods.Add(GetMethodContext(ctx, method));
            ct.ThrowIfCancellationRequested();
        }

        return new(typeDecl, methods.ToImmutable());
    }

    private static MethodContext GetMethodContext(GeneratorSyntaxContext ctx, MethodDeclarationSyntax method)
    {
        var parameters = ImmutableArray.CreateBuilder<(ParameterSyntax, string)>();
        foreach (ParameterSyntax param in method.ParameterList.Parameters)
        {
            if (IsOverloadTupleParameter(ctx, param) && IsParamsArrayParameter(param))
            {
                var info = ctx.SemanticModel.GetSymbolInfo(param.Type!);
                if (info.Symbol is IArrayTypeSymbol type)
                {
                    parameters.Add((param, type.ElementType.Name));
                }
            }
        }

        return new(method, parameters.ToImmutable());
    }

    private static bool IsOverloadTupleParameter(GeneratorSyntaxContext ctx, BaseParameterSyntax param)
    {
        foreach (AttributeSyntax attribute in param.AttributeLists.Attributes())
        {
            var model = ctx.SemanticModel.GetTypeInfo(attribute);
            if (model.Type is not null)
            {
                var typeName = model.Type.ToDisplayString();
                if (typeName == Helper.AttributeTypeName)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private static bool IsParamsArrayParameter(BaseParameterSyntax param)
    {
        return param.Modifiers.Any(static modifier => modifier.Text == "params");
    }

    private static void Execute(ImmutableArray<TypeContext> classes, SourceProductionContext context)
    {
        if (!classes.IsDefaultOrEmpty)
        {
            var distinctClasses = classes.Distinct();
            foreach ((SyntaxToken name, CompilationUnitSyntax compilation) in distinctClasses.FilterMap(static typeContext
                => GenerateSyntaxTree(typeContext) is { } compilation ? (typeContext.Declaration.Identifier, compilation).Nullable() : default))
            {
                SourceText sourceText = compilation.GetText(Encoding.Default);
                context.AddSource($"{name.Text}.{Helper.AttributeName}.g.cs", sourceText);
            }
        }
    }

    private static CompilationUnitSyntax? GenerateSyntaxTree(TypeContext typeContext) {
        var members = SyntaxFactory.List<MemberDeclarationSyntax>(typeContext.Methods.SelectMany(static method => CreateMember(method)));

        if (members.Count <= 0)
        {
            return default;
        }

        TypeDeclarationSyntax declSyntax = ModifyTypeDeclaration(typeContext, members);

        CompilationUnitSyntax? compWithMembers = ModifyCompilationUnit(typeContext, declSyntax);
        return compWithMembers;
    }

    private static TypeDeclarationSyntax ModifyTypeDeclaration(TypeContext typeContext, SyntaxList<MemberDeclarationSyntax> members) {
        var declSyntax = typeContext.Declaration.WithMembers(members);
        // for records we must remove the parameterlist: 'record Concatinate(string Prefix, string Suffix) {}' -> 'record Concatinate {}'
        if (declSyntax is RecordDeclarationSyntax record && record.ParameterList is not null) {
            declSyntax = record.RemoveNode(record.ParameterList, SyntaxRemoveOptions.KeepNoTrivia) ?? declSyntax;
        }

        return declSyntax;
    }

    private static CompilationUnitSyntax? ModifyCompilationUnit(TypeContext typeContext, MemberDeclarationSyntax declSyntax) {
        // the partial modifier only effects members of the same namespace
        var namespaceDecl = typeContext.Declaration.ParentOf<BaseNamespaceDeclarationSyntax>();
        var compilationUnit = namespaceDecl?.ParentOf<CompilationUnitSyntax>();
        if (namespaceDecl is null || compilationUnit is null)
        {
            return default;
        }

        // replace all members in namespace with ONLY the declaration syntax
        var namespaceMembers = SyntaxFactory.List(Enumerable.Repeat(declSyntax, 1));
        var modifiedNamespace = namespaceDecl.WithMembers(namespaceMembers);

        var unitMembers = SyntaxFactory.List(Enumerable.Repeat<MemberDeclarationSyntax>(modifiedNamespace, 1));
        var modifiedUnit = compilationUnit?.WithMembers(unitMembers);
        return modifiedUnit;
    }

    private static IEnumerable<MethodDeclarationSyntax> CreateMember(MethodContext method)
    {
        foreach ((ParameterSyntax param, string elementType) in method.Param)
        {
            foreach (ParameterSyntax newParam in CreateTupleParameters(param, elementType))
            {
                ParameterListSyntax paramList = method.Method.ParameterList;
                var parameters = paramList.Parameters.Replace(param, newParam);
                var newParamList = paramList.WithParameters(parameters);
                yield return method.Method.WithParameterList(newParamList);
            }
        }
    }

    private static IEnumerable<ParameterSyntax> CreateTupleParameters(ParameterSyntax paramsArray, string elementType)
    {
        // remove params from the modifiers
        var paramModifier = paramsArray.Modifiers.First(static modifier => modifier.ToString() == "params");
        var modifiers = paramsArray.Modifiers.Remove(paramModifier);
        // create tuple element from the element type name
        var elementSyntax = SyntaxFactory.ParseTypeName(elementType);
        var tupleElement = SyntaxFactory.TupleElement(elementSyntax);

        for (int i = 2; i <= 7; i++)
        {
            yield return CreateTupleParameter(paramsArray.AttributeLists, modifiers, tupleElement, i, paramsArray.Identifier);
        }
    }

    private static ParameterSyntax CreateTupleParameter(SyntaxList<AttributeListSyntax> attributes, SyntaxTokenList modifiers, TupleElementSyntax elementType, int count, SyntaxToken identifier)
    {
        var tupleElements = SyntaxFactory.SeparatedList(Enumerable.Repeat(elementType, count));
        var tupleType = SyntaxFactory.TupleType(tupleElements);
        // add space after type '(T,T,T)elements' -> '(T,T,T) elements' 
        tupleType = tupleType.WithTrailingTrivia(SyntaxFactory.Space);

        return SyntaxFactory.Parameter(attributes, modifiers, tupleType, identifier, default);
    }
}
