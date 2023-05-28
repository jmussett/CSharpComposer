using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface ISimpleLambdaExpressionBuilder : ILambdaExpressionBuilder<ISimpleLambdaExpressionBuilder>
{
}

public interface IWithSimpleLambdaExpressionBuilder<TBuilder>
{
    TBuilder WithSimpleLambdaExpression(string parameterIdentifier, Action<IParameterBuilder> parameterParameterCallback, Action<ISimpleLambdaExpressionBuilder> simpleLambdaExpressionCallback);
    TBuilder WithSimpleLambdaExpression(SimpleLambdaExpressionSyntax simpleLambdaExpressionSyntax);
}

public partial class SimpleLambdaExpressionBuilder : ISimpleLambdaExpressionBuilder
{
    public SimpleLambdaExpressionSyntax Syntax { get; set; }

    public SimpleLambdaExpressionBuilder(SimpleLambdaExpressionSyntax syntax)
    {
        Syntax = syntax;
    }

    public static SimpleLambdaExpressionSyntax CreateSyntax(string parameterIdentifier, Action<IParameterBuilder> parameterParameterCallback, Action<ISimpleLambdaExpressionBuilder> simpleLambdaExpressionCallback)
    {
        var parameterSyntax = ParameterBuilder.CreateSyntax(parameterIdentifier, parameterParameterCallback);
        var arrowTokenToken = SyntaxFactory.Token(SyntaxKind.EqualsGreaterThanToken);
        var syntax = SyntaxFactory.SimpleLambdaExpression(default(SyntaxList<AttributeListSyntax>), default(SyntaxTokenList), parameterSyntax, arrowTokenToken, null, null);
        var builder = new SimpleLambdaExpressionBuilder(syntax);
        simpleLambdaExpressionCallback(builder);
        return builder.Syntax;
    }

    public ISimpleLambdaExpressionBuilder WithBlock(Action<IBlockBuilder> blockCallback)
    {
        var blockSyntax = BlockBuilder.CreateSyntax(blockCallback);
        Syntax = Syntax.WithBlock(blockSyntax);
        return this;
    }

    public ISimpleLambdaExpressionBuilder WithBlock(BlockSyntax block)
    {
        Syntax = Syntax.WithBlock(block);
        return this;
    }

    public ISimpleLambdaExpressionBuilder WithExpressionBody(Action<IExpressionBuilder> expressionBodyCallback)
    {
        var expressionBodySyntax = ExpressionBuilder.CreateSyntax(expressionBodyCallback);
        Syntax = Syntax.WithExpressionBody(expressionBodySyntax);
        return this;
    }

    public ISimpleLambdaExpressionBuilder WithExpressionBody(ExpressionSyntax expressionBody)
    {
        Syntax = Syntax.WithExpressionBody(expressionBody);
        return this;
    }

    public ISimpleLambdaExpressionBuilder AddAttribute(Action<INameBuilder> nameCallback, Action<IAttributeBuilder> attributeCallback)
    {
        var attribute = AttributeBuilder.CreateSyntax(nameCallback, attributeCallback);
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public ISimpleLambdaExpressionBuilder AddAttribute(AttributeSyntax attribute)
    {
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }
}