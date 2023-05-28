using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithAssignmentExpressionBuilder<TBuilder>
{
    TBuilder WithAssignmentExpression(AssignmentExpressionKind kind, Action<IExpressionBuilder> leftCallback, Action<IExpressionBuilder> rightCallback);
    TBuilder WithAssignmentExpression(AssignmentExpressionSyntax assignmentExpressionSyntax);
}

public partial class AssignmentExpressionBuilder
{
    public static AssignmentExpressionSyntax CreateSyntax(AssignmentExpressionKind kind, Action<IExpressionBuilder> leftCallback, Action<IExpressionBuilder> rightCallback)
    {
        var syntaxKind = kind switch
        {
            AssignmentExpressionKind.SimpleAssignmentExpression => SyntaxKind.SimpleAssignmentExpression,
            AssignmentExpressionKind.AddAssignmentExpression => SyntaxKind.AddAssignmentExpression,
            AssignmentExpressionKind.SubtractAssignmentExpression => SyntaxKind.SubtractAssignmentExpression,
            AssignmentExpressionKind.MultiplyAssignmentExpression => SyntaxKind.MultiplyAssignmentExpression,
            AssignmentExpressionKind.DivideAssignmentExpression => SyntaxKind.DivideAssignmentExpression,
            AssignmentExpressionKind.ModuloAssignmentExpression => SyntaxKind.ModuloAssignmentExpression,
            AssignmentExpressionKind.AndAssignmentExpression => SyntaxKind.AndAssignmentExpression,
            AssignmentExpressionKind.ExclusiveOrAssignmentExpression => SyntaxKind.ExclusiveOrAssignmentExpression,
            AssignmentExpressionKind.OrAssignmentExpression => SyntaxKind.OrAssignmentExpression,
            AssignmentExpressionKind.LeftShiftAssignmentExpression => SyntaxKind.LeftShiftAssignmentExpression,
            AssignmentExpressionKind.RightShiftAssignmentExpression => SyntaxKind.RightShiftAssignmentExpression,
            AssignmentExpressionKind.UnsignedRightShiftAssignmentExpression => SyntaxKind.UnsignedRightShiftAssignmentExpression,
            AssignmentExpressionKind.CoalesceAssignmentExpression => SyntaxKind.CoalesceAssignmentExpression,
            _ => throw new NotSupportedException()
        };
        var leftSyntax = ExpressionBuilder.CreateSyntax(leftCallback);
        var operatorTokenToken = kind switch
        {
            AssignmentExpressionKind.SimpleAssignmentExpression => SyntaxFactory.Token(SyntaxKind.EqualsToken),
            AssignmentExpressionKind.AddAssignmentExpression => SyntaxFactory.Token(SyntaxKind.PlusEqualsToken),
            AssignmentExpressionKind.SubtractAssignmentExpression => SyntaxFactory.Token(SyntaxKind.MinusEqualsToken),
            AssignmentExpressionKind.MultiplyAssignmentExpression => SyntaxFactory.Token(SyntaxKind.AsteriskEqualsToken),
            AssignmentExpressionKind.DivideAssignmentExpression => SyntaxFactory.Token(SyntaxKind.SlashEqualsToken),
            AssignmentExpressionKind.ModuloAssignmentExpression => SyntaxFactory.Token(SyntaxKind.PercentEqualsToken),
            AssignmentExpressionKind.AndAssignmentExpression => SyntaxFactory.Token(SyntaxKind.AmpersandEqualsToken),
            AssignmentExpressionKind.ExclusiveOrAssignmentExpression => SyntaxFactory.Token(SyntaxKind.CaretEqualsToken),
            AssignmentExpressionKind.OrAssignmentExpression => SyntaxFactory.Token(SyntaxKind.BarEqualsToken),
            AssignmentExpressionKind.LeftShiftAssignmentExpression => SyntaxFactory.Token(SyntaxKind.LessThanLessThanEqualsToken),
            AssignmentExpressionKind.RightShiftAssignmentExpression => SyntaxFactory.Token(SyntaxKind.GreaterThanGreaterThanEqualsToken),
            AssignmentExpressionKind.UnsignedRightShiftAssignmentExpression => SyntaxFactory.Token(SyntaxKind.GreaterThanGreaterThanGreaterThanEqualsToken),
            AssignmentExpressionKind.CoalesceAssignmentExpression => SyntaxFactory.Token(SyntaxKind.QuestionQuestionEqualsToken),
            _ => throw new NotSupportedException()
        };
        var rightSyntax = ExpressionBuilder.CreateSyntax(rightCallback);
        return SyntaxFactory.AssignmentExpression(syntaxKind, leftSyntax, operatorTokenToken, rightSyntax);
    }
}