using System;

namespace CSharpComposer;

public static class AddStatementExtensions
{
    public static TBuilder AddBlockStatement<TBuilder>(this TBuilder builder, Action<IBlockBuilder>? blockCallback = null) 
        where TBuilder : IAddStatement<TBuilder>
    {
        return builder.AddStatement(x => x.AsBlock(blockCallback));
    }

    public static TBuilder AddLocalFunctionStatement<TBuilder>(this TBuilder builder, Action<ITypeBuilder> returnTypeCallback, string identifier, Action<ILocalFunctionStatementBuilder>? localFunctionStatementCallback = null)
        where TBuilder : IAddStatement<TBuilder>
    {
        return builder.AddStatement(x => x.AsLocalFunctionStatement(returnTypeCallback, identifier, localFunctionStatementCallback));
    }

    public static TBuilder AddLocalDeclarationStatement<TBuilder>(this TBuilder builder, Action<ITypeBuilder> declarationTypeCallback, Action<IVariableDeclarationBuilder> declarationVariableDeclarationCallback, Action<ILocalDeclarationStatementBuilder> localDeclarationStatementCallback)
        where TBuilder : IAddStatement<TBuilder>
    {
        return builder.AddStatement(x => x.AsLocalDeclarationStatement(declarationTypeCallback, declarationVariableDeclarationCallback, localDeclarationStatementCallback));
    }

    public static TBuilder AddExpressionStatement<TBuilder>(this TBuilder builder, Action<IExpressionBuilder> expressionCallback, Action<IExpressionStatementBuilder>? expressionStatementCallback = null)
        where TBuilder : IAddStatement<TBuilder>
    {
        return builder.AddStatement(x => x.AsExpressionStatement(expressionCallback, expressionStatementCallback));
    }

    public static TBuilder AddEmptyStatement<TBuilder>(this TBuilder builder, Action<IEmptyStatementBuilder>? emptyStatementCallback = null)
        where TBuilder : IAddStatement<TBuilder>
    {
        return builder.AddStatement(x => x.AsEmptyStatement(emptyStatementCallback));
    }

    public static TBuilder AddLabeledStatement<TBuilder>(this TBuilder builder, string identifier, Action<IStatementBuilder> statementCallback, Action<ILabeledStatementBuilder>? labeledStatementCallback = null)
        where TBuilder : IAddStatement<TBuilder>
    {
        return builder.AddStatement(x => x.AsLabeledStatement(identifier, statementCallback, labeledStatementCallback));
    }

    public static TBuilder AddGotoStatement<TBuilder>(this TBuilder builder, GotoStatementKind kind, Action<IGotoStatementBuilder>? gotoStatementCallback = null)
        where TBuilder : IAddStatement<TBuilder>
    {
        return builder.AddStatement(x => x.AsGotoStatement(kind, gotoStatementCallback));
    }

    public static TBuilder AddBreakStatement<TBuilder>(this TBuilder builder, Action<IBreakStatementBuilder>? breakStatementCallback = null)
        where TBuilder : IAddStatement<TBuilder>
    {
        return builder.AddStatement(x => x.AsBreakStatement(breakStatementCallback));
    }

    public static TBuilder AddContinueStatement<TBuilder>(this TBuilder builder, Action<IContinueStatementBuilder>? continueStatementCallback = null)
        where TBuilder : IAddStatement<TBuilder>
    {
        return builder.AddStatement(x => x.AsContinueStatement(continueStatementCallback));
    }

    public static TBuilder AddReturnStatement<TBuilder>(this TBuilder builder, Action<IReturnStatementBuilder>? returnStatementCallback = null)
        where TBuilder : IAddStatement<TBuilder>
    {
        return builder.AddStatement(x => x.AsReturnStatement(returnStatementCallback));
    }

    public static TBuilder AddThrowStatement<TBuilder>(this TBuilder builder, Action<IThrowStatementBuilder>? throwStatementCallback = null)
        where TBuilder : IAddStatement<TBuilder>
    {
        return builder.AddStatement(x => x.AsThrowStatement(throwStatementCallback));
    }

    public static TBuilder AddYieldStatement<TBuilder>(this TBuilder builder, YieldStatementKind kind, Action<IYieldStatementBuilder>? yieldStatementCallback = null)
        where TBuilder : IAddStatement<TBuilder>
    {
        return builder.AddStatement(x => x.AsYieldStatement(kind, yieldStatementCallback));
    }

    public static TBuilder AddWhileStatement<TBuilder>(this TBuilder builder, Action<IExpressionBuilder> conditionCallback, Action<IStatementBuilder> statementCallback, Action<IWhileStatementBuilder>? whileStatementCallback = null)
        where TBuilder : IAddStatement<TBuilder>
    {
        return builder.AddStatement(x => x.AsWhileStatement(conditionCallback, statementCallback, whileStatementCallback));
    }

