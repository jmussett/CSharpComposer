using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IReturnStatementBuilder : IWithExpressionBuilder<IReturnStatementBuilder>, IStatementBuilder<IReturnStatementBuilder>
{
    IReturnStatementBuilder WithExpression(Action<IExpressionBuilder> expressionCallback);
    IReturnStatementBuilder WithExpression(ExpressionSyntax expression);
}

public interface IWithReturnStatementBuilder<TBuilder>
{
    TBuilder WithReturnStatement(Action<IReturnStatementBuilder> returnStatementCallback);
    TBuilder WithReturnStatement(ReturnStatementSyntax returnStatementSyntax);
}

public partial class ReturnStatementBuilder : IReturnStatementBuilder
{
    public ReturnStatementSyntax Syntax { get; set; }

    public ReturnStatementBuilder(ReturnStatementSyntax syntax)
    {
        Syntax = syntax;
    }

    public static ReturnStatementSyntax CreateSyntax(Action<IReturnStatementBuilder> returnStatementCallback)
    {
        var returnKeywordToken = SyntaxFactory.Token(SyntaxKind.ReturnKeyword);
        var semicolonTokenToken = SyntaxFactory.Token(SyntaxKind.SemicolonToken);
        var syntax = SyntaxFactory.ReturnStatement(default(SyntaxList<AttributeListSyntax>), returnKeywordToken, default(ExpressionSyntax), semicolonTokenToken);
        var builder = new ReturnStatementBuilder(syntax);
        returnStatementCallback(builder);
        return builder.Syntax;
    }

    public IReturnStatementBuilder AddAttribute(Action<INameBuilder> nameCallback, Action<IAttributeBuilder> attributeCallback)
    {
        var attribute = AttributeBuilder.CreateSyntax(nameCallback, attributeCallback);
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public IReturnStatementBuilder AddAttribute(AttributeSyntax attribute)
    {
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public IReturnStatementBuilder WithExpression(Action<IExpressionBuilder> expressionCallback)
    {
        var expressionSyntax = ExpressionBuilder.CreateSyntax(expressionCallback);
        Syntax = Syntax.WithExpression(expressionSyntax);
        return this;
    }

    public IReturnStatementBuilder WithExpression(ExpressionSyntax expression)
    {
        Syntax = Syntax.WithExpression(expression);
        return this;
    }
}