﻿using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
internal partial class CaseSwitchLabelBuilder
{
    public static CaseSwitchLabelSyntax CreateSyntax(Action<IExpressionBuilder> valueCallback)
    {
        var keywordToken = SyntaxFactory.Token(SyntaxKind.CaseKeyword);
        var valueSyntax = ExpressionBuilder.CreateSyntax(valueCallback);
        var colonTokenToken = SyntaxFactory.Token(SyntaxKind.ColonToken);
        return SyntaxFactory.CaseSwitchLabel(keywordToken, valueSyntax, colonTokenToken);
    }
}