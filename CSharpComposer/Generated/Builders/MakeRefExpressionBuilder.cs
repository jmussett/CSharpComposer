using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithMakeRefExpressionBuilder<TBuilder>
{
    TBuilder WithMakeRefExpression(Action<IExpressionBuilder> expressionCallback);
    TBuilder WithMakeRefExpression(MakeRefExpressionSyntax makeRefExpressionSyntax);
}

public partial class MakeRefExpressionBuilder
{
    public static MakeRefExpressionSyntax CreateSyntax(Action<IExpressionBuilder> expressionCallback)
    {
        var keywordToken = SyntaxFactory.Token(SyntaxKind.MakeRefKeyword);
        var openParenTokenToken = SyntaxFactory.Token(SyntaxKind.OpenParenToken);
        var expressionSyntax = ExpressionBuilder.CreateSyntax(expressionCallback);
        var closeParenTokenToken = SyntaxFactory.Token(SyntaxKind.CloseParenToken);
        return SyntaxFactory.MakeRefExpression(keywordToken, openParenTokenToken, expressionSyntax, closeParenTokenToken);
    }
}