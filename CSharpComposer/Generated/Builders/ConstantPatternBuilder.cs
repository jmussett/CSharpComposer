﻿using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithConstantPattern<TBuilder>
{
    TBuilder WithConstantPattern(Action<IExpressionBuilder> expressionCallback);
    TBuilder WithConstantPattern(ConstantPatternSyntax constantPatternSyntax);
}

public interface IAddConstantPattern<TBuilder>
{
    TBuilder AddConstantPattern(Action<IExpressionBuilder> expressionCallback);
    TBuilder AddConstantPattern(ConstantPatternSyntax constantPatternSyntax);
}

public partial class ConstantPatternBuilder
{
    public static ConstantPatternSyntax CreateSyntax(Action<IExpressionBuilder> expressionCallback)
    {
        var expressionSyntax = ExpressionBuilder.CreateSyntax(expressionCallback);
        return SyntaxFactory.ConstantPattern(expressionSyntax);
    }
}