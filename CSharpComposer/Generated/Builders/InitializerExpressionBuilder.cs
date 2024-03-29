﻿using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IInitializerExpressionBuilder : IAddExpression<IInitializerExpressionBuilder>
{
}

internal partial class InitializerExpressionBuilder : IInitializerExpressionBuilder
{
    public InitializerExpressionSyntax Syntax { get; set; }

    public InitializerExpressionBuilder(InitializerExpressionSyntax syntax)
    {
        Syntax = syntax;
    }

    public static InitializerExpressionSyntax CreateSyntax(InitializerExpressionKind kind, Action<IInitializerExpressionBuilder>? initializerExpressionCallback = null)
    {
        var syntaxKind = kind switch
        {
            InitializerExpressionKind.ObjectInitializerExpression => SyntaxKind.ObjectInitializerExpression,
            InitializerExpressionKind.CollectionInitializerExpression => SyntaxKind.CollectionInitializerExpression,
            InitializerExpressionKind.ArrayInitializerExpression => SyntaxKind.ArrayInitializerExpression,
            InitializerExpressionKind.ComplexElementInitializerExpression => SyntaxKind.ComplexElementInitializerExpression,
            InitializerExpressionKind.WithInitializerExpression => SyntaxKind.WithInitializerExpression,
            _ => throw new NotSupportedException()
        };
        var openBraceTokenToken = SyntaxFactory.Token(SyntaxKind.OpenBraceToken);
        var closeBraceTokenToken = SyntaxFactory.Token(SyntaxKind.CloseBraceToken);
        var syntax = SyntaxFactory.InitializerExpression(syntaxKind, openBraceTokenToken, default(SeparatedSyntaxList<ExpressionSyntax>), closeBraceTokenToken);
        var builder = new InitializerExpressionBuilder(syntax);
        initializerExpressionCallback?.Invoke(builder);
        return builder.Syntax;
    }

    public IInitializerExpressionBuilder AddExpression(Action<IExpressionBuilder> expressionCallback)
    {
        var expression = ExpressionBuilder.CreateSyntax(expressionCallback);
        Syntax = Syntax.AddExpressions(expression);
        return this;
    }

    public IInitializerExpressionBuilder AddExpression(ExpressionSyntax expression)
    {
        Syntax = Syntax.AddExpressions(expression);
        return this;
    }
}