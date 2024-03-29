﻿using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
internal partial class WarningDirectiveTriviaBuilder
{
    public static WarningDirectiveTriviaSyntax CreateSyntax(bool isActive)
    {
        var hashTokenToken = SyntaxFactory.Token(SyntaxKind.HashToken);
        var warningKeywordToken = SyntaxFactory.Token(SyntaxKind.WarningKeyword);
        var endOfDirectiveTokenToken = SyntaxFactory.Token(SyntaxKind.EndOfDirectiveToken);
        return SyntaxFactory.WarningDirectiveTrivia(hashTokenToken, warningKeywordToken, endOfDirectiveTokenToken, isActive);
    }
}