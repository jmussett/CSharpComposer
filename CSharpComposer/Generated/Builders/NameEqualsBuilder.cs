﻿using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithNameEquals<TBuilder>
{
    TBuilder WithNameEquals(string nameIdentifier);
    TBuilder WithNameEquals(NameEqualsSyntax nameEqualsSyntax);
}

public interface IAddNameEquals<TBuilder>
{
    TBuilder AddNameEquals(string nameIdentifier);
    TBuilder AddNameEquals(NameEqualsSyntax nameEqualsSyntax);
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