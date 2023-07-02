using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.MSBuild;
using CSharpComposer.Generator;
using CSharpComposer.Generator.Models;
using System.Xml.Serialization;
using Microsoft.CodeAnalysis.FindSymbols;
using Microsoft.CodeAnalysis;
using CSharpComposer;
using CSharpComposer.Extensions;
using Microsoft.CodeAnalysis.CSharp;

// Configured based on version of Microsoft.CodeAnalysis.CSharp
var registryUrl = "https://raw.githubusercontent.com/dotnet/roslyn/Visual-Studio-2022-Version-17.5/src/Compilers/CSharp/Portable/Syntax/Syntax.xml";
using var httpClient = new HttpClient();

Console.WriteLine("Fetching XML...");

var registryXml = await httpClient.GetStringAsync(registryUrl);

var serializer = new XmlSerializer(typeof(Tree));
using var reader = new StringReader(registryXml);
var tree = (Tree)serializer.Deserialize(reader)!;

var workspace = MSBuildWorkspace.Create();

var project = await workspace.OpenProjectAsync("..\\..\\..\\..\\CSharpComposer\\CSharpComposer.csproj");

var documentRegistry = new Dictionary<string, CompilationUnitSyntax>();

var enumStore = new EnumStore();

var parametersBuilder = new ParametersBuilder(tree, enumStore);
var argumentsBuilder = new ArgumentsBuilder(tree, enumStore);
var methodBuilder = new MethodBuilder(tree, enumStore, parametersBuilder, argumentsBuilder);
var interfaceBuilder = new InterfaceBuilder(tree, methodBuilder, parametersBuilder);
var implementationBuilder = new ImplementationBuilder(tree, parametersBuilder, argumentsBuilder, methodBuilder);

Console.WriteLine("Generating C#...");

CreateBuilders(interfaceBuilder, implementationBuilder);

CreateFactory();

CreateEnums(enumStore);

foreach (var documentEntry in documentRegistry)
{
    var documentName = documentEntry.Key;
    var compilationUnit = documentEntry.Value;

    compilationUnit = compilationUnit.NormalizeWhitespace();

    //compilationUnit = SimplifierAnnotator.Annotate(compilationUnit);

    var document = project.AddDocument(documentName, compilationUnit);

    var formattedDocument = await Formatter.FormatAsync(document);
    var formattedText = await formattedDocument.GetTextAsync();

    var updatedDocument = formattedDocument.Project
        .GetDocument(formattedDocument.Id)!
        .WithText(formattedText);

    //var simplifiedDocument = await Simplifier.ReduceAsync(updatedDocument);

    project = updatedDocument.Project;
}

project = await RemoveUnusedInterfaces(project);

Console.WriteLine("Applying changes...");

workspace.TryApplyChanges(project.Solution);

Console.WriteLine("Generation complete.");

void CreateBuilders(InterfaceBuilder interfaceBuilder, ImplementationBuilder implementationBuilder)
{
    foreach (var type in tree.Types)
    {
        // TODO: Skip identifier syntax (like FunctionPointerUnmanagedCallingConventionSyntax, NameColonSyntax)
        if (!NodeValidator.IsValidNode(type.Name))
        {
            continue;
        }

        var compilationUnit = CSharpFactory.CompilationUnit(x => x
            .AddUsingDirective(x => x.ParseName("System"), x => { })
            .AddUsingDirective(x => x.ParseName("Microsoft.CodeAnalysis"), x => { }) // TODO: ???
            .AddUsingDirective(x => x.ParseName("Microsoft.CodeAnalysis.CSharp"), x => { })
            .AddUsingDirective(x => x.ParseName("Microsoft.CodeAnalysis.CSharp.Syntax"), x => { })
            .AddFileScopedNamespaceDeclaration(x => x.ParseName("CSharpComposer"), ns =>
            {
                // TODO: Seperate into different files (don't want to expose internal types when navigating)
                ns = interfaceBuilder.WithInterfaces(ns, type);
                ns = implementationBuilder.WithImplementation(ns, type);
            })
        );

        var builderName = NameFactory.CreateBuilderName(type.Name!);

        documentRegistry.Add($"Generated/Builders/{builderName}.cs", compilationUnit);
    }
}

