using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IStatementBuilder
{
    void AsBlock(Action<IBlockBuilder> blockCallback);
    void AsLocalFunctionStatement(Action<ITypeBuilder> returnTypeCallback, string identifier, Action<ILocalFunctionStatementBuilder> localFunctionStatementCallback);
    void AsLocalDeclarationStatement(Action<ITypeBuilder> declarationTypeCallback, Action<IVariableDeclarationBuilder> declarationVariableDeclarationCallback, Action<ILocalDeclarationStatementBuilder> localDeclarationStatementCallback);
    void AsExpressionStatement(Action<IExpressionBuilder> expressionCallback, Action<IExpressionStatementBuilder> expressionStatementCallback);
    void AsEmptyStatement(Action<IEmptyStatementBuilder> emptyStatementCallback);
    void AsLabeledStatement(string identifier, Action<IStatementBuilder> statementCallback, Action<ILabeledStatementBuilder> labeledStatementCallback);
    void AsGotoStatement(GotoStatementKind kind, Action<IGotoStatementBuilder> gotoStatementCallback);
    void AsBreakStatement(Action<IBreakStatementBuilder> breakStatementCallback);
    void AsContinueStatement(Action<IContinueStatementBuilder> continueStatementCallback);
    void AsReturnStatement(Action<IReturnStatementBuilder> returnStatementCallback);
    void AsThrowStatement(Action<IThrowStatementBuilder> throwStatementCallback);
    void AsYieldStatement(YieldStatementKind kind, Action<IYieldStatementBuilder> yieldStatementCallback);
    void AsWhileStatement(Action<IExpressionBuilder> conditionCallback, Action<IStatementBuilder> statementCallback, Action<IWhileStatementBuilder> whileStatementCallback);
    void AsDoStatement(Action<IStatementBuilder> statementCallback, Action<IExpressionBuilder> conditionCallback, Action<IDoStatementBuilder> doStatementCallback);
    void AsForStatement(Action<IStatementBuilder> statementCallback, Action<IForStatementBuilder> forStatementCallback);
    void AsForEachStatement(Action<ITypeBuilder> typeCallback, string identifier, Action<IExpressionBuilder> expressionCallback, Action<IStatementBuilder> statementCallback, Action<IForEachStatementBuilder> forEachStatementCallback);
    void AsForEachVariableStatement(Action<IExpressionBuilder> variableCallback, Action<IExpressionBuilder> expressionCallback, Action<IStatementBuilder> statementCallback, Action<IForEachVariableStatementBuilder> forEachVariableStatementCallback);
    void AsUsingStatement(Action<IStatementBuilder> statementCallback, Action<IUsingStatementBuilder> usingStatementCallback);
    void AsFixedStatement(Action<ITypeBuilder> declarationTypeCallback, Action<IVariableDeclarationBuilder> declarationVariableDeclarationCallback, Action<IStatementBuilder> statementCallback, Action<IFixedStatementBuilder> fixedStatementCallback);
    void AsCheckedStatement(CheckedStatementKind kind, Action<IBlockBuilder> blockBlockCallback, Action<ICheckedStatementBuilder> checkedStatementCallback);
    void AsUnsafeStatement(Action<IBlockBuilder> blockBlockCallback, Action<IUnsafeStatementBuilder> unsafeStatementCallback);
    void AsLockStatement(Action<IExpressionBuilder> expressionCallback, Action<IStatementBuilder> statementCallback, Action<ILockStatementBuilder> lockStatementCallback);
    void AsIfStatement(Action<IExpressionBuilder> conditionCallback, Action<IStatementBuilder> statementCallback, Action<IIfStatementBuilder> ifStatementCallback);
    void AsSwitchStatement(Action<IExpressionBuilder> expressionCallback, Action<ISwitchStatementBuilder> switchStatementCallback);
    void AsTryStatement(Action<IBlockBuilder> blockBlockCallback, Action<ITryStatementBuilder> tryStatementCallback);
}

