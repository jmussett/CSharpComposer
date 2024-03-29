﻿using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IAnonymousFunctionExpressionBuilder
{
    void FromSyntax(AnonymousFunctionExpressionSyntax syntax);
    void AsAnonymousMethodExpression(Action<IBlockBuilder> blockBlockCallback, Action<IAnonymousMethodExpressionBuilder> anonymousMethodExpressionCallback);
    void AsSimpleLambdaExpression(string parameterIdentifier, Action<IParameterBuilder> parameterParameterCallback, Action<ISimpleLambdaExpressionBuilder> simpleLambdaExpressionCallback);
    void AsParenthesizedLambdaExpression(Action<IParenthesizedLambdaExpressionBuilder>? parenthesizedLambdaExpressionCallback = null);
}

public partial interface IAnonymousFunctionExpressionBuilder<TBuilder>
{
    TBuilder AddModifierToken(SyntaxKind modifier);
}

internal partial class AnonymousFunctionExpressionBuilder : IAnonymousFunctionExpressionBuilder
{
    public AnonymousFunctionExpressionSyntax? Syntax { get; set; }

    public static AnonymousFunctionExpressionSyntax CreateSyntax(Action<IAnonymousFunctionExpressionBuilder> callback)
    {
        var builder = new AnonymousFunctionExpressionBuilder();
        callback(builder);
        if (builder.Syntax is null)
        {
            throw new InvalidOperationException("AnonymousFunctionExpressionSyntax has not been specified");
        }

        return builder.Syntax;
    }

    public void FromSyntax(AnonymousFunctionExpressionSyntax syntax)
    {
        Syntax = syntax;
    }

    public void AsAnonymousMethodExpression(Action<IBlockBuilder> blockBlockCallback, Action<IAnonymousMethodExpressionBuilder> anonymousMethodExpressionCallback)
    {
        Syntax = AnonymousMethodExpressionBuilder.CreateSyntax(blockBlockCallback, anonymousMethodExpressionCallback);
    }

    public void AsSimpleLambdaExpression(string parameterIdentifier, Action<IParameterBuilder> parameterParameterCallback, Action<ISimpleLambdaExpressionBuilder> simpleLambdaExpressionCallback)
    {
        Syntax = SimpleLambdaExpressionBuilder.CreateSyntax(parameterIdentifier, parameterParameterCallback, simpleLambdaExpressionCallback);
    }

    public void AsParenthesizedLambdaExpression(Action<IParenthesizedLambdaExpressionBuilder>? parenthesizedLambdaExpressionCallback = null)
    {
        Syntax = ParenthesizedLambdaExpressionBuilder.CreateSyntax(parenthesizedLambdaExpressionCallback);
    }
}