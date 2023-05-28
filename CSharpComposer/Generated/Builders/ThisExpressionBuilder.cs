using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithThisExpressionBuilder<TBuilder>
{
    TBuilder WithThisExpression();
    TBuilder WithThisExpression(ThisExpressionSyntax thisExpressionSyntax);
}

public partial class ThisExpressionBuilder
{
    public static ThisExpressionSyntax CreateSyntax()
    {
        var tokenToken = SyntaxFactory.Token(SyntaxKind.ThisKeyword);
        return SyntaxFactory.ThisExpression(tokenToken);
    }
}