﻿using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
internal partial class EndIfDirectiveTriviaBuilder
{
    public static EndIfDirectiveTriviaSyntax CreateSyntax(bool isActive)
    {
        var hashTokenToken = SyntaxFactory.Token(SyntaxKind.HashToken);
        var endIfKeywordToken = SyntaxFactory.Token(SyntaxKind.EndIfKeyword);
        var endOfDirectiveTokenToken = SyntaxFactory.Token(SyntaxKind.EndOfDirectiveToken);
        return SyntaxFactory.EndIfDirectiveTrivia(hashTokenToken, endIfKeywordToken, endOfDirectiveTokenToken, isActive);
    }
}