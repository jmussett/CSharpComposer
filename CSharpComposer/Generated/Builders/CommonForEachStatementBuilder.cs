using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface ICommonForEachStatementBuilder
{
    void AsForEachStatement(Action<ITypeBuilder> typeCallback, string identifier, Action<IExpressionBuilder> expressionCallback, Action<IStatementBuilder> statementCallback, Action<IForEachStatementBuilder> forEachStatementCallback);
    void AsForEachVariableStatement(Action<IExpressionBuilder> variableCallback, Action<IExpressionBuilder> expressionCallback, Action<IStatementBuilder> statementCallback, Action<IForEachVariableStatementBuilder> forEachVariableStatementCallback);
}

public partial interface ICommonForEachStatementBuilder<TBuilder> : IStatementBuilder<TBuilder>
{
    TBuilder WithAwaitKeyword();
}

public interface IWithCommonForEachStatementBuilder<TBuilder>
{
    TBuilder WithCommonForEachStatement(Action<ICommonForEachStatementBuilder> commonForEachStatementCallback);
    TBuilder WithCommonForEachStatement(CommonForEachStatementSyntax commonForEachStatementSyntax);
}

public partial class CommonForEachStatementBuilder : ICommonForEachStatementBuilder
{
    public CommonForEachStatementSyntax? Syntax { get; set; }

    public static CommonForEachStatementSyntax CreateSyntax(Action<ICommonForEachStatementBuilder> callback)
    {
        var builder = new CommonForEachStatementBuilder();
        callback(builder);
        if (builder.Syntax is null)
        {
            throw new InvalidOperationException("CommonForEachStatementSyntax has not been specified");
        }

        return builder.Syntax;
    }

    public void AsForEachStatement(Action<ITypeBuilder> typeCallback, string identifier, Action<IExpressionBuilder> expressionCallback, Action<IStatementBuilder> statementCallback, Action<IForEachStatementBuilder> forEachStatementCallback)
    {
        Syntax = ForEachStatementBuilder.CreateSyntax(typeCallback, identifier, expressionCallback, statementCallback, forEachStatementCallback);
    }

    public void AsForEachVariableStatement(Action<IExpressionBuilder> variableCallback, Action<IExpressionBuilder> expressionCallback, Action<IStatementBuilder> statementCallback, Action<IForEachVariableStatementBuilder> forEachVariableStatementCallback)
    {
        Syntax = ForEachVariableStatementBuilder.CreateSyntax(variableCallback, expressionCallback, statementCallback, forEachVariableStatementCallback);
    }
}