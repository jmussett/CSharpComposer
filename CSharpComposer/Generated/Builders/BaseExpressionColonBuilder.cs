using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IBaseExpressionColonBuilder
{
    void AsExpressionColon(Action<IExpressionBuilder> expressionCallback);
    void AsNameColon(string nameIdentifier);
}

public interface IWithBaseExpressionColonBuilder<TBuilder>
{
    TBuilder WithBaseExpressionColon(Action<IBaseExpressionColonBuilder> baseExpressionColonCallback);
    TBuilder WithBaseExpressionColon(BaseExpressionColonSyntax baseExpressionColonSyntax);
}

public partial class BaseExpressionColonBuilder : IBaseExpressionColonBuilder
{
    public BaseExpressionColonSyntax? Syntax { get; set; }

    public static BaseExpressionColonSyntax CreateSyntax(Action<IBaseExpressionColonBuilder> callback)
    {
        var builder = new BaseExpressionColonBuilder();
        callback(builder);
        if (builder.Syntax is null)
        {
            throw new InvalidOperationException("BaseExpressionColonSyntax has not been specified");
        }

        return builder.Syntax;
    }

    public void AsExpressionColon(Action<IExpressionBuilder> expressionCallback)
    {
        Syntax = ExpressionColonBuilder.CreateSyntax(expressionCallback);
    }

    public void AsNameColon(string nameIdentifier)
    {
        Syntax = NameColonBuilder.CreateSyntax(nameIdentifier);
    }
}