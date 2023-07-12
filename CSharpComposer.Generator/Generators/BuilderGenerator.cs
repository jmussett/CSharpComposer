using CSharpComposer.Generator.Builders;
using CSharpComposer.Generator.Registries;
using CSharpComposer.Generator.Utility;

namespace CSharpComposer.Generator.Generators;

internal class BuilderGenerator
{
    private readonly CSharpRegistry _csharpRegistry;
    private readonly InterfaceBuilder _interfaceBuilder;
    private readonly ImplementationBuilder _implementationBuilder;
    private readonly DocumentRegistry _documentRegistry;

    public BuilderGenerator(CSharpRegistry registry, InterfaceBuilder interfaceBuilder, ImplementationBuilder implementationBuilder, DocumentRegistry documentRegistry)
    {
        _csharpRegistry = registry;
        _interfaceBuilder = interfaceBuilder;
        _implementationBuilder = implementationBuilder;
        _documentRegistry = documentRegistry;
    }

    public void GenerateBuilders()
    {
        foreach (var type in _csharpRegistry.Tree.Types)
        {
            // TODO: Skip identifier syntax (like FunctionPointerUnmanagedCallingConventionSyntax, NameColonSyntax)
            if (!NodeValidator.IsValidNode(type.Name))
            {
                continue;
            }

            var compilationUnit = CSharpFactory.CompilationUnit(x => x
                .AddUsingDirective("System")
                .AddUsingDirective("Microsoft.CodeAnalysis") // TODO: ???
                .AddUsingDirective("Microsoft.CodeAnalysis.CSharp")
                .AddUsingDirective("Microsoft.CodeAnalysis.CSharp.Syntax")
                .AddFileScopedNamespaceDeclaration("CSharpComposer", ns =>
                {
                    // TODO: Seperate into different files (don't want to expose internal types when navigating)
                    ns = _interfaceBuilder.WithInterfaces(ns, type);
                    ns = _implementationBuilder.WithImplementation(ns, type);
                })
            );

            var builderName = NameFactory.CreateBuilderName(type.Name!);

            _documentRegistry.Documents.Add($"Generated/Builders/{builderName}.cs", compilationUnit);
        }
    }
}
