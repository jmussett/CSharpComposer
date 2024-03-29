﻿using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
internal partial class DiscardPatternBuilder
{
    public static DiscardPatternSyntax CreateSyntax()
    {
        var underscoreTokenToken = SyntaxFactory.Token(SyntaxKind.UnderscoreToken);
        return SyntaxFactory.DiscardPattern(underscoreTokenToken);
    }
}