using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SyntaxBuilder.Builders;

internal static class IfStatementBuilder
{

    public static IfStatementSyntax CreateSyntax(Action<IExpressionBuilder> expressionCallback, Func<IBlockBuilder, IBlockBuilder> blockCallback, Func<IElseClauseBuilder, IElseClauseBuilder>? elseCallback)
    {
        var expressionSyntax = ExpressionBuilder.CreateSyntax(expressionCallback);

        var blockSyntax = BlockBuilder.CreateSyntax(blockCallback);

        var ifStatementSyntax = SyntaxFactory.IfStatement(
            expressionSyntax,
            blockSyntax
        );

        if (elseCallback is not null)
        {
            var elseClasuseSyntax = ElseClauseBuilder.CreateSyntax(elseCallback);
            ifStatementSyntax = ifStatementSyntax.WithElse(elseClasuseSyntax);
        }

        return ifStatementSyntax;
    }
}