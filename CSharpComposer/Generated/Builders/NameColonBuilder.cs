using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithNameColon<TBuilder>
{
    TBuilder WithNameColon(NameColonSyntax nameColonSyntax);
    TBuilder WithNameColon(string nameIdentifier);
}

internal partial class NameColonBuilder
{
    public static NameColonSyntax CreateSyntax(string nameIdentifier)
    {
        var nameSyntax = IdentifierNameBuilder.CreateSyntax(nameIdentifier);
        var colonTokenToken = SyntaxFactory.Token(SyntaxKind.ColonToken);
        return SyntaxFactory.NameColon(nameSyntax, colonTokenToken);
    }
}