    public static TBuilder AddDoStatement<TBuilder>(this TBuilder builder, Action<IStatementBuilder> statementCallback, Action<IExpressionBuilder> conditionCallback, Action<IDoStatementBuilder>? doStatementCallback = null)
        where TBuilder : IAddStatement<TBuilder>
    {
        return builder.AddStatement(x => x.AsDoStatement(statementCallback, conditionCallback, doStatementCallback));
    }

    public static TBuilder AddForStatement<TBuilder>(this TBuilder builder, Action<IStatementBuilder> statementCallback, Action<IForStatementBuilder>? forStatementCallback = null)
        where TBuilder : IAddStatement<TBuilder>
    {
        return builder.AddStatement(x => x.AsForStatement(statementCallback, forStatementCallback));
    }

    public static TBuilder AddForEachStatement<TBuilder>(this TBuilder builder, Action<ITypeBuilder> typeCallback, string identifier, Action<IExpressionBuilder> expressionCallback, Action<IStatementBuilder> statementCallback, Action<IForEachStatementBuilder>? forEachStatementCallback = null)
        where TBuilder : IAddStatement<TBuilder>
    {
        return builder.AddStatement(x => x.AsForEachStatement(typeCallback, identifier, expressionCallback, statementCallback, forEachStatementCallback));
    }

    public static TBuilder AddForEachVariableStatement<TBuilder>(this TBuilder builder, Action<IExpressionBuilder> variableCallback, Action<IExpressionBuilder> expressionCallback, Action<IStatementBuilder> statementCallback, Action<IForEachVariableStatementBuilder>? forEachVariableStatementCallback = null)
        where TBuilder : IAddStatement<TBuilder>
    {
        return builder.AddStatement(x => x.AsForEachVariableStatement(variableCallback, expressionCallback, statementCallback, forEachVariableStatementCallback));
    }

    public static TBuilder AddUsingStatement<TBuilder>(this TBuilder builder, Action<IStatementBuilder> statementCallback, Action<IUsingStatementBuilder>? usingStatementCallback = null)
        where TBuilder : IAddStatement<TBuilder>
    {
        return builder.AddStatement(x => x.AsUsingStatement(statementCallback, usingStatementCallback));
    }

    public static TBuilder AddFixedStatement<TBuilder>(this TBuilder builder, Action<ITypeBuilder> declarationTypeCallback, Action<IVariableDeclarationBuilder> declarationVariableDeclarationCallback, Action<IStatementBuilder> statementCallback, Action<IFixedStatementBuilder> fixedStatementCallback)
        where TBuilder : IAddStatement<TBuilder>
    {
        return builder.AddStatement(x => x.AsFixedStatement(declarationTypeCallback, declarationVariableDeclarationCallback, statementCallback, fixedStatementCallback));
    }

    public static TBuilder AddCheckedStatement<TBuilder>(this TBuilder builder, CheckedStatementKind kind, Action<IBlockBuilder> blockBlockCallback, Action<ICheckedStatementBuilder> checkedStatementCallback)
        where TBuilder : IAddStatement<TBuilder>
    {
        return builder.AddStatement(x => x.AsCheckedStatement(kind, blockBlockCallback, checkedStatementCallback));
    }

    public static TBuilder AddUnsafeStatement<TBuilder>(this TBuilder builder, Action<IBlockBuilder> blockBlockCallback, Action<IUnsafeStatementBuilder> unsafeStatementCallback)
        where TBuilder : IAddStatement<TBuilder>
    {
        return builder.AddStatement(x => x.AsUnsafeStatement(blockBlockCallback, unsafeStatementCallback));
    }

    public static TBuilder AddLockStatement<TBuilder>(this TBuilder builder, Action<IExpressionBuilder> expressionCallback, Action<IStatementBuilder> statementCallback, Action<ILockStatementBuilder>? lockStatementCallback = null)
        where TBuilder : IAddStatement<TBuilder>
    {
        return builder.AddStatement(x => x.AsLockStatement(expressionCallback, statementCallback, lockStatementCallback));
    }

    public static TBuilder AddIfStatement<TBuilder>(this TBuilder builder, Action<IExpressionBuilder> conditionCallback, Action<IStatementBuilder> statementCallback, Action<IIfStatementBuilder>? ifStatementCallback = null)
        where TBuilder : IAddStatement<TBuilder>
    {
        return builder.AddStatement(x => x.AsIfStatement(conditionCallback, statementCallback, ifStatementCallback));
    }

    public static TBuilder AddSwitchStatement<TBuilder>(this TBuilder builder, Action<IExpressionBuilder> expressionCallback, Action<ISwitchStatementBuilder>? switchStatementCallback = null)
        where TBuilder : IAddStatement<TBuilder>
    {
        return builder.AddStatement(x => x.AsSwitchStatement(expressionCallback, switchStatementCallback));
    }

    public static TBuilder AddTryStatement<TBuilder>(this TBuilder builder, Action<IBlockBuilder> blockBlockCallback, Action<ITryStatementBuilder> tryStatementCallback)
        where TBuilder : IAddStatement<TBuilder>
    {
        return builder.AddStatement(x => x.AsTryStatement(blockBlockCallback, tryStatementCallback));
    }

}