async Task<Project> RemoveUnusedInterfaces(Project project)
{
    var compilation = await project.GetCompilationAsync();
    var unusedInterfaces = await GetUnusedInterfacesAsync(compilation, project);

    var interfacesByCompilation = unusedInterfaces.GroupBy(
        i => i.FirstAncestorOrSelf<CompilationUnitSyntax>()
    );

    foreach (var compilationGroup in interfacesByCompilation)
    {
        var document = project.GetDocument(compilationGroup.Key.SyntaxTree);
        var root = await document.GetSyntaxRootAsync();

        // Remove all unused interfaces from the document at once
        var newRoot = root.RemoveNodes(compilationGroup, SyntaxRemoveOptions.KeepNoTrivia);

        document = document.WithSyntaxRoot(newRoot);
        project = document.Project;
    }

    return project;
}

static async Task<IEnumerable<InterfaceDeclarationSyntax>> GetUnusedInterfacesAsync(Compilation compilation, Project project)
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
            var interfaceDeclaration = (InterfaceDeclarationSyntax) await interfaceReference.GetSyntaxAsync();

            unusedInterfaces.Add(interfaceDeclaration);
        }
    }

    return unusedInterfaces;
}

void CreateFactory()
{
    var compilationUnit = CSharpFactory.CompilationUnit(x => x
        .AddUsingDirective(x => x.ParseName("Microsoft.CodeAnalysis.CSharp.Syntax"), x => { })
        .AddFileScopedNamespaceDeclaration(x => x.ParseName("CSharpComposer"), ns =>
        {
            ns.AddClassDeclaration("CSharpFactory", x =>
            {
                x.AddModifierToken(SyntaxFactory.Token(SyntaxKind.PublicKeyword));
                x.AddModifierToken(SyntaxFactory.Token(SyntaxKind.StaticKeyword));

                foreach (var type in tree.Types)
                {
                    // TODO: Skip identifier syntax (like FunctionPointerUnmanagedCallingConventionSyntax, NameColonSyntax)
                    if (!NodeValidator.IsValidNode(type.Name))
                    {
                        continue;
                    }

                    var typeName = NameFactory.CreateTypeName(type.Name);
                    var builderName = NameFactory.CreateBuilderName(type.Name);

                    x.AddMethodDeclaration(x => x.ParseTypeName(type.Name), typeName, x => 
                    {
                        x.AddModifierToken(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                            .AddModifierToken(SyntaxFactory.Token(SyntaxKind.StaticKeyword));

                        if (type is AbstractNode || NodeValidator.IsTokenized(type))
                        {
                            x.AddParameter("callback", x => x.WithType(x => x.ParseTypeName($"Action<I{builderName}>")));

                            x.WithBody(x =>
                            {
                                x.AddStatement($"return {builderName}.CreateSyntax(callback);");
                            });
                        }

                        if (type is Node node && !NodeValidator.IsTokenized(type))
                        {
                            parametersBuilder.WithParameters(x, node);

                            x.WithBody(x =>
                            {
                                var arguments = argumentsBuilder.WithArguments(x, node, false);

                                x.AddStatement($"return {builderName}.CreateSyntax({string.Join(", ", arguments)});");
                            });
                        }
                    });
                }
            });
        })
    );

    documentRegistry.Add($"Generated/CSharpFactory.cs", compilationUnit);

}

void CreateEnums(EnumStore enumStore)
{
    foreach (var kvp in enumStore.FieldEnums)
    {
        var compilationUnit = CompilationUnitBuilder.CreateSyntax(x => x
           .AddFileScopedNamespaceDeclaration(x => x.ParseName("CSharpComposer"), ns =>
               ns.AddEnumDeclaration(kvp.Key, x =>
               {
                   x.AddModifierToken(SyntaxFactory.Token(SyntaxKind.PublicKeyword));

                   foreach (var kind in kvp.Value.Kinds.Where(x => x.Name != "IdentifierToken").Select(x => x.Name).Distinct())
                   {
                       x.AddEnumMemberDeclaration(kind, x => { });
                   }
               })
           )
        );

        documentRegistry.Add($"Generated/Enums/{kvp.Key}.cs", compilationUnit);
    }

    foreach (var kvp in enumStore.KindEnums)
    {
        var compilationUnit = CompilationUnitBuilder.CreateSyntax(x => x
           .AddFileScopedNamespaceDeclaration(x => x.ParseName("CSharpComposer"), ns =>
               ns.AddEnumDeclaration(kvp.Key, x =>
               {
                   x.AddModifierToken(SyntaxFactory.Token(SyntaxKind.PublicKeyword));

                   foreach (var kind in kvp.Value.Kinds.Select(x => x.Name).Distinct())
                   {
                       x.AddEnumMemberDeclaration(kind, x => { });
                   }
               })
           )
        );

        documentRegistry.Add($"Generated/Enums/{kvp.Key}.cs", compilationUnit);
    }
}