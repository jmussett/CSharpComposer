﻿using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithThrowExpression<TBuilder>
{
    TBuilder WithThrowExpression(Action<IExpressionBuilder> expressionCallback);
    TBuilder WithThrowExpression(ThrowExpressionSyntax throwExpressionSyntax);
}

public interface IAddThrowExpression<TBuilder>
{
    TBuilder AddThrowExpression(Action<IExpressionBuilder> expressionCallback);
    TBuilder AddThrowExpression(ThrowExpressionSyntax throwExpressionSyntax);
}

public partial class ThrowExpressionBuilder
{
    public static ThrowExpressionSyntax CreateSyntax(Action<IExpressionBuilder> expressionCallback)
    {
        var throwKeywordToken = SyntaxFactory.Token(SyntaxKind.ThrowKeyword);
        var expressionSyntax = ExpressionBuilder.CreateSyntax(expressionCallback);
        return SyntaxFactory.ThrowExpression(throwKeywordToken, expressionSyntax);
    }
}