﻿using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithParenthesizedPattern<TBuilder>
{
    TBuilder WithParenthesizedPattern(Action<IPatternBuilder> patternCallback);
    TBuilder WithParenthesizedPattern(ParenthesizedPatternSyntax parenthesizedPatternSyntax);
}

public interface IAddParenthesizedPattern<TBuilder>
{
    TBuilder AddParenthesizedPattern(Action<IPatternBuilder> patternCallback);
    TBuilder AddParenthesizedPattern(ParenthesizedPatternSyntax parenthesizedPatternSyntax);
}

public partial class ParenthesizedPatternBuilder
{
    public static ParenthesizedPatternSyntax CreateSyntax(Action<IPatternBuilder> patternCallback)
    {
        var openParenTokenToken = SyntaxFactory.Token(SyntaxKind.OpenParenToken);
        var patternSyntax = PatternBuilder.CreateSyntax(patternCallback);
        var closeParenTokenToken = SyntaxFactory.Token(SyntaxKind.CloseParenToken);
        return SyntaxFactory.ParenthesizedPattern(openParenTokenToken, patternSyntax, closeParenTokenToken);
    }
}