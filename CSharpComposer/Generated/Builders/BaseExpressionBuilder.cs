using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithBaseExpression<TBuilder>
{
    TBuilder WithBaseExpression();
    TBuilder WithBaseExpression(BaseExpressionSyntax baseExpressionSyntax);
}

public interface IAddBaseExpression<TBuilder>
{
    TBuilder AddBaseExpression();
    TBuilder AddBaseExpression(BaseExpressionSyntax baseExpressionSyntax);
}

public partial class BaseExpressionBuilder
{
    public static BaseExpressionSyntax CreateSyntax()
    {
        var tokenToken = SyntaxFactory.Token(SyntaxKind.BaseKeyword);
        return SyntaxFactory.BaseExpression(tokenToken);
    }
}