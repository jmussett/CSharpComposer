﻿using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IDestructorDeclarationBuilder : IBaseMethodDeclarationBuilder<IDestructorDeclarationBuilder>
{
    IDestructorDeclarationBuilder WithExpressionBody(Action<IExpressionBuilder> expressionCallback);
    IDestructorDeclarationBuilder WithExpressionBody(ArrowExpressionClauseSyntax expressionBody);
    IDestructorDeclarationBuilder WithBody(Action<IBlockBuilder>? blockCallback = null);
    IDestructorDeclarationBuilder WithBody(BlockSyntax body);
}

internal partial class DestructorDeclarationBuilder : IDestructorDeclarationBuilder
{
    public DestructorDeclarationSyntax Syntax { get; set; }

    public DestructorDeclarationBuilder(DestructorDeclarationSyntax syntax)
    {
        Syntax = syntax;
    }

    public static DestructorDeclarationSyntax CreateSyntax(string identifier, Action<IDestructorDeclarationBuilder>? destructorDeclarationCallback = null)
    {
        var tildeTokenToken = SyntaxFactory.Token(SyntaxKind.TildeToken);
        var identifierToken = SyntaxFactory.Identifier(identifier);
        var parameterListSyntax = SyntaxFactory.ParameterList();
        var syntax = SyntaxFactory.DestructorDeclaration(default(SyntaxList<AttributeListSyntax>), default(SyntaxTokenList), tildeTokenToken, identifierToken, parameterListSyntax, null, null, default(SyntaxToken));
        var builder = new DestructorDeclarationBuilder(syntax);
        destructorDeclarationCallback?.Invoke(builder);
        return builder.Syntax;
    }

    public IDestructorDeclarationBuilder WithExpressionBody(Action<IExpressionBuilder> expressionCallback)
    {
        var expressionBodySyntax = ArrowExpressionClauseBuilder.CreateSyntax(expressionCallback);
        Syntax = Syntax.WithExpressionBody(expressionBodySyntax);
        return this;
    }

    public IDestructorDeclarationBuilder WithExpressionBody(ArrowExpressionClauseSyntax expressionBody)
    {
        Syntax = Syntax.WithExpressionBody(expressionBody);
        return this;
    }

    public IDestructorDeclarationBuilder WithBody(Action<IBlockBuilder>? blockCallback = null)
    {
        var bodySyntax = BlockBuilder.CreateSyntax(blockCallback);
        Syntax = Syntax.WithBody(bodySyntax);
        return this;
    }

    public IDestructorDeclarationBuilder WithBody(BlockSyntax body)
    {
        Syntax = Syntax.WithBody(body);
        return this;
    }

    public IDestructorDeclarationBuilder AddAttribute(Action<INameBuilder> nameCallback, Action<IAttributeBuilder>? attributeCallback = null)
    {
        var attribute = AttributeBuilder.CreateSyntax(nameCallback, attributeCallback);
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public IDestructorDeclarationBuilder AddAttribute(AttributeSyntax attribute)
    {
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public IDestructorDeclarationBuilder AddModifierToken(SyntaxKind modifier)
    {
        Syntax = Syntax.AddModifiers(SyntaxFactory.Token(modifier));
        return this;
    }

    public IDestructorDeclarationBuilder AddParameter(string identifier, Action<IParameterBuilder>? parameterCallback = null)
    {
        var parameter = ParameterBuilder.CreateSyntax(identifier, parameterCallback);
        Syntax = Syntax.AddParameterListParameters(parameter);
        return this;
    }

    public IDestructorDeclarationBuilder AddParameter(ParameterSyntax parameter)
    {
        Syntax = Syntax.AddParameterListParameters(parameter);
        return this;
    }
}