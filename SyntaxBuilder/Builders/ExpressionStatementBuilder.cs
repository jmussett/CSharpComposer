using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SyntaxBuilder.Builders;

public static class ExpressionStatementBuilder
{
    public static ExpressionStatementSyntax CreateSyntax(Action<IExpressionBuilder> expressionCallback)
    {
        var expressionSyntax = ExpressionBuilder.CreateSyntax(expressionCallback);

        return SyntaxFactory.ExpressionStatement(expressionSyntax);
    }
}