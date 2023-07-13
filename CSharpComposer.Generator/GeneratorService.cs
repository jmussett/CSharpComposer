using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FindSymbols;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Hosting;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.CodeAnalysis.Formatting;
using CSharpComposer.Generator.Registries;
using CSharpComposer.Generator.Generators;

namespace CSharpComposer.Generator;

internal class GeneratorService : BackgroundService
{
    private readonly DocumentRegistry _documentRegistry;
    private readonly BuilderGenerator _builderGenerator;
    private readonly CSharpFactoryGenerator _factoryGenerator;
    private readonly EnumGenerator _enumGenerator;
    private readonly CSharpRegistry _csharpRegistry;

    public GeneratorService(DocumentRegistry documentRegistry, BuilderGenerator builderGenerator, CSharpFactoryGenerator factoryGenerator, EnumGenerator enumGenerator, CSharpRegistry csharpRegistry)
    {
        _documentRegistry = documentRegistry;
        _builderGenerator = builderGenerator;
        _factoryGenerator = factoryGenerator;
        _enumGenerator = enumGenerator;
        _csharpRegistry = csharpRegistry;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _csharpRegistry.FetchRegistryAsync();

        var workspace = MSBuildWorkspace.Create();

        var project = await workspace.OpenProjectAsync("..\\..\\..\\..\\CSharpComposer\\CSharpComposer.csproj");

        Console.WriteLine("Generating C#...");

        _builderGenerator.GenerateBuilders();
        _factoryGenerator.GenerateCSharpFactory();
        _enumGenerator.GenerateEnums();

        foreach (var documentEntry in _documentRegistry.Documents)
        {
            var documentName = documentEntry.Key;
            var compilationUnit = documentEntry.Value;

            compilationUnit = compilationUnit.NormalizeWhitespace();

            var document = project.AddDocument(documentName, compilationUnit);

            var formattedDocument = await Formatter.FormatAsync(document);
            var formattedText = await formattedDocument.GetTextAsync();

            var updatedDocument = formattedDocument.Project
                .GetDocument(formattedDocument.Id)!
                .WithText(formattedText);

            project = updatedDocument.Project;
        }

        project = await RemoveUnusedInterfaces(project);

        Console.WriteLine("Applying changes...");

        workspace.TryApplyChanges(project.Solution);

        Console.WriteLine("Generation complete.");
    }

    public async Task<Project> RemoveUnusedInterfaces(Project project)
    {
        var compilation = await project.GetCompilationAsync()
            ?? throw new InvalidOperationException("Unable to compile CSharpComposer");

        var unusedInterfaces = await GetUnusedInterfacesAsync(compilation, project);

        var interfacesByCompilation = unusedInterfaces.GroupBy(
            i => i.FirstAncestorOrSelf<CompilationUnitSyntax>()!
        );

        foreach (var compilationGroup in interfacesByCompilation)
        {
            Document document = project.GetDocument(compilationGroup.Key.SyntaxTree)
                ?? throw new InvalidOperationException($"Unable to find document for syntax tree: {compilationGroup.Key.SyntaxTree}");

            var root = await document.GetSyntaxRootAsync()
                ?? throw new InvalidOperationException($"Unable to find root node for document '{document.Name}'");

            // Remove all unused interfaces from the document at once
            var newRoot = root.RemoveNodes(compilationGroup, SyntaxRemoveOptions.KeepNoTrivia)
                ?? throw new InvalidOperationException($"Unable to remove unused interfaces from document '{document.Name}'"); ;

            document = document.WithSyntaxRoot(newRoot);
            project = document.Project;
        }

        return project;
    }

    public async Task<IEnumerable<InterfaceDeclarationSyntax>> GetUnusedInterfacesAsync(Compilation compilation, Project project)
    {
        var unusedInterfaces = new List<InterfaceDeclarationSyntax>();

        var interfaceSymbols = compilation.GetSymbolsWithName(symbol => true, SymbolFilter.Type)
            .OfType<ITypeSymbol>()
            .Where(symbol => symbol.TypeKind == TypeKind.Interface);

        foreach (var interfaceSymbol in interfaceSymbols)
        {
            var references = await SymbolFinder.FindReferencesAsync(interfaceSymbol, project.Solution);
            if (!references.Any() || !references.First().Locations.Any())
            {
                var interfaceReference = interfaceSymbol.DeclaringSyntaxReferences.First();
                var interfaceDeclaration = (InterfaceDeclarationSyntax)await interfaceReference.GetSyntaxAsync();

                unusedInterfaces.Add(interfaceDeclaration);
            }
        }

        return unusedInterfaces;
    }


}
