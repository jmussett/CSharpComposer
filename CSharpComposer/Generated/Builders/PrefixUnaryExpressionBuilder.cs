using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithPrefixUnaryExpressionBuilder<TBuilder>
{
    TBuilder WithPrefixUnaryExpression(PrefixUnaryExpressionKind kind, Action<IExpressionBuilder> operandCallback);
    TBuilder WithPrefixUnaryExpression(PrefixUnaryExpressionSyntax prefixUnaryExpressionSyntax);
}

public partial class PrefixUnaryExpressionBuilder
{
    public static PrefixUnaryExpressionSyntax CreateSyntax(PrefixUnaryExpressionKind kind, Action<IExpressionBuilder> operandCallback)
    {
        var syntaxKind = kind switch
        {
            PrefixUnaryExpressionKind.UnaryPlusExpression => SyntaxKind.UnaryPlusExpression,
            PrefixUnaryExpressionKind.UnaryMinusExpression => SyntaxKind.UnaryMinusExpression,
            PrefixUnaryExpressionKind.BitwiseNotExpression => SyntaxKind.BitwiseNotExpression,
            PrefixUnaryExpressionKind.LogicalNotExpression => SyntaxKind.LogicalNotExpression,
            PrefixUnaryExpressionKind.PreIncrementExpression => SyntaxKind.PreIncrementExpression,
            PrefixUnaryExpressionKind.PreDecrementExpression => SyntaxKind.PreDecrementExpression,
            PrefixUnaryExpressionKind.AddressOfExpression => SyntaxKind.AddressOfExpression,
            PrefixUnaryExpressionKind.PointerIndirectionExpression => SyntaxKind.PointerIndirectionExpression,
            PrefixUnaryExpressionKind.IndexExpression => SyntaxKind.IndexExpression,
            _ => throw new NotSupportedException()
        };
        var operatorTokenToken = kind switch
        {
            PrefixUnaryExpressionKind.UnaryPlusExpression => SyntaxFactory.Token(SyntaxKind.PlusToken),
            PrefixUnaryExpressionKind.UnaryMinusExpression => SyntaxFactory.Token(SyntaxKind.MinusToken),
            PrefixUnaryExpressionKind.BitwiseNotExpression => SyntaxFactory.Token(SyntaxKind.TildeToken),
            PrefixUnaryExpressionKind.LogicalNotExpression => SyntaxFactory.Token(SyntaxKind.ExclamationToken),
            PrefixUnaryExpressionKind.PreIncrementExpression => SyntaxFactory.Token(SyntaxKind.PlusPlusToken),
            PrefixUnaryExpressionKind.PreDecrementExpression => SyntaxFactory.Token(SyntaxKind.MinusMinusToken),
            PrefixUnaryExpressionKind.AddressOfExpression => SyntaxFactory.Token(SyntaxKind.AmpersandToken),
            PrefixUnaryExpressionKind.PointerIndirectionExpression => SyntaxFactory.Token(SyntaxKind.AsteriskToken),
            PrefixUnaryExpressionKind.IndexExpression => SyntaxFactory.Token(SyntaxKind.CaretToken),
            _ => throw new NotSupportedException()
        };
        var operandSyntax = ExpressionBuilder.CreateSyntax(operandCallback);
        return SyntaxFactory.PrefixUnaryExpression(syntaxKind, operatorTokenToken, operandSyntax);
    }
}