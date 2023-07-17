using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IInvocationExpressionBuilder : IAddArgument<IInvocationExpressionBuilder>
{
}

internal partial class InvocationExpressionBuilder : IInvocationExpressionBuilder
{
    public InvocationExpressionSyntax Syntax { get; set; }

    public InvocationExpressionBuilder(InvocationExpressionSyntax syntax)
    {
        Syntax = syntax;
    }

    public static InvocationExpressionSyntax CreateSyntax(Action<IExpressionBuilder> expressionCallback, Action<IInvocationExpressionBuilder>? invocationExpressionCallback = null)
    {
        var expressionSyntax = ExpressionBuilder.CreateSyntax(expressionCallback);
        var argumentListSyntax = SyntaxFactory.ArgumentList();
        var syntax = SyntaxFactory.InvocationExpression(expressionSyntax, argumentListSyntax);
        var builder = new InvocationExpressionBuilder(syntax);
        invocationExpressionCallback?.Invoke(builder);
        return builder.Syntax;
    }

    public IInvocationExpressionBuilder AddArgument(Action<IExpressionBuilder> expressionCallback, Action<IArgumentBuilder>? argumentCallback = null)
    {
        var argument = ArgumentBuilder.CreateSyntax(expressionCallback, argumentCallback);
        Syntax = Syntax.AddArgumentListArguments(argument);
        return this;
    }

    public IInvocationExpressionBuilder AddArgument(ArgumentSyntax argument)
    {
        Syntax = Syntax.AddArgumentListArguments(argument);
        return this;
    }
}