using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithThisExpression<TBuilder>
{
    TBuilder WithThisExpression();
    TBuilder WithThisExpression(ThisExpressionSyntax thisExpressionSyntax);
}

public interface IAddThisExpression<TBuilder>
{
    TBuilder AddThisExpression();
    TBuilder AddThisExpression(ThisExpressionSyntax thisExpressionSyntax);
}

public partial class ThisExpressionBuilder
{
    public static ThisExpressionSyntax CreateSyntax()
    {
        var tokenToken = SyntaxFactory.Token(SyntaxKind.ThisKeyword);
        return SyntaxFactory.ThisExpression(tokenToken);
    }
}