using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithRefExpressionBuilder<TBuilder>
{
    TBuilder WithRefExpression(Action<IExpressionBuilder> expressionCallback);
    TBuilder WithRefExpression(RefExpressionSyntax refExpressionSyntax);
}

public partial class RefExpressionBuilder
{
    public static RefExpressionSyntax CreateSyntax(Action<IExpressionBuilder> expressionCallback)
    {
        var refKeywordToken = SyntaxFactory.Token(SyntaxKind.RefKeyword);
        var expressionSyntax = ExpressionBuilder.CreateSyntax(expressionCallback);
        return SyntaxFactory.RefExpression(refKeywordToken, expressionSyntax);
    }
}