public partial interface IStatementBuilder<TBuilder>
{
}

public interface IWithStatementBuilder<TBuilder>
{
    TBuilder WithStatement(Action<IStatementBuilder> statementCallback);
    TBuilder WithStatement(StatementSyntax statementSyntax);
}

public partial class StatementBuilder : IStatementBuilder
{
    public StatementSyntax? Syntax { get; set; }

    public static StatementSyntax CreateSyntax(Action<IStatementBuilder> callback)
    {
        var builder = new StatementBuilder();
        callback(builder);
        if (builder.Syntax is null)
        {
            throw new InvalidOperationException("StatementSyntax has not been specified");
        }

        return builder.Syntax;
    }

    public void AsBlock(Action<IBlockBuilder> blockCallback)
    {
        Syntax = BlockBuilder.CreateSyntax(blockCallback);
    }

    public void AsLocalFunctionStatement(Action<ITypeBuilder> returnTypeCallback, string identifier, Action<ILocalFunctionStatementBuilder> localFunctionStatementCallback)
    {
        Syntax = LocalFunctionStatementBuilder.CreateSyntax(returnTypeCallback, identifier, localFunctionStatementCallback);
    }

    public void AsLocalDeclarationStatement(Action<ITypeBuilder> declarationTypeCallback, Action<IVariableDeclarationBuilder> declarationVariableDeclarationCallback, Action<ILocalDeclarationStatementBuilder> localDeclarationStatementCallback)
    {
        Syntax = LocalDeclarationStatementBuilder.CreateSyntax(declarationTypeCallback, declarationVariableDeclarationCallback, localDeclarationStatementCallback);
    }

    public void AsExpressionStatement(Action<IExpressionBuilder> expressionCallback, Action<IExpressionStatementBuilder> expressionStatementCallback)
    {
        Syntax = ExpressionStatementBuilder.CreateSyntax(expressionCallback, expressionStatementCallback);
    }

    public void AsEmptyStatement(Action<IEmptyStatementBuilder> emptyStatementCallback)
    {
        Syntax = EmptyStatementBuilder.CreateSyntax(emptyStatementCallback);
    }

    public void AsLabeledStatement(string identifier, Action<IStatementBuilder> statementCallback, Action<ILabeledStatementBuilder> labeledStatementCallback)
    {
        Syntax = LabeledStatementBuilder.CreateSyntax(identifier, statementCallback, labeledStatementCallback);
    }

    public void AsGotoStatement(GotoStatementKind kind, Action<IGotoStatementBuilder> gotoStatementCallback)
    {
        Syntax = GotoStatementBuilder.CreateSyntax(kind, gotoStatementCallback);
    }

    public void AsBreakStatement(Action<IBreakStatementBuilder> breakStatementCallback)
    {
        Syntax = BreakStatementBuilder.CreateSyntax(breakStatementCallback);
    }

    public void AsContinueStatement(Action<IContinueStatementBuilder> continueStatementCallback)
    {
        Syntax = ContinueStatementBuilder.CreateSyntax(continueStatementCallback);
    }

    public void AsReturnStatement(Action<IReturnStatementBuilder> returnStatementCallback)
    {
        Syntax = ReturnStatementBuilder.CreateSyntax(returnStatementCallback);
    }

    public void AsThrowStatement(Action<IThrowStatementBuilder> throwStatementCallback)
    {
        Syntax = ThrowStatementBuilder.CreateSyntax(throwStatementCallback);
    }

    public void AsYieldStatement(YieldStatementKind kind, Action<IYieldStatementBuilder> yieldStatementCallback)
    {
        Syntax = YieldStatementBuilder.CreateSyntax(kind, yieldStatementCallback);
    }

    public void AsWhileStatement(Action<IExpressionBuilder> conditionCallback, Action<IStatementBuilder> statementCallback, Action<IWhileStatementBuilder> whileStatementCallback)
    {
        Syntax = WhileStatementBuilder.CreateSyntax(conditionCallback, statementCallback, whileStatementCallback);
    }

    public void AsDoStatement(Action<IStatementBuilder> statementCallback, Action<IExpressionBuilder> conditionCallback, Action<IDoStatementBuilder> doStatementCallback)
    {
        Syntax = DoStatementBuilder.CreateSyntax(statementCallback, conditionCallback, doStatementCallback);
    }

    public void AsForStatement(Action<IStatementBuilder> statementCallback, Action<IForStatementBuilder> forStatementCallback)
    {
        Syntax = ForStatementBuilder.CreateSyntax(statementCallback, forStatementCallback);
    }

    public void AsForEachStatement(Action<ITypeBuilder> typeCallback, string identifier, Action<IExpressionBuilder> expressionCallback, Action<IStatementBuilder> statementCallback, Action<IForEachStatementBuilder> forEachStatementCallback)
    {
        Syntax = ForEachStatementBuilder.CreateSyntax(typeCallback, identifier, expressionCallback, statementCallback, forEachStatementCallback);
    }

    public void AsForEachVariableStatement(Action<IExpressionBuilder> variableCallback, Action<IExpressionBuilder> expressionCallback, Action<IStatementBuilder> statementCallback, Action<IForEachVariableStatementBuilder> forEachVariableStatementCallback)
    {
        Syntax = ForEachVariableStatementBuilder.CreateSyntax(variableCallback, expressionCallback, statementCallback, forEachVariableStatementCallback);
    }

    public void AsUsingStatement(Action<IStatementBuilder> statementCallback, Action<IUsingStatementBuilder> usingStatementCallback)
    {
        Syntax = UsingStatementBuilder.CreateSyntax(statementCallback, usingStatementCallback);
    }

    public void AsFixedStatement(Action<ITypeBuilder> declarationTypeCallback, Action<IVariableDeclarationBuilder> declarationVariableDeclarationCallback, Action<IStatementBuilder> statementCallback, Action<IFixedStatementBuilder> fixedStatementCallback)
    {
        Syntax = FixedStatementBuilder.CreateSyntax(declarationTypeCallback, declarationVariableDeclarationCallback, statementCallback, fixedStatementCallback);
    }

    public void AsCheckedStatement(CheckedStatementKind kind, Action<IBlockBuilder> blockBlockCallback, Action<ICheckedStatementBuilder> checkedStatementCallback)
    {
        Syntax = CheckedStatementBuilder.CreateSyntax(kind, blockBlockCallback, checkedStatementCallback);
    }

    public void AsUnsafeStatement(Action<IBlockBuilder> blockBlockCallback, Action<IUnsafeStatementBuilder> unsafeStatementCallback)
    {
        Syntax = UnsafeStatementBuilder.CreateSyntax(blockBlockCallback, unsafeStatementCallback);
    }

    public void AsLockStatement(Action<IExpressionBuilder> expressionCallback, Action<IStatementBuilder> statementCallback, Action<ILockStatementBuilder> lockStatementCallback)
    {
        Syntax = LockStatementBuilder.CreateSyntax(expressionCallback, statementCallback, lockStatementCallback);
    }

    public void AsIfStatement(Action<IExpressionBuilder> conditionCallback, Action<IStatementBuilder> statementCallback, Action<IIfStatementBuilder> ifStatementCallback)
    {
        Syntax = IfStatementBuilder.CreateSyntax(conditionCallback, statementCallback, ifStatementCallback);
    }

    public void AsSwitchStatement(Action<IExpressionBuilder> expressionCallback, Action<ISwitchStatementBuilder> switchStatementCallback)
    {
        Syntax = SwitchStatementBuilder.CreateSyntax(expressionCallback, switchStatementCallback);
    }

    public void AsTryStatement(Action<IBlockBuilder> blockBlockCallback, Action<ITryStatementBuilder> tryStatementCallback)
    {
        Syntax = TryStatementBuilder.CreateSyntax(blockBlockCallback, tryStatementCallback);
    }
}