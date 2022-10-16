using System.Collections.Immutable;

using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TupleOverloadGenerator;

[Serializable]
internal record struct TypeContext(TypeDeclarationSyntax Declaration, ImmutableArray<MethodContext> Methods) {
    public bool IsDefault => default == this;
}

[Serializable]
internal record struct ParamContext(ParameterSyntax Param, string ElementType, int Minimum, int Maximum) {
    public bool IsDefault => default == this;
}

[Serializable]
internal record struct MethodContext(MethodDeclarationSyntax Method, ParamContext Param) {
    public bool IsDefault => default == this;
}
