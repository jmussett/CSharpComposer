using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SyntaxBuilder.Builders;

public static class ReturnStatementBuilder
{

    public static ReturnStatementSyntax CreateSyntax(Action<IExpressionBuilder> expressionCallback)
    {
        var expressionSyntax = ExpressionBuilder.CreateSyntax(expressionCallback);

        return SyntaxFactory.ReturnStatement(expressionSyntax);
    }
}