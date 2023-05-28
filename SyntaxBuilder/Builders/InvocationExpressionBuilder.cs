using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SyntaxBuilder.Builders;

public class InvocationExpressionBuilder
{
    public static InvocationExpressionSyntax CreateSyntax(Action<IExpressionBuilder> expressionCallback)
    {
        var expressionSyntax = ExpressionBuilder.CreateSyntax(expressionCallback);

        var syntax = SyntaxFactory.InvocationExpression(expressionSyntax);

        return syntax;
    }

    // TODO: WithArgument
}
