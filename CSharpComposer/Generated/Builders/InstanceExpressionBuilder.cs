using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IInstanceExpressionBuilder
{
    void AsThisExpression();
    void AsBaseExpression();
}

public interface IWithInstanceExpressionBuilder<TBuilder>
{
    TBuilder WithInstanceExpression(Action<IInstanceExpressionBuilder> instanceExpressionCallback);
    TBuilder WithInstanceExpression(InstanceExpressionSyntax instanceExpressionSyntax);
}

public partial class InstanceExpressionBuilder : IInstanceExpressionBuilder
{
    public InstanceExpressionSyntax? Syntax { get; set; }

    public static InstanceExpressionSyntax CreateSyntax(Action<IInstanceExpressionBuilder> callback)
    {
        var builder = new InstanceExpressionBuilder();
        callback(builder);
        if (builder.Syntax is null)
        {
            throw new InvalidOperationException("InstanceExpressionSyntax has not been specified");
        }

        return builder.Syntax;
    }

    public void AsThisExpression()
    {
        Syntax = ThisExpressionBuilder.CreateSyntax();
    }

    public void AsBaseExpression()
    {
        Syntax = BaseExpressionBuilder.CreateSyntax();
    }
}