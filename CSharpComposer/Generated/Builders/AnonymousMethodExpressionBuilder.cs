using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IAnonymousMethodExpressionBuilder : IAnonymousFunctionExpressionBuilder<IAnonymousMethodExpressionBuilder>
{
    IAnonymousMethodExpressionBuilder AddParameter(string identifier, Action<IParameterBuilder> parameterCallback);
    IAnonymousMethodExpressionBuilder AddParameter(ParameterSyntax parameter);
}

public interface IWithAnonymousMethodExpressionBuilder<TBuilder>
{
    TBuilder WithAnonymousMethodExpression(Action<IBlockBuilder> blockBlockCallback, Action<IAnonymousMethodExpressionBuilder> anonymousMethodExpressionCallback);
    TBuilder WithAnonymousMethodExpression(AnonymousMethodExpressionSyntax anonymousMethodExpressionSyntax);
}

public partial class AnonymousMethodExpressionBuilder : IAnonymousMethodExpressionBuilder
{
    public AnonymousMethodExpressionSyntax Syntax { get; set; }

    public AnonymousMethodExpressionBuilder(AnonymousMethodExpressionSyntax syntax)
    {
        Syntax = syntax;
    }

    public static AnonymousMethodExpressionSyntax CreateSyntax(Action<IBlockBuilder> blockBlockCallback, Action<IAnonymousMethodExpressionBuilder> anonymousMethodExpressionCallback)
    {
        var delegateKeywordToken = SyntaxFactory.Token(SyntaxKind.DelegateKeyword);
        var blockSyntax = BlockBuilder.CreateSyntax(blockBlockCallback);
        var syntax = SyntaxFactory.AnonymousMethodExpression(default(SyntaxTokenList), delegateKeywordToken, default(ParameterListSyntax), blockSyntax, default(ExpressionSyntax));
        var builder = new AnonymousMethodExpressionBuilder(syntax);
        anonymousMethodExpressionCallback(builder);
        return builder.Syntax;
    }

    public IAnonymousMethodExpressionBuilder AddParameter(string identifier, Action<IParameterBuilder> parameterCallback)
    {
        var parameter = ParameterBuilder.CreateSyntax(identifier, parameterCallback);
        Syntax = Syntax.AddParameterListParameters(parameter);
        return this;
    }

    public IAnonymousMethodExpressionBuilder AddParameter(ParameterSyntax parameter)
    {
        Syntax = Syntax.AddParameterListParameters(parameter);
        return this;
    }

    public IAnonymousMethodExpressionBuilder WithExpressionBody(Action<IExpressionBuilder> expressionBodyCallback)
    {
        var expressionBodySyntax = ExpressionBuilder.CreateSyntax(expressionBodyCallback);
        Syntax = Syntax.WithExpressionBody(expressionBodySyntax);
        return this;
    }

    public IAnonymousMethodExpressionBuilder WithExpressionBody(ExpressionSyntax expressionBody)
    {
        Syntax = Syntax.WithExpressionBody(expressionBody);
        return this;
    }
}