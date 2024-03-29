﻿using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
internal partial class ReferenceDirectiveTriviaBuilder
{
    public static ReferenceDirectiveTriviaSyntax CreateSyntax(string file, bool isActive)
    {
        var hashTokenToken = SyntaxFactory.Token(SyntaxKind.HashToken);
        var referenceKeywordToken = SyntaxFactory.Token(SyntaxKind.ReferenceKeyword);
        var fileToken = SyntaxFactory.Literal(file);
        var endOfDirectiveTokenToken = SyntaxFactory.Token(SyntaxKind.EndOfDirectiveToken);
        return SyntaxFactory.ReferenceDirectiveTrivia(hashTokenToken, referenceKeywordToken, fileToken, endOfDirectiveTokenToken, isActive);
    }
}