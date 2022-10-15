using System.Collections.Immutable;
using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace TupleOverloadGenerator;

public sealed class Generator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(static ctx =>
        {
            ctx.AddSource(
                "OverloadTuple.g.cs",
                SourceText.From(Helper.AttributeSource, Encoding.UTF8)
            );
            ctx.AddSource(
                "TupleExtensions.g.cs",
                SourceText.From(Helper.ExtensionSource, Encoding.UTF8)
            );
        }
        );

        var classContexts = context.SyntaxProvider
           .CreateSyntaxProvider(
                static (s, _) => IsMethodContextCandidate(s),
                static (ctx, ct) => GetClassContext(ctx, ct)
            )
           .Where(static t => !t.IsDefault)
           .Collect();

        IncrementalValueProvider<(Compilation Compilation, ImmutableArray<ClassContext> Contexts)> compilationAndEnums
            = context.CompilationProvider.Combine(classContexts);
        context.RegisterSourceOutput(compilationAndEnums,
            static (spc, source) => Execute(source.Compilation, source.Contexts, spc));
    }

    private static bool IsMethodContextCandidate(SyntaxNode node)
    {
        // static partial class with a method which has a body with a argument with a attribute
        // its a pretty good heuristic to reduce the number of affected classes
        return node is ClassDeclarationSyntax decl
         && decl.Modifiers.Any(static id => id.Text == "partial")
         && decl.Members.Any(static member
             => member is MethodDeclarationSyntax { Body: { } } method
             && method.ParameterList.Parameters.Any(static param
                 => param.AttributeLists.Attributes().Any()));
    }

    private static ClassContext GetClassContext(GeneratorSyntaxContext ctx, CancellationToken ct)
    {
        var classSyntax = (ClassDeclarationSyntax)ctx.Node;
        var methodsWithAttributes = classSyntax.Members.Where(static member
            => member is MethodDeclarationSyntax { Body: { } } method
            && method.ParameterList.Parameters.Any(static param
                => param.AttributeLists.Any())).Cast<MethodDeclarationSyntax>();

        var methods = ImmutableArray.CreateBuilder<MethodContext>();
        foreach (MethodDeclarationSyntax method in methodsWithAttributes)
        {
            methods.Add(GetMethodContext(ctx, method));
            ct.ThrowIfCancellationRequested();
        }

        return new(classSyntax, methods.ToImmutable());
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
            var model = ModelExtensions.GetSymbolInfo(ctx.SemanticModel, attribute);
            if (model.Symbol is IMethodSymbol attributeSymbol)
            {
                var typeName = attributeSymbol.ContainingType.ToDisplayString();
                if (typeName == "System.OverloadTupleAttribute")
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

    private static void Execute(Compilation compilation, ImmutableArray<ClassContext> classes, SourceProductionContext context)
    {
        if (!classes.IsDefaultOrEmpty)
        {
            var distinctClasses = classes.Distinct();
            foreach ((SyntaxToken name, CompilationUnitSyntax compSyntax) in distinctClasses.Select(static classContext => (classContext.Declaration.Identifier, CompSyntax: GenerateSyntaxTree(classContext)!))
               .NotNull(static t => t.CompSyntax))
            {
                SourceText sourceText = compSyntax.GetText();
                context.AddSource($"{name.Text}.cs", sourceText);
            }
        }
    }

    private static CompilationUnitSyntax? GenerateSyntaxTree(ClassContext classContext)
    {
        SyntaxList<MemberDeclarationSyntax> members = new();
        foreach (MethodContext method in classContext.Methods)
        {
            foreach (MethodDeclarationSyntax methodSyntax in CreateMember(method))
            {
                members.Add(methodSyntax);
            }
        }

        if (members.Count <= 0)
        {
            return default;
        }

        var declSyntax = classContext.Declaration.WithMembers(members);
        var compilation = classContext.Declaration.ParentOf<CompilationUnitSyntax>();
        if (compilation is null)
        {
            return default;
        }

        // replace all members in compilation with ONLY the declaration syntax
        var compMembers = SyntaxFactory.List(Enumerable.Repeat((MemberDeclarationSyntax)declSyntax, 1));
        var compWithMembers = compilation?.WithMembers(compMembers);
        return compWithMembers;
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

        return SyntaxFactory.Parameter(attributes, modifiers, tupleType, identifier, default);
    }
}
