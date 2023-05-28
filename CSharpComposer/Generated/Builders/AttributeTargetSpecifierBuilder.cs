using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithAttributeTargetSpecifierBuilder<TBuilder>
{
    TBuilder WithAttributeTargetSpecifier(string identifier);
    TBuilder WithAttributeTargetSpecifier(AttributeTargetSpecifierSyntax attributeTargetSpecifierSyntax);
}

public partial class AttributeTargetSpecifierBuilder
{
    public static AttributeTargetSpecifierSyntax CreateSyntax(string identifier)
    {
        var identifierToken = SyntaxFactory.Identifier(identifier);
        var colonTokenToken = SyntaxFactory.Token(SyntaxKind.ColonToken);
        return SyntaxFactory.AttributeTargetSpecifier(identifierToken, colonTokenToken);
    }
}