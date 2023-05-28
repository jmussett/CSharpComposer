using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithCastExpressionBuilder<TBuilder>
{
    TBuilder WithCastExpression(Action<ITypeBuilder> typeCallback, Action<IExpressionBuilder> expressionCallback);
    TBuilder WithCastExpression(CastExpressionSyntax castExpressionSyntax);
}

public partial class CastExpressionBuilder
{
    public static CastExpressionSyntax CreateSyntax(Action<ITypeBuilder> typeCallback, Action<IExpressionBuilder> expressionCallback)
    {
        var openParenTokenToken = SyntaxFactory.Token(SyntaxKind.OpenParenToken);
        var typeSyntax = TypeBuilder.CreateSyntax(typeCallback);
        var closeParenTokenToken = SyntaxFactory.Token(SyntaxKind.CloseParenToken);
        var expressionSyntax = ExpressionBuilder.CreateSyntax(expressionCallback);
        return SyntaxFactory.CastExpression(openParenTokenToken, typeSyntax, closeParenTokenToken, expressionSyntax);
    }
}