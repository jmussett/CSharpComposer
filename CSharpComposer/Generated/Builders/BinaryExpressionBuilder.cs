using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithBinaryExpressionBuilder<TBuilder>
{
    TBuilder WithBinaryExpression(BinaryExpressionKind kind, Action<IExpressionBuilder> leftCallback, Action<IExpressionBuilder> rightCallback);
    TBuilder WithBinaryExpression(BinaryExpressionSyntax binaryExpressionSyntax);
}

public partial class BinaryExpressionBuilder
{
    public static BinaryExpressionSyntax CreateSyntax(BinaryExpressionKind kind, Action<IExpressionBuilder> leftCallback, Action<IExpressionBuilder> rightCallback)
    {
        var syntaxKind = kind switch
        {
            BinaryExpressionKind.AddExpression => SyntaxKind.AddExpression,
            BinaryExpressionKind.SubtractExpression => SyntaxKind.SubtractExpression,
            BinaryExpressionKind.MultiplyExpression => SyntaxKind.MultiplyExpression,
            BinaryExpressionKind.DivideExpression => SyntaxKind.DivideExpression,
            BinaryExpressionKind.ModuloExpression => SyntaxKind.ModuloExpression,
            BinaryExpressionKind.LeftShiftExpression => SyntaxKind.LeftShiftExpression,
            BinaryExpressionKind.RightShiftExpression => SyntaxKind.RightShiftExpression,
            BinaryExpressionKind.UnsignedRightShiftExpression => SyntaxKind.UnsignedRightShiftExpression,
            BinaryExpressionKind.LogicalOrExpression => SyntaxKind.LogicalOrExpression,
            BinaryExpressionKind.LogicalAndExpression => SyntaxKind.LogicalAndExpression,
            BinaryExpressionKind.BitwiseOrExpression => SyntaxKind.BitwiseOrExpression,
            BinaryExpressionKind.BitwiseAndExpression => SyntaxKind.BitwiseAndExpression,
            BinaryExpressionKind.ExclusiveOrExpression => SyntaxKind.ExclusiveOrExpression,
            BinaryExpressionKind.EqualsExpression => SyntaxKind.EqualsExpression,
            BinaryExpressionKind.NotEqualsExpression => SyntaxKind.NotEqualsExpression,
            BinaryExpressionKind.LessThanExpression => SyntaxKind.LessThanExpression,
            BinaryExpressionKind.LessThanOrEqualExpression => SyntaxKind.LessThanOrEqualExpression,
            BinaryExpressionKind.GreaterThanExpression => SyntaxKind.GreaterThanExpression,
            BinaryExpressionKind.GreaterThanOrEqualExpression => SyntaxKind.GreaterThanOrEqualExpression,
            BinaryExpressionKind.IsExpression => SyntaxKind.IsExpression,
            BinaryExpressionKind.AsExpression => SyntaxKind.AsExpression,
            BinaryExpressionKind.CoalesceExpression => SyntaxKind.CoalesceExpression,
            _ => throw new NotSupportedException()
        };
        var leftSyntax = ExpressionBuilder.CreateSyntax(leftCallback);
        var operatorTokenToken = kind switch
        {
            BinaryExpressionKind.AddExpression => SyntaxFactory.Token(SyntaxKind.PlusToken),
            BinaryExpressionKind.SubtractExpression => SyntaxFactory.Token(SyntaxKind.MinusToken),
            BinaryExpressionKind.MultiplyExpression => SyntaxFactory.Token(SyntaxKind.AsteriskToken),
            BinaryExpressionKind.DivideExpression => SyntaxFactory.Token(SyntaxKind.SlashToken),
            BinaryExpressionKind.ModuloExpression => SyntaxFactory.Token(SyntaxKind.PercentToken),
            BinaryExpressionKind.LeftShiftExpression => SyntaxFactory.Token(SyntaxKind.LessThanLessThanToken),
            BinaryExpressionKind.RightShiftExpression => SyntaxFactory.Token(SyntaxKind.GreaterThanGreaterThanToken),
            BinaryExpressionKind.UnsignedRightShiftExpression => SyntaxFactory.Token(SyntaxKind.GreaterThanGreaterThanGreaterThanToken),
            BinaryExpressionKind.LogicalOrExpression => SyntaxFactory.Token(SyntaxKind.BarBarToken),
            BinaryExpressionKind.LogicalAndExpression => SyntaxFactory.Token(SyntaxKind.AmpersandAmpersandToken),
            BinaryExpressionKind.BitwiseOrExpression => SyntaxFactory.Token(SyntaxKind.BarToken),
            BinaryExpressionKind.BitwiseAndExpression => SyntaxFactory.Token(SyntaxKind.AmpersandToken),
            BinaryExpressionKind.ExclusiveOrExpression => SyntaxFactory.Token(SyntaxKind.CaretToken),
            BinaryExpressionKind.EqualsExpression => SyntaxFactory.Token(SyntaxKind.EqualsEqualsToken),
            BinaryExpressionKind.NotEqualsExpression => SyntaxFactory.Token(SyntaxKind.ExclamationEqualsToken),
            BinaryExpressionKind.LessThanExpression => SyntaxFactory.Token(SyntaxKind.LessThanToken),
            BinaryExpressionKind.LessThanOrEqualExpression => SyntaxFactory.Token(SyntaxKind.LessThanEqualsToken),
            BinaryExpressionKind.GreaterThanExpression => SyntaxFactory.Token(SyntaxKind.GreaterThanToken),
            BinaryExpressionKind.GreaterThanOrEqualExpression => SyntaxFactory.Token(SyntaxKind.GreaterThanEqualsToken),
            BinaryExpressionKind.IsExpression => SyntaxFactory.Token(SyntaxKind.IsKeyword),
            BinaryExpressionKind.AsExpression => SyntaxFactory.Token(SyntaxKind.AsKeyword),
            BinaryExpressionKind.CoalesceExpression => SyntaxFactory.Token(SyntaxKind.QuestionQuestionToken),
            _ => throw new NotSupportedException()
        };
        var rightSyntax = ExpressionBuilder.CreateSyntax(rightCallback);
        return SyntaxFactory.BinaryExpression(syntaxKind, leftSyntax, operatorTokenToken, rightSyntax);
    }
}