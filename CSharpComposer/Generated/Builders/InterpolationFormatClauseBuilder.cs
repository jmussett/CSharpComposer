﻿using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
internal partial class InterpolationFormatClauseBuilder
{
    public static InterpolationFormatClauseSyntax CreateSyntax()
    {
        var colonTokenToken = SyntaxFactory.Token(SyntaxKind.ColonToken);
        var formatStringTokenToken = SyntaxFactory.Token(SyntaxKind.InterpolatedStringTextToken);
        return SyntaxFactory.InterpolationFormatClause(colonTokenToken, formatStringTokenToken);
    }
}