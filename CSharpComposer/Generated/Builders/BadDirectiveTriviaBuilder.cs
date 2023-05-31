﻿using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithBadDirectiveTrivia<TBuilder>
{
    TBuilder WithBadDirectiveTrivia(string identifier, bool isActive);
    TBuilder WithBadDirectiveTrivia(BadDirectiveTriviaSyntax badDirectiveTriviaSyntax);
}

public interface IAddBadDirectiveTrivia<TBuilder>
{
    TBuilder AddBadDirectiveTrivia(string identifier, bool isActive);
    TBuilder AddBadDirectiveTrivia(BadDirectiveTriviaSyntax badDirectiveTriviaSyntax);
}

public partial class BadDirectiveTriviaBuilder
{
    public static BadDirectiveTriviaSyntax CreateSyntax(string identifier, bool isActive)
    {
        var hashTokenToken = SyntaxFactory.Token(SyntaxKind.HashToken);
        var identifierToken = SyntaxFactory.Identifier(identifier);
        var endOfDirectiveTokenToken = SyntaxFactory.Token(SyntaxKind.EndOfDirectiveToken);
        return SyntaxFactory.BadDirectiveTrivia(hashTokenToken, identifierToken, endOfDirectiveTokenToken, isActive);
    }
}