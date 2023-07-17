using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface ITupleExpressionBuilder : IAddArgument<ITupleExpressionBuilder>
{
}

internal partial class TupleExpressionBuilder : ITupleExpressionBuilder
{
    public TupleExpressionSyntax Syntax { get; set; }

    public TupleExpressionBuilder(TupleExpressionSyntax syntax)
    {
        Syntax = syntax;
    }

    public static TupleExpressionSyntax CreateSyntax(Action<ITupleExpressionBuilder>? tupleExpressionCallback = null)
    {
        var openParenTokenToken = SyntaxFactory.Token(SyntaxKind.OpenParenToken);
        var closeParenTokenToken = SyntaxFactory.Token(SyntaxKind.CloseParenToken);
        var syntax = SyntaxFactory.TupleExpression(openParenTokenToken, default(SeparatedSyntaxList<ArgumentSyntax>), closeParenTokenToken);
        var builder = new TupleExpressionBuilder(syntax);
        tupleExpressionCallback?.Invoke(builder);
        return builder.Syntax;
    }

    public ITupleExpressionBuilder AddArgument(Action<IExpressionBuilder> expressionCallback, Action<IArgumentBuilder>? argumentCallback = null)
    {
        var argument = ArgumentBuilder.CreateSyntax(expressionCallback, argumentCallback);
        Syntax = Syntax.AddArguments(argument);
        return this;
    }

    public ITupleExpressionBuilder AddArgument(ArgumentSyntax argument)
    {
        Syntax = Syntax.AddArguments(argument);
        return this;
    }
}