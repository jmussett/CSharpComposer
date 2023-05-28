using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithBaseExpressionBuilder<TBuilder>
{
    TBuilder WithBaseExpression();
    TBuilder WithBaseExpression(BaseExpressionSyntax baseExpressionSyntax);
}

public partial class BaseExpressionBuilder
{
    public static BaseExpressionSyntax CreateSyntax()
    {
        var tokenToken = SyntaxFactory.Token(SyntaxKind.BaseKeyword);
        return SyntaxFactory.BaseExpression(tokenToken);
    }
}