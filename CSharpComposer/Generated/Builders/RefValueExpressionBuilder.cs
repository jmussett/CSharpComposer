using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithRefValueExpressionBuilder<TBuilder>
{
    TBuilder WithRefValueExpression(Action<IExpressionBuilder> expressionCallback, Action<ITypeBuilder> typeCallback);
    TBuilder WithRefValueExpression(RefValueExpressionSyntax refValueExpressionSyntax);
}

public partial class RefValueExpressionBuilder
{
    public static RefValueExpressionSyntax CreateSyntax(Action<IExpressionBuilder> expressionCallback, Action<ITypeBuilder> typeCallback)
    {
        var keywordToken = SyntaxFactory.Token(SyntaxKind.RefValueKeyword);
        var openParenTokenToken = SyntaxFactory.Token(SyntaxKind.OpenParenToken);
        var expressionSyntax = ExpressionBuilder.CreateSyntax(expressionCallback);
        var commaToken = SyntaxFactory.Token(SyntaxKind.CommaToken);
        var typeSyntax = TypeBuilder.CreateSyntax(typeCallback);
        var closeParenTokenToken = SyntaxFactory.Token(SyntaxKind.CloseParenToken);
        return SyntaxFactory.RefValueExpression(keywordToken, openParenTokenToken, expressionSyntax, commaToken, typeSyntax, closeParenTokenToken);
    }
}