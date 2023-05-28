using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface ILambdaExpressionBuilder
{
    void AsSimpleLambdaExpression(string parameterIdentifier, Action<IParameterBuilder> parameterParameterCallback, Action<ISimpleLambdaExpressionBuilder> simpleLambdaExpressionCallback);
    void AsParenthesizedLambdaExpression(Action<IParenthesizedLambdaExpressionBuilder> parenthesizedLambdaExpressionCallback);
}

public partial interface ILambdaExpressionBuilder<TBuilder> : IAnonymousFunctionExpressionBuilder<TBuilder>
{
}

public interface IWithLambdaExpressionBuilder<TBuilder>
{
    TBuilder WithLambdaExpression(Action<ILambdaExpressionBuilder> lambdaExpressionCallback);
    TBuilder WithLambdaExpression(LambdaExpressionSyntax lambdaExpressionSyntax);
}

public partial class LambdaExpressionBuilder : ILambdaExpressionBuilder
{
    public LambdaExpressionSyntax? Syntax { get; set; }

    public static LambdaExpressionSyntax CreateSyntax(Action<ILambdaExpressionBuilder> callback)
    {
        var builder = new LambdaExpressionBuilder();
        callback(builder);
        if (builder.Syntax is null)
        {
            throw new InvalidOperationException("LambdaExpressionSyntax has not been specified");
        }

        return builder.Syntax;
    }

    public void AsSimpleLambdaExpression(string parameterIdentifier, Action<IParameterBuilder> parameterParameterCallback, Action<ISimpleLambdaExpressionBuilder> simpleLambdaExpressionCallback)
    {
        Syntax = SimpleLambdaExpressionBuilder.CreateSyntax(parameterIdentifier, parameterParameterCallback, simpleLambdaExpressionCallback);
    }

    public void AsParenthesizedLambdaExpression(Action<IParenthesizedLambdaExpressionBuilder> parenthesizedLambdaExpressionCallback)
    {
        Syntax = ParenthesizedLambdaExpressionBuilder.CreateSyntax(parenthesizedLambdaExpressionCallback);
    }
}