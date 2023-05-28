using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.MSBuild;
using SyntaxBuilder.Builders;
using CSharpComposer.Generator;
using CSharpComposer.Generator.Models;
using System.Xml.Serialization;
using SyntaxBuilder.Types;
using Microsoft.CodeAnalysis.CSharp;

// Configured based on version of Microsoft.CodeAnalysis.CSharp
var registryUrl = "https://raw.githubusercontent.com/dotnet/roslyn/Visual-Studio-2022-Version-17.5/src/Compilers/CSharp/Portable/Syntax/Syntax.xml";
using var httpClient = new HttpClient();

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
var implementationBuilder = new ImplementationBuilder(parametersBuilder, argumentsBuilder, methodBuilder);

CreateBuilders(interfaceBuilder, implementationBuilder);

CreateFactory();

CreateEnums(enumStore);

foreach (var documentEntry in documentRegistry)
{
    var documentName = documentEntry.Key;
    var compilationUnit = documentEntry.Value;

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

workspace.TryApplyChanges(project.Solution);

void CreateBuilders(InterfaceBuilder interfaceBuilder, ImplementationBuilder implementationBuilder)
{
    foreach (var type in tree.Types)
    {
        // TODO: Skip identifier syntax (like FunctionPointerUnmanagedCallingConventionSyntax, NameColonSyntax)
        if (!NodeValidator.IsValidNode(type.Name))
        {
            continue;
        }

        var compilationUnit = CompilationUnitBuilder.CreateSyntax(x => x
            .WithUsing("System")
            .WithUsing("Microsoft.CodeAnalysis") // TODO: ???
            .WithUsing("Microsoft.CodeAnalysis.CSharp")
            .WithUsing("Microsoft.CodeAnalysis.CSharp.Syntax")
            .WithFileScopedNamespace("CSharpComposer", ns =>
            {
                // TODO: Seperate into different files (don't want to expose internal types when navigating)
                ns = interfaceBuilder.WithInterfaces(ns, type);
                ns = implementationBuilder.WithImplementation(ns, type);

                return ns;
            })
        );

        var builderName = NameFactory.CreateBuilderName(type.Name!);

        documentRegistry.Add($"Generated/Builders/{builderName}.cs", compilationUnit);
    }
}

void CreateFactory()
{
    var compilationUnit = CompilationUnitBuilder.CreateSyntax(x => x
        .WithUsing("Microsoft.CodeAnalysis.CSharp.Syntax")
        .WithFileScopedNamespace("CSharpComposer", ns =>
        {
            ns.WithClass("CSharpFactory", x =>
            {
                x.WithAccessModifier(TypeAccessModifier.Public);
                x.WithStaticModifier();

                foreach (var type in tree.Types)
                {
                    // TODO: Skip identifier syntax (like FunctionPointerUnmanagedCallingConventionSyntax, NameColonSyntax)
                    if (!NodeValidator.IsValidNode(type.Name))
                    {
                        continue;
                    }

                    var typeName = NameFactory.CreateTypeName(type.Name);
                    var builderName = NameFactory.CreateBuilderName(type.Name);

                    x.WithMethod(typeName, x => x.AsType(type.Name), x => 
                    {
                        x.WithAccessModifier(MemberAccessModifier.Public)
                            .WithStaticModifier();

                        if (type is AbstractNode || NodeValidator.IsTokenized(type))
                        {
                            x.WithParameter("callback", $"Action<I{builderName}>");

                            x.WithBody(x =>
                            {
                                x.WithStatement($"return {builderName}.CreateSyntax(callback);");
                                return x;
                            });
                        }

                        if (type is Node node && !NodeValidator.IsTokenized(type))
                        {
                            parametersBuilder.WithParameters(x, node);

                            x.WithBody(x =>
                            {
                                var arguments = argumentsBuilder.WithArguments(x, node, false);

                                x.WithStatement($"return {builderName}.CreateSyntax({string.Join(", ", arguments)});");

                                return x;
                            });
                        }

                        

                        return x;
                    });
                }

                return x;
            });

            return ns;
        })
    );

    documentRegistry.Add($"Generated/CSharpFactory.cs", compilationUnit);

}

void CreateEnums(EnumStore enumStore)
{
    foreach (var kvp in enumStore.FieldEnums)
    {
        var compilationUnit = CompilationUnitBuilder.CreateSyntax(x => x
           .WithFileScopedNamespace("CSharpComposer", ns =>
               ns.WithEnum(kvp.Key, x =>
               {
                   x.WithAccessModifier(SyntaxBuilder.Types.TypeAccessModifier.Public);

                   foreach (var kind in kvp.Value.Kinds.Where(x => x.Name != "IdentifierToken").Select(x => x.Name).Distinct())
                   {
                       x.WithMember(kind);
                   }

                   return x;
               })
           )
        );

        documentRegistry.Add($"Generated/Enums/{kvp.Key}.cs", compilationUnit);
    }

    foreach (var kvp in enumStore.KindEnums)
    {
        var compilationUnit = CompilationUnitBuilder.CreateSyntax(x => x
           .WithFileScopedNamespace("CSharpComposer", ns =>
               ns.WithEnum(kvp.Key, x =>
               {
                   x.WithAccessModifier(SyntaxBuilder.Types.TypeAccessModifier.Public);

                   foreach (var kind in kvp.Value.Kinds.Select(x => x.Name).Distinct())
                   {
                       x.WithMember(kind);
                   }

                   return x;
               })
           )
        );

        documentRegistry.Add($"Generated/Enums/{kvp.Key}.cs", compilationUnit);
    }
}