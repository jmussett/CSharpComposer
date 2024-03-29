﻿using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IUsingStatementBuilder : IStatementBuilder<IUsingStatementBuilder>, IWithExpression<IUsingStatementBuilder>
{
    IUsingStatementBuilder WithDeclaration(Action<ITypeBuilder> typeCallback, Action<IVariableDeclarationBuilder>? variableDeclarationCallback = null);
    IUsingStatementBuilder WithDeclaration(VariableDeclarationSyntax declaration);
    IUsingStatementBuilder WithAwaitKeyword();
}

internal partial class UsingStatementBuilder : IUsingStatementBuilder
{
    public UsingStatementSyntax Syntax { get; set; }

    public UsingStatementBuilder(UsingStatementSyntax syntax)
    {
        Syntax = syntax;
    }

    public static UsingStatementSyntax CreateSyntax(Action<IStatementBuilder> statementCallback, Action<IUsingStatementBuilder>? usingStatementCallback = null)
    {
        var usingKeywordToken = SyntaxFactory.Token(SyntaxKind.UsingKeyword);
        var openParenTokenToken = SyntaxFactory.Token(SyntaxKind.OpenParenToken);
        var closeParenTokenToken = SyntaxFactory.Token(SyntaxKind.CloseParenToken);
        var statementSyntax = StatementBuilder.CreateSyntax(statementCallback);
        var syntax = SyntaxFactory.UsingStatement(default(SyntaxList<AttributeListSyntax>), default(SyntaxToken), usingKeywordToken, openParenTokenToken, null, null, closeParenTokenToken, statementSyntax);
        var builder = new UsingStatementBuilder(syntax);
        usingStatementCallback?.Invoke(builder);
        return builder.Syntax;
    }

    public IUsingStatementBuilder WithDeclaration(Action<ITypeBuilder> typeCallback, Action<IVariableDeclarationBuilder>? variableDeclarationCallback = null)
    {
        var declarationSyntax = VariableDeclarationBuilder.CreateSyntax(typeCallback, variableDeclarationCallback);
        Syntax = Syntax.WithDeclaration(declarationSyntax);
        return this;
    }

    public IUsingStatementBuilder WithDeclaration(VariableDeclarationSyntax declaration)
    {
        Syntax = Syntax.WithDeclaration(declaration);
        return this;
    }

    public IUsingStatementBuilder WithExpression(Action<IExpressionBuilder> expressionCallback)
    {
        var expressionSyntax = ExpressionBuilder.CreateSyntax(expressionCallback);
        Syntax = Syntax.WithExpression(expressionSyntax);
        return this;
    }

    public IUsingStatementBuilder WithExpression(ExpressionSyntax expression)
    {
        Syntax = Syntax.WithExpression(expression);
        return this;
    }

    public IUsingStatementBuilder AddAttribute(Action<INameBuilder> nameCallback, Action<IAttributeBuilder>? attributeCallback = null)
    {
        var attribute = AttributeBuilder.CreateSyntax(nameCallback, attributeCallback);
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public IUsingStatementBuilder AddAttribute(AttributeSyntax attribute)
    {
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public IUsingStatementBuilder WithAwaitKeyword()
    {
        Syntax = Syntax.WithAwaitKeyword(SyntaxFactory.Token(SyntaxKind.AwaitKeyword));
        return this;
    }
}