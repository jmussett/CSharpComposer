using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithRefTypeExpressionBuilder<TBuilder>
{
    TBuilder WithRefTypeExpression(Action<IExpressionBuilder> expressionCallback);
    TBuilder WithRefTypeExpression(RefTypeExpressionSyntax refTypeExpressionSyntax);
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