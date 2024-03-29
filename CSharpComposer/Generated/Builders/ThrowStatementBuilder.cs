﻿using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IThrowStatementBuilder : IStatementBuilder<IThrowStatementBuilder>, IWithExpression<IThrowStatementBuilder>
{
}

internal partial class ThrowStatementBuilder : IThrowStatementBuilder
{
    public ThrowStatementSyntax Syntax { get; set; }

    public ThrowStatementBuilder(ThrowStatementSyntax syntax)
    {
        Syntax = syntax;
    }

    public static ThrowStatementSyntax CreateSyntax(Action<IThrowStatementBuilder>? throwStatementCallback = null)
    {
        var throwKeywordToken = SyntaxFactory.Token(SyntaxKind.ThrowKeyword);
        var semicolonTokenToken = SyntaxFactory.Token(SyntaxKind.SemicolonToken);
        var syntax = SyntaxFactory.ThrowStatement(default(SyntaxList<AttributeListSyntax>), throwKeywordToken, default(ExpressionSyntax), semicolonTokenToken);
        var builder = new ThrowStatementBuilder(syntax);
        throwStatementCallback?.Invoke(builder);
        return builder.Syntax;
    }

    public IThrowStatementBuilder AddAttribute(Action<INameBuilder> nameCallback, Action<IAttributeBuilder>? attributeCallback = null)
    {
        var attribute = AttributeBuilder.CreateSyntax(nameCallback, attributeCallback);
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public IThrowStatementBuilder AddAttribute(AttributeSyntax attribute)
    {
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public IThrowStatementBuilder WithExpression(Action<IExpressionBuilder> expressionCallback)
    {
        var expressionSyntax = ExpressionBuilder.CreateSyntax(expressionCallback);
        Syntax = Syntax.WithExpression(expressionSyntax);
        return this;
    }

    public IThrowStatementBuilder WithExpression(ExpressionSyntax expression)
    {
        Syntax = Syntax.WithExpression(expression);
        return this;
    }
}