using System.Collections.Immutable;

using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TupleOverloadGenerator;

[Serializable]
internal readonly record struct TypeContext(TypeDeclarationSyntax Declaration, ImmutableArray<MethodContext> Methods)
{
    public bool IsDefault => default == this;
}

[Serializable]
internal  readonly record struct MethodContext(MethodDeclarationSyntax Method, ImmutableArray<(ParameterSyntax Param, string ElementType)> Param)
{
    public bool IsDefault => default == this;
}
