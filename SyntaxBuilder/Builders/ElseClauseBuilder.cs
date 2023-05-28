using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SyntaxBuilder.Builders;

public interface IElseClauseBuilder
{
    IElseClauseBuilder WithElse(Func<IBlockBuilder, IBlockBuilder> blockCallback);
    IElseClauseBuilder WithElseIf(
        Action<IExpressionBuilder> expressionCallback,
        Func<IBlockBuilder, IBlockBuilder> blockCallback,
        Func<IElseClauseBuilder, IElseClauseBuilder>? elseCallback = null
    );
}

public sealed class ElseClauseBuilder : IElseClauseBuilder
{
    public ElseClauseSyntax? Syntax { get; set; }

    public ElseClauseBuilder() { }

    public static ElseClauseSyntax CreateSyntax(Func<IElseClauseBuilder, IElseClauseBuilder> elseCallback)
    {
        var builder = new ElseClauseBuilder();

        builder = (ElseClauseBuilder)elseCallback(builder);

        if (builder.Syntax is null)
        {
            throw new InvalidOperationException("Else clause not specified.");
        }

        return builder.Syntax;
    }

    public IElseClauseBuilder WithElse(Func<IBlockBuilder, IBlockBuilder> blockCallback)
    {
        var blockSyntax = BlockBuilder.CreateSyntax(blockCallback);

        Syntax = SyntaxFactory.ElseClause(blockSyntax);

        return this;
    }

    public IElseClauseBuilder WithElseIf(
        Action<IExpressionBuilder> expressionCallback,
        Func<IBlockBuilder, IBlockBuilder> blockCallback,
        Func<IElseClauseBuilder, IElseClauseBuilder>? elseCallback = null)
    {
        // Move to IfStatementBuilder

        var expressionSyntax = ExpressionBuilder.CreateSyntax(expressionCallback);

        var blockSyntax = BlockBuilder.CreateSyntax(blockCallback);

        var ifStatementSyntax = SyntaxFactory.IfStatement(
            expressionSyntax,
            blockSyntax
        );

        if (elseCallback is not null)
        {
            var elseClasuseSyntax = CreateSyntax(elseCallback);
            ifStatementSyntax = ifStatementSyntax.WithElse(elseClasuseSyntax);
        }

        Syntax = SyntaxFactory.ElseClause(ifStatementSyntax);

        return this;
    }
}