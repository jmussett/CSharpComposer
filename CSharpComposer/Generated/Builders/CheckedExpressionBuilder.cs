using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithCheckedExpressionBuilder<TBuilder>
{
    TBuilder WithCheckedExpression(CheckedExpressionKind kind, Action<IExpressionBuilder> expressionCallback);
    TBuilder WithCheckedExpression(CheckedExpressionSyntax checkedExpressionSyntax);
}

public partial class CheckedExpressionBuilder
{
    public static CheckedExpressionSyntax CreateSyntax(CheckedExpressionKind kind, Action<IExpressionBuilder> expressionCallback)
    {
        var syntaxKind = kind switch
        {
            CheckedExpressionKind.CheckedExpression => SyntaxKind.CheckedExpression,
            CheckedExpressionKind.UncheckedExpression => SyntaxKind.UncheckedExpression,
            _ => throw new NotSupportedException()
        };
        var keywordToken = kind switch
        {
            CheckedExpressionKind.CheckedExpression => SyntaxFactory.Token(SyntaxKind.CheckedKeyword),
            CheckedExpressionKind.UncheckedExpression => SyntaxFactory.Token(SyntaxKind.UncheckedKeyword),
            _ => throw new NotSupportedException()
        };
        var openParenTokenToken = SyntaxFactory.Token(SyntaxKind.OpenParenToken);
        var expressionSyntax = ExpressionBuilder.CreateSyntax(expressionCallback);
        var closeParenTokenToken = SyntaxFactory.Token(SyntaxKind.CloseParenToken);
        return SyntaxFactory.CheckedExpression(syntaxKind, keywordToken, openParenTokenToken, expressionSyntax, closeParenTokenToken);
    }
}