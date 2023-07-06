using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithExplicitInterfaceSpecifier<TBuilder>
{
    TBuilder WithExplicitInterfaceSpecifier(ExplicitInterfaceSpecifierSyntax explicitInterfaceSpecifierSyntax);
    TBuilder WithExplicitInterfaceSpecifier(Action<INameBuilder> nameCallback);
}

public partial class ExplicitInterfaceSpecifierBuilder
{
    public static ExplicitInterfaceSpecifierSyntax CreateSyntax(Action<INameBuilder> nameCallback)
    {
        var nameSyntax = NameBuilder.CreateSyntax(nameCallback);
        var dotTokenToken = SyntaxFactory.Token(SyntaxKind.DotToken);
        return SyntaxFactory.ExplicitInterfaceSpecifier(nameSyntax, dotTokenToken);
    }
}