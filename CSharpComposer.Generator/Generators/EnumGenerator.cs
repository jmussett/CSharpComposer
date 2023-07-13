using CSharpComposer.Generator.Registries;
using Microsoft.CodeAnalysis.CSharp;

namespace CSharpComposer.Generator.Generators;

internal class EnumGenerator
{
    private readonly EnumRegistry _enumRegistry;
    private readonly DocumentRegistry _documentRegistry;

    public EnumGenerator(EnumRegistry enumRegistry, DocumentRegistry documentRegistry)
    {
        _enumRegistry = enumRegistry;
        _documentRegistry = documentRegistry;
    }

    public void GenerateEnums()
    {
        foreach (var kvp in _enumRegistry.FieldEnums)
        {
            var compilationUnit = CompilationUnitBuilder.CreateSyntax(x => x
               .AddFileScopedNamespaceDeclaration("CSharpComposer", ns =>
                   ns.AddEnumDeclaration(kvp.Key, x =>
                   {
                       x.AddModifierToken(SyntaxKind.PublicKeyword);

                       foreach (var kind in kvp.Value.Kinds.Where(x => x.Name != "IdentifierToken").Select(x => x.Name).Distinct())
                       {
                           x.AddEnumMemberDeclaration(kind);
                       }
                   })
               )
            );

            _documentRegistry.Documents.Add($"Generated/Enums/{kvp.Key}.cs", compilationUnit);
        }

        foreach (var kvp in _enumRegistry.KindEnums)
        {
            var compilationUnit = CompilationUnitBuilder.CreateSyntax(x => x
               .AddFileScopedNamespaceDeclaration("CSharpComposer", ns =>
                   ns.AddEnumDeclaration(kvp.Key, x =>
                   {
                       x.AddModifierToken(SyntaxKind.PublicKeyword);

                       foreach (var kind in kvp.Value.Kinds.Select(x => x.Name).Distinct())
                       {
                           x.AddEnumMemberDeclaration(kind);
                       }
                   })
               )
            );

            _documentRegistry.Documents.Add($"Generated/Enums/{kvp.Key}.cs", compilationUnit);
        }
    }
}
