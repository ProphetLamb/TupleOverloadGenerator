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
        foreach (MethodDeclarationSyntax method in methodsWithAttributes) {
            var methodContext = GetMethodContext(ctx, method);
            if (!methodContext.IsDefault) {
                methods.Add(methodContext);
            }
            ct.ThrowIfCancellationRequested();
        }

        return new(typeDecl, methods.ToImmutable());
    }

    private static MethodContext GetMethodContext(GeneratorSyntaxContext ctx, MethodDeclarationSyntax method) {
        return method.ParameterList.Parameters
           .FilterMap(param => GetParamContext(ctx, param) is { IsDefault: false } context ? new MethodContext(method, context).Nullable() : default)
           .FirstOrDefault();
    }

    private static ParamContext GetParamContext(GeneratorSyntaxContext ctx, ParameterSyntax param) {
        if (GetOverloadTupleParameter(ctx, param) is not { } attribute || !IsParamsArrayParameter(param)) {
            return default;
        }

        // read minimum and maximum value
        int min = 1, max = 21;
        if (attribute.ArgumentList is not null) {
            if (ParseArgumentInt(attribute.ArgumentList.Arguments, "Minimum", ref min)
              | ParseArgumentInt(attribute.ArgumentList.Arguments, "Maximum", ref max)) {
                if (min >= max || min is < 1 or > 21 || max is < 1 or > 21) {
                    return default;
                }
            }
        }

        var info = ctx.SemanticModel.GetSymbolInfo(param.Type!);
        if (info.Symbol is not IArrayTypeSymbol type) {
            return default;
        }

        return new(param, type.ElementType.Name, min, max);
    }

    private static AttributeSyntax? GetOverloadTupleParameter(GeneratorSyntaxContext ctx, BaseParameterSyntax param) {
        return param.AttributeLists.Attributes().FilterMapFirst(attribute
            => ctx.SemanticModel.GetTypeInfo(attribute) is { Type: { } type }
            && type.ToDisplayString() is Helper.ATTRIBUTE_TYPE_NAME or Helper.ATTRIBUTE_NAME ? attribute : default
        );
    }

    private static bool IsParamsArrayParameter(BaseParameterSyntax param) {
        return param.Modifiers.Any(static modifier => modifier.Text == "params");
    }

    private static bool ParseArgumentInt(SeparatedSyntaxList<AttributeArgumentSyntax> args, string name, ref int value) {
        return args.FirstOrDefault(arg
                => arg.NameEquals?.Name.Identifier.Text == name) is { Expression: LiteralExpressionSyntax minLiteral }
         && int.TryParse(minLiteral.Token.ValueText, out value);
    }

    private static void Execute(ImmutableArray<TypeContext> classes, SourceProductionContext context) {
        if (classes.IsDefaultOrEmpty) {
            return;
        }

        object guard = new();
        var distinctClasses = classes.Distinct();
        Parallel.ForEach(distinctClasses.FilterMap(static typeContext
                    => GenerateSyntaxTree(typeContext) is { } compilation ? (typeContext.Declaration.Identifier, Unit: compilation).Nullable() : default),
            ctx => {
                SourceText sourceText = ctx.Unit.GetText(Encoding.Default);
                lock(guard) {
                    context.AddSource($"{ctx.Identifier.Text}.{Helper.ATTRIBUTE_NAME}.g.cs", sourceText);
                }
            }
        );
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
        // for records we must remove the parameterlist: 'record Concat(string Prefix, string Infix, string Suffix) {}' -> 'record Concat {}'
        if (declSyntax is RecordDeclarationSyntax { ParameterList: { } } record) {
            declSyntax = record.RemoveNode(record.ParameterList, SyntaxRemoveOptions.KeepNoTrivia) ?? declSyntax;
        }

        return declSyntax;
    }

    private static CompilationUnitSyntax? ModifyCompilationUnit(TypeContext typeContext, MemberDeclarationSyntax declSyntax) {
        // the partial modifier only effects members of the same namespace
        var namespaceDecl = typeContext.Declaration.ParentOf<BaseNamespaceDeclarationSyntax>();
        var unit = namespaceDecl?.ParentOf<CompilationUnitSyntax>();
        if (namespaceDecl is null || unit is null)
        {
            // TODO: diagnostics
            return default;
        }

        // replace all members in namespace with ONLY the declaration syntax
        var namespaceMembers = SyntaxFactory.List(Enumerable.Repeat(declSyntax, 1));
        var modifiedNamespace = namespaceDecl.WithMembers(namespaceMembers);

        var unitMembers = SyntaxFactory.List(Enumerable.Repeat<MemberDeclarationSyntax>(modifiedNamespace, 1));
        unit = unit.WithMembers(unitMembers);
        return unit;
    }

    private static IEnumerable<MethodDeclarationSyntax> CreateMember(MethodContext method)
    {
        foreach (ParameterSyntax newParam in CreateTupleParameters(method.Param))
        {
            ParameterListSyntax paramList = method.Method.ParameterList;
            var parameters = paramList.Parameters.Replace(method.Param.Param, newParam);
            var newParamList = paramList.WithParameters(parameters);
            yield return method.Method.WithParameterList(newParamList);
        }
    }

    private static IEnumerable<ParameterSyntax> CreateTupleParameters(ParamContext context)
    {
        // remove params from the modifiers
        var paramModifier = context.Param.Modifiers.First(static modifier => modifier.ToString() == "params");
        var modifiers = context.Param.Modifiers.Remove(paramModifier);
        // create tuple element from the element type name
        var elementSyntax = SyntaxFactory.ParseTypeName(context.ElementType);
        var tupleElement = SyntaxFactory.TupleElement(elementSyntax);
        if (context.Minimum == 1) {
            // `ValueTuple<T>` as `(T)` is invalid syntax. Explicit type is required for count = 1
            yield return CreateParameter(context, modifiers, CreateValueTuple(elementSyntax, 1));
        }
        for (int i = Math.Max(2, context.Minimum); i <= context.Maximum; i++)
        {
            yield return CreateParameter(context, modifiers, CreateTuple(tupleElement, i));
        }
    }

    private static SyntaxTrivia[] SpaceTrivia { get; } = { SyntaxFactory.Space };

    private static ParameterSyntax CreateParameter(ParamContext context, SyntaxTokenList modifiers, TypeSyntax type) {

        // add space after type 'ValueTuple<T>elements' -> 'ValueTuple<T> elements'
        var typeWithTrivia = type.WithTrailingTrivia(SpaceTrivia);
        return SyntaxFactory.Parameter(context.Param.AttributeLists, modifiers, typeWithTrivia, context.Param.Identifier, default);
    }

    private static SyntaxToken ValueTupleIdentifier { get; } = SyntaxFactory.Identifier("ValueTuple");

    private static GenericNameSyntax CreateValueTuple(TypeSyntax elementType, int count) {
        var typeArgs = SyntaxFactory.SeparatedList(Enumerable.Repeat(elementType, count));
        return SyntaxFactory.GenericName(ValueTupleIdentifier, SyntaxFactory.TypeArgumentList(typeArgs));
    }

    private static TupleTypeSyntax CreateTuple(TupleElementSyntax elementType, int count)
    {
        var tupleElements = SyntaxFactory.SeparatedList(Enumerable.Repeat(elementType, count));
        return SyntaxFactory.TupleType(tupleElements);
    }
}
