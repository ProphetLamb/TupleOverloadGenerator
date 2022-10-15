using System.Collections.Immutable;

using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace InheritInterfaceDefaultMethods;

[Serializable]
public readonly record struct ClassContext(TypeDeclarationSyntax Declaration, ImmutableArray<MethodContext> Methods) {
    public bool IsDefault => default == this;
}

[Serializable]
public readonly record struct MethodContext(MethodDeclarationSyntax Method, ImmutableArray<(ParameterSyntax Param, string ElementType)> Param) {
    public bool IsDefault => default == this;
}
