﻿using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithConditionalAccessExpression<TBuilder>
{
    TBuilder WithConditionalAccessExpression(Action<IExpressionBuilder> expressionCallback, Action<IExpressionBuilder> whenNotNullCallback);
    TBuilder WithConditionalAccessExpression(ConditionalAccessExpressionSyntax conditionalAccessExpressionSyntax);
}

public interface IAddConditionalAccessExpression<TBuilder>
{
    TBuilder AddConditionalAccessExpression(Action<IExpressionBuilder> expressionCallback, Action<IExpressionBuilder> whenNotNullCallback);
    TBuilder AddConditionalAccessExpression(ConditionalAccessExpressionSyntax conditionalAccessExpressionSyntax);
}

public partial class ConditionalAccessExpressionBuilder
{
    public static ConditionalAccessExpressionSyntax CreateSyntax(Action<IExpressionBuilder> expressionCallback, Action<IExpressionBuilder> whenNotNullCallback)
    {
        var expressionSyntax = ExpressionBuilder.CreateSyntax(expressionCallback);
        var operatorTokenToken = SyntaxFactory.Token(SyntaxKind.QuestionToken);
        var whenNotNullSyntax = ExpressionBuilder.CreateSyntax(whenNotNullCallback);
        return SyntaxFactory.ConditionalAccessExpression(expressionSyntax, operatorTokenToken, whenNotNullSyntax);
    }
}