using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithPostfixUnaryExpressionBuilder<TBuilder>
{
    TBuilder WithPostfixUnaryExpression(PostfixUnaryExpressionKind kind, Action<IExpressionBuilder> operandCallback);
    TBuilder WithPostfixUnaryExpression(PostfixUnaryExpressionSyntax postfixUnaryExpressionSyntax);
}

public partial class PostfixUnaryExpressionBuilder
{
    public static PostfixUnaryExpressionSyntax CreateSyntax(PostfixUnaryExpressionKind kind, Action<IExpressionBuilder> operandCallback)
    {
        var syntaxKind = kind switch
        {
            PostfixUnaryExpressionKind.PostIncrementExpression => SyntaxKind.PostIncrementExpression,
            PostfixUnaryExpressionKind.PostDecrementExpression => SyntaxKind.PostDecrementExpression,
            PostfixUnaryExpressionKind.SuppressNullableWarningExpression => SyntaxKind.SuppressNullableWarningExpression,
            _ => throw new NotSupportedException()
        };
        var operandSyntax = ExpressionBuilder.CreateSyntax(operandCallback);
        var operatorTokenToken = kind switch
        {
            PostfixUnaryExpressionKind.PostIncrementExpression => SyntaxFactory.Token(SyntaxKind.PlusPlusToken),
            PostfixUnaryExpressionKind.PostDecrementExpression => SyntaxFactory.Token(SyntaxKind.MinusMinusToken),
            PostfixUnaryExpressionKind.SuppressNullableWarningExpression => SyntaxFactory.Token(SyntaxKind.ExclamationToken),
            _ => throw new NotSupportedException()
        };
        return SyntaxFactory.PostfixUnaryExpression(syntaxKind, operandSyntax, operatorTokenToken);
    }
}