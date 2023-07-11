using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IStatementBuilder
{
    void AsBlock(Action<IBlockBuilder>? blockCallback = null);
    void AsLocalFunctionStatement(Action<ITypeBuilder> returnTypeCallback, string identifier, Action<ILocalFunctionStatementBuilder>? localFunctionStatementCallback = null);
    void AsLocalDeclarationStatement(Action<ITypeBuilder> declarationTypeCallback, Action<IVariableDeclarationBuilder> declarationVariableDeclarationCallback, Action<ILocalDeclarationStatementBuilder> localDeclarationStatementCallback);
    void AsExpressionStatement(Action<IExpressionBuilder> expressionCallback, Action<IExpressionStatementBuilder>? expressionStatementCallback = null);
    void AsEmptyStatement(Action<IEmptyStatementBuilder>? emptyStatementCallback = null);
    void AsLabeledStatement(string identifier, Action<IStatementBuilder> statementCallback, Action<ILabeledStatementBuilder>? labeledStatementCallback = null);
    void AsGotoStatement(GotoStatementKind kind, Action<IGotoStatementBuilder>? gotoStatementCallback = null);
    void AsBreakStatement(Action<IBreakStatementBuilder>? breakStatementCallback = null);
    void AsContinueStatement(Action<IContinueStatementBuilder>? continueStatementCallback = null);
    void AsReturnStatement(Action<IReturnStatementBuilder>? returnStatementCallback = null);
    void AsThrowStatement(Action<IThrowStatementBuilder>? throwStatementCallback = null);
    void AsYieldStatement(YieldStatementKind kind, Action<IYieldStatementBuilder>? yieldStatementCallback = null);
    void AsWhileStatement(Action<IExpressionBuilder> conditionCallback, Action<IStatementBuilder> statementCallback, Action<IWhileStatementBuilder>? whileStatementCallback = null);
    void AsDoStatement(Action<IStatementBuilder> statementCallback, Action<IExpressionBuilder> conditionCallback, Action<IDoStatementBuilder>? doStatementCallback = null);
    void AsForStatement(Action<IStatementBuilder> statementCallback, Action<IForStatementBuilder>? forStatementCallback = null);
    void AsForEachStatement(Action<ITypeBuilder> typeCallback, string identifier, Action<IExpressionBuilder> expressionCallback, Action<IStatementBuilder> statementCallback, Action<IForEachStatementBuilder>? forEachStatementCallback = null);
    void AsForEachVariableStatement(Action<IExpressionBuilder> variableCallback, Action<IExpressionBuilder> expressionCallback, Action<IStatementBuilder> statementCallback, Action<IForEachVariableStatementBuilder>? forEachVariableStatementCallback = null);
    void AsUsingStatement(Action<IStatementBuilder> statementCallback, Action<IUsingStatementBuilder>? usingStatementCallback = null);
    void AsFixedStatement(Action<ITypeBuilder> declarationTypeCallback, Action<IVariableDeclarationBuilder> declarationVariableDeclarationCallback, Action<IStatementBuilder> statementCallback, Action<IFixedStatementBuilder> fixedStatementCallback);
    void AsCheckedStatement(CheckedStatementKind kind, Action<IBlockBuilder> blockBlockCallback, Action<ICheckedStatementBuilder> checkedStatementCallback);
    void AsUnsafeStatement(Action<IBlockBuilder> blockBlockCallback, Action<IUnsafeStatementBuilder> unsafeStatementCallback);
    void AsLockStatement(Action<IExpressionBuilder> expressionCallback, Action<IStatementBuilder> statementCallback, Action<ILockStatementBuilder>? lockStatementCallback = null);
    void AsIfStatement(Action<IExpressionBuilder> conditionCallback, Action<IStatementBuilder> statementCallback, Action<IIfStatementBuilder>? ifStatementCallback = null);
    void AsSwitchStatement(Action<IExpressionBuilder> expressionCallback, Action<ISwitchStatementBuilder>? switchStatementCallback = null);
    void AsTryStatement(Action<IBlockBuilder> blockBlockCallback, Action<ITryStatementBuilder> tryStatementCallback);
}

public partial interface IStatementBuilder<TBuilder> : IAddAttribute<TBuilder>
{
}

public interface IAddStatement<TBuilder>
{
    TBuilder AddStatement(StatementSyntax statementSyntax);
    TBuilder AddStatement(Action<IStatementBuilder> statementCallback);
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

    public void AsBlock(Action<IBlockBuilder>? blockCallback = null)
    {
        Syntax = BlockBuilder.CreateSyntax(blockCallback);
    }

    public void AsLocalFunctionStatement(Action<ITypeBuilder> returnTypeCallback, string identifier, Action<ILocalFunctionStatementBuilder>? localFunctionStatementCallback = null)
    {
        Syntax = LocalFunctionStatementBuilder.CreateSyntax(returnTypeCallback, identifier, localFunctionStatementCallback);
    }

    public void AsLocalDeclarationStatement(Action<ITypeBuilder> declarationTypeCallback, Action<IVariableDeclarationBuilder> declarationVariableDeclarationCallback, Action<ILocalDeclarationStatementBuilder> localDeclarationStatementCallback)
    {
        Syntax = LocalDeclarationStatementBuilder.CreateSyntax(declarationTypeCallback, declarationVariableDeclarationCallback, localDeclarationStatementCallback);
    }

    public void AsExpressionStatement(Action<IExpressionBuilder> expressionCallback, Action<IExpressionStatementBuilder>? expressionStatementCallback = null)
    {
        Syntax = ExpressionStatementBuilder.CreateSyntax(expressionCallback, expressionStatementCallback);
    }

    public void AsEmptyStatement(Action<IEmptyStatementBuilder>? emptyStatementCallback = null)
    {
        Syntax = EmptyStatementBuilder.CreateSyntax(emptyStatementCallback);
    }

    public void AsLabeledStatement(string identifier, Action<IStatementBuilder> statementCallback, Action<ILabeledStatementBuilder>? labeledStatementCallback = null)
    {
        Syntax = LabeledStatementBuilder.CreateSyntax(identifier, statementCallback, labeledStatementCallback);
    }

    public void AsGotoStatement(GotoStatementKind kind, Action<IGotoStatementBuilder>? gotoStatementCallback = null)
    {
        Syntax = GotoStatementBuilder.CreateSyntax(kind, gotoStatementCallback);
    }

    public void AsBreakStatement(Action<IBreakStatementBuilder>? breakStatementCallback = null)
    {
        Syntax = BreakStatementBuilder.CreateSyntax(breakStatementCallback);
    }

    public void AsContinueStatement(Action<IContinueStatementBuilder>? continueStatementCallback = null)
    {
        Syntax = ContinueStatementBuilder.CreateSyntax(continueStatementCallback);
    }

    public void AsReturnStatement(Action<IReturnStatementBuilder>? returnStatementCallback = null)
    {
        Syntax = ReturnStatementBuilder.CreateSyntax(returnStatementCallback);
    }

    public void AsThrowStatement(Action<IThrowStatementBuilder>? throwStatementCallback = null)
    {
        Syntax = ThrowStatementBuilder.CreateSyntax(throwStatementCallback);
    }

    public void AsYieldStatement(YieldStatementKind kind, Action<IYieldStatementBuilder>? yieldStatementCallback = null)
    {
        Syntax = YieldStatementBuilder.CreateSyntax(kind, yieldStatementCallback);
    }

    public void AsWhileStatement(Action<IExpressionBuilder> conditionCallback, Action<IStatementBuilder> statementCallback, Action<IWhileStatementBuilder>? whileStatementCallback = null)
    {
        Syntax = WhileStatementBuilder.CreateSyntax(conditionCallback, statementCallback, whileStatementCallback);
    }

    public void AsDoStatement(Action<IStatementBuilder> statementCallback, Action<IExpressionBuilder> conditionCallback, Action<IDoStatementBuilder>? doStatementCallback = null)
    {
        Syntax = DoStatementBuilder.CreateSyntax(statementCallback, conditionCallback, doStatementCallback);
    }

    public void AsForStatement(Action<IStatementBuilder> statementCallback, Action<IForStatementBuilder>? forStatementCallback = null)
    {
        Syntax = ForStatementBuilder.CreateSyntax(statementCallback, forStatementCallback);
    }

    public void AsForEachStatement(Action<ITypeBuilder> typeCallback, string identifier, Action<IExpressionBuilder> expressionCallback, Action<IStatementBuilder> statementCallback, Action<IForEachStatementBuilder>? forEachStatementCallback = null)
    {
        Syntax = ForEachStatementBuilder.CreateSyntax(typeCallback, identifier, expressionCallback, statementCallback, forEachStatementCallback);
    }

    public void AsForEachVariableStatement(Action<IExpressionBuilder> variableCallback, Action<IExpressionBuilder> expressionCallback, Action<IStatementBuilder> statementCallback, Action<IForEachVariableStatementBuilder>? forEachVariableStatementCallback = null)
    {
        Syntax = ForEachVariableStatementBuilder.CreateSyntax(variableCallback, expressionCallback, statementCallback, forEachVariableStatementCallback);
    }

    public void AsUsingStatement(Action<IStatementBuilder> statementCallback, Action<IUsingStatementBuilder>? usingStatementCallback = null)
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

    public void AsLockStatement(Action<IExpressionBuilder> expressionCallback, Action<IStatementBuilder> statementCallback, Action<ILockStatementBuilder>? lockStatementCallback = null)
    {
        Syntax = LockStatementBuilder.CreateSyntax(expressionCallback, statementCallback, lockStatementCallback);
    }

    public void AsIfStatement(Action<IExpressionBuilder> conditionCallback, Action<IStatementBuilder> statementCallback, Action<IIfStatementBuilder>? ifStatementCallback = null)
    {
        Syntax = IfStatementBuilder.CreateSyntax(conditionCallback, statementCallback, ifStatementCallback);
    }

    public void AsSwitchStatement(Action<IExpressionBuilder> expressionCallback, Action<ISwitchStatementBuilder>? switchStatementCallback = null)
    {
        Syntax = SwitchStatementBuilder.CreateSyntax(expressionCallback, switchStatementCallback);
    }

    public void AsTryStatement(Action<IBlockBuilder> blockBlockCallback, Action<ITryStatementBuilder> tryStatementCallback)
    {
        Syntax = TryStatementBuilder.CreateSyntax(blockBlockCallback, tryStatementCallback);
    }
}