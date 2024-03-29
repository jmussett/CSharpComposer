﻿using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
internal partial class ShebangDirectiveTriviaBuilder
{
    public static ShebangDirectiveTriviaSyntax CreateSyntax(bool isActive)
    {
        var hashTokenToken = SyntaxFactory.Token(SyntaxKind.HashToken);
        var exclamationTokenToken = SyntaxFactory.Token(SyntaxKind.ExclamationToken);
        var endOfDirectiveTokenToken = SyntaxFactory.Token(SyntaxKind.EndOfDirectiveToken);
        return SyntaxFactory.ShebangDirectiveTrivia(hashTokenToken, exclamationTokenToken, endOfDirectiveTokenToken, isActive);
    }
}