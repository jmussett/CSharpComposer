using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithNameEqualsBuilder<TBuilder>
{
    TBuilder WithNameEquals(string nameIdentifier);
    TBuilder WithNameEquals(NameEqualsSyntax nameEqualsSyntax);
}

public partial class NameEqualsBuilder
{
    public static NameEqualsSyntax CreateSyntax(string nameIdentifier)
    {
        var nameSyntax = IdentifierNameBuilder.CreateSyntax(nameIdentifier);
        var equalsTokenToken = SyntaxFactory.Token(SyntaxKind.EqualsToken);
        return SyntaxFactory.NameEquals(nameSyntax, equalsTokenToken);
    }
}