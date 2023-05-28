using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithExplicitInterfaceSpecifierBuilder<TBuilder>
{
    TBuilder WithExplicitInterfaceSpecifier(Action<INameBuilder> nameCallback);
    TBuilder WithExplicitInterfaceSpecifier(ExplicitInterfaceSpecifierSyntax explicitInterfaceSpecifierSyntax);
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