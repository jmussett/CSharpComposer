using CSharpComposer.Generator.Builders;
using CSharpComposer.Generator.Models;
using CSharpComposer.Generator.Registries;
using CSharpComposer.Generator.Utility;
using Microsoft.CodeAnalysis.CSharp;

namespace CSharpComposer.Generator.Generators;

internal class CSharpFactoryGenerator
{
    private readonly CSharpRegistry _csharpRegistry;
    private readonly ParametersBuilder _parametersBuilder;
    private readonly ArgumentsBuilder _argumentsBuilder;
    private readonly DocumentRegistry _documentRegistry;

    public CSharpFactoryGenerator(CSharpRegistry csharpRegistry, ParametersBuilder parametersBuilder, ArgumentsBuilder argumentsBuilder, DocumentRegistry documentRegistry)
    {
        _csharpRegistry = csharpRegistry;
        _parametersBuilder = parametersBuilder;
        _argumentsBuilder = argumentsBuilder;
        _documentRegistry = documentRegistry;
    }

    public void GenerateCSharpFactory()
    {
        var compilationUnit = CSharpFactory.CompilationUnit(x => x
            .AddUsingDirective("Microsoft.CodeAnalysis.CSharp.Syntax")
            .AddFileScopedNamespaceDeclaration("CSharpComposer", ns =>
            {
                ns.AddClassDeclaration("CSharpFactory", x =>
                {
                    x.AddModifierToken(SyntaxFactory.Token(SyntaxKind.PublicKeyword));
                    x.AddModifierToken(SyntaxFactory.Token(SyntaxKind.StaticKeyword));
                    x.AddModifierToken(SyntaxFactory.Token(SyntaxKind.PartialKeyword));

                    foreach (var type in _csharpRegistry.Tree.Types)
                    {
                        // TODO: Skip identifier syntax (like FunctionPointerUnmanagedCallingConventionSyntax, NameColonSyntax)
                        if (!NodeValidator.IsValidNode(type.Name))
                        {
                            continue;
                        }

                        var typeName = NameFactory.CreateTypeName(type.Name);
                        var builderName = NameFactory.CreateBuilderName(type.Name);

                        x.AddMethodDeclaration(type.Name, typeName, x =>
                        {
                            x.AddModifierToken(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                                .AddModifierToken(SyntaxFactory.Token(SyntaxKind.StaticKeyword));

                            if (type is AbstractNode || NodeValidator.IsTokenized(type))
                            {
                                x.AddParameter("callback", x => x.WithType($"Action<I{builderName}>"));

                                x.WithBody(x =>
                                {
                                    x.AddStatement($"return {builderName}.CreateSyntax(callback);");
                                });
                            }

                            if (type is Node node && !NodeValidator.IsTokenized(type))
                            {
                                _parametersBuilder.WithParameters(x, node);

                                x.WithBody(x =>
                                {
                                    var arguments = _argumentsBuilder.WithArguments(x, node, false);

                                    x.AddStatement($"return {builderName}.CreateSyntax({string.Join(", ", arguments)});");
                                });
                            }
                        });
                    }
                });
            })
        );

        _documentRegistry.Documents.Add($"Generated/CSharpFactory.cs", compilationUnit);
    }
}
