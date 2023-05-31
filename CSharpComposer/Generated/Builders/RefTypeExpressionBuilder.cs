using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithRefTypeExpression<TBuilder>
{
    TBuilder WithRefTypeExpression(Action<IExpressionBuilder> expressionCallback);
    TBuilder WithRefTypeExpression(RefTypeExpressionSyntax refTypeExpressionSyntax);
}

public interface IAddRefTypeExpression<TBuilder>
{
    TBuilder AddRefTypeExpression(Action<IExpressionBuilder> expressionCallback);
    TBuilder AddRefTypeExpression(RefTypeExpressionSyntax refTypeExpressionSyntax);
}

public partial class RefTypeExpressionBuilder
{
    public static RefTypeExpressionSyntax CreateSyntax(Action<IExpressionBuilder> expressionCallback)
    {
        var keywordToken = SyntaxFactory.Token(SyntaxKind.RefTypeKeyword);
        var openParenTokenToken = SyntaxFactory.Token(SyntaxKind.OpenParenToken);
        var expressionSyntax = ExpressionBuilder.CreateSyntax(expressionCallback);
        var closeParenTokenToken = SyntaxFactory.Token(SyntaxKind.CloseParenToken);
        return SyntaxFactory.RefTypeExpression(keywordToken, openParenTokenToken, expressionSyntax, closeParenTokenToken);
    }
}