using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IParenthesizedLambdaExpressionBuilder : ILambdaExpressionBuilder<IParenthesizedLambdaExpressionBuilder>, IWithBlock<IParenthesizedLambdaExpressionBuilder>, IAddParameter<IParenthesizedLambdaExpressionBuilder>
{
    IParenthesizedLambdaExpressionBuilder WithExpressionBody(Action<IExpressionBuilder> expressionBodyCallback);
    IParenthesizedLambdaExpressionBuilder WithExpressionBody(ExpressionSyntax expressionBody);
    IParenthesizedLambdaExpressionBuilder WithReturnType(Action<ITypeBuilder> returnTypeCallback);
    IParenthesizedLambdaExpressionBuilder WithReturnType(TypeSyntax returnType);
}

public partial class ParenthesizedLambdaExpressionBuilder : IParenthesizedLambdaExpressionBuilder
{
    public ParenthesizedLambdaExpressionSyntax Syntax { get; set; }

    public ParenthesizedLambdaExpressionBuilder(ParenthesizedLambdaExpressionSyntax syntax)
    {
        Syntax = syntax;
    }

    public static ParenthesizedLambdaExpressionSyntax CreateSyntax(Action<IParenthesizedLambdaExpressionBuilder>? parenthesizedLambdaExpressionCallback = null)
    {
        var parameterListSyntax = SyntaxFactory.ParameterList();
        var arrowTokenToken = SyntaxFactory.Token(SyntaxKind.EqualsGreaterThanToken);
        var syntax = SyntaxFactory.ParenthesizedLambdaExpression(default(SyntaxList<AttributeListSyntax>), default(SyntaxTokenList), default(TypeSyntax), parameterListSyntax, arrowTokenToken, null, null);
        var builder = new ParenthesizedLambdaExpressionBuilder(syntax);
        parenthesizedLambdaExpressionCallback?.Invoke(builder);
        return builder.Syntax;
    }

    public IParenthesizedLambdaExpressionBuilder WithBlock(Action<IBlockBuilder>? blockCallback = null)
    {
        var blockSyntax = BlockBuilder.CreateSyntax(blockCallback);
        Syntax = Syntax.WithBlock(blockSyntax);
        return this;
    }

    public IParenthesizedLambdaExpressionBuilder WithBlock(BlockSyntax block)
    {
        Syntax = Syntax.WithBlock(block);
        return this;
    }

    public IParenthesizedLambdaExpressionBuilder WithExpressionBody(Action<IExpressionBuilder> expressionBodyCallback)
    {
        var expressionBodySyntax = ExpressionBuilder.CreateSyntax(expressionBodyCallback);
        Syntax = Syntax.WithExpressionBody(expressionBodySyntax);
        return this;
    }

    public IParenthesizedLambdaExpressionBuilder WithExpressionBody(ExpressionSyntax expressionBody)
    {
        Syntax = Syntax.WithExpressionBody(expressionBody);
        return this;
    }

    public IParenthesizedLambdaExpressionBuilder AddAttribute(Action<INameBuilder> nameCallback, Action<IAttributeBuilder>? attributeCallback = null)
    {
        var attribute = AttributeBuilder.CreateSyntax(nameCallback, attributeCallback);
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public IParenthesizedLambdaExpressionBuilder AddAttribute(AttributeSyntax attribute)
    {
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public IParenthesizedLambdaExpressionBuilder AddModifierToken(SyntaxKind modifier)
    {
        Syntax = Syntax.AddModifiers(SyntaxFactory.Token(modifier));
        return this;
    }

    public IParenthesizedLambdaExpressionBuilder WithReturnType(Action<ITypeBuilder> returnTypeCallback)
    {
        var returnTypeSyntax = TypeBuilder.CreateSyntax(returnTypeCallback);
        Syntax = Syntax.WithReturnType(returnTypeSyntax);
        return this;
    }

    public IParenthesizedLambdaExpressionBuilder WithReturnType(TypeSyntax returnType)
    {
        Syntax = Syntax.WithReturnType(returnType);
        return this;
    }

    public IParenthesizedLambdaExpressionBuilder AddParameter(string identifier, Action<IParameterBuilder>? parameterCallback = null)
    {
        var parameter = ParameterBuilder.CreateSyntax(identifier, parameterCallback);
        Syntax = Syntax.AddParameterListParameters(parameter);
        return this;
    }

    public IParenthesizedLambdaExpressionBuilder AddParameter(ParameterSyntax parameter)
    {
        Syntax = Syntax.AddParameterListParameters(parameter);
        return this;
    }
}