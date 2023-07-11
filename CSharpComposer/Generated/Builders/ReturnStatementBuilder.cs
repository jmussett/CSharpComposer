using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IReturnStatementBuilder : IStatementBuilder<IReturnStatementBuilder>, IWithExpression<IReturnStatementBuilder>
{
}

public partial class ReturnStatementBuilder : IReturnStatementBuilder
{
    public ReturnStatementSyntax Syntax { get; set; }

    public ReturnStatementBuilder(ReturnStatementSyntax syntax)
    {
        Syntax = syntax;
    }

    public static ReturnStatementSyntax CreateSyntax(Action<IReturnStatementBuilder>? returnStatementCallback = null)
    {
        var returnKeywordToken = SyntaxFactory.Token(SyntaxKind.ReturnKeyword);
        var semicolonTokenToken = SyntaxFactory.Token(SyntaxKind.SemicolonToken);
        var syntax = SyntaxFactory.ReturnStatement(default(SyntaxList<AttributeListSyntax>), returnKeywordToken, default(ExpressionSyntax), semicolonTokenToken);
        var builder = new ReturnStatementBuilder(syntax);
        returnStatementCallback?.Invoke(builder);
        return builder.Syntax;
    }

    public IReturnStatementBuilder AddAttribute(Action<INameBuilder> nameCallback, Action<IAttributeBuilder>? attributeCallback = null)
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