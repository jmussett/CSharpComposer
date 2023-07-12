using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer.Generator.Registries;

internal class DocumentRegistry
{
    public Dictionary<string, CompilationUnitSyntax> Documents { get; } = new();
}
