﻿using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IConstructorDeclarationBuilder : IBaseMethodDeclarationBuilder<IConstructorDeclarationBuilder>
{
    IConstructorDeclarationBuilder WithExpressionBody(Action<IExpressionBuilder> expressionCallback);
    IConstructorDeclarationBuilder WithExpressionBody(ArrowExpressionClauseSyntax expressionBody);
    IConstructorDeclarationBuilder WithBody(Action<IBlockBuilder>? blockCallback = null);
    IConstructorDeclarationBuilder WithBody(BlockSyntax body);
    IConstructorDeclarationBuilder WithInitializer(ConstructorInitializerKind kind, Action<IConstructorInitializerBuilder>? constructorInitializerCallback = null);
    IConstructorDeclarationBuilder WithInitializer(ConstructorInitializerSyntax initializer);
}

internal partial class ConstructorDeclarationBuilder : IConstructorDeclarationBuilder
{
    public ConstructorDeclarationSyntax Syntax { get; set; }

    public ConstructorDeclarationBuilder(ConstructorDeclarationSyntax syntax)
    {
        Syntax = syntax;
    }

    public static ConstructorDeclarationSyntax CreateSyntax(string identifier, Action<IConstructorDeclarationBuilder>? constructorDeclarationCallback = null)
    {
        var identifierToken = SyntaxFactory.Identifier(identifier);
        var parameterListSyntax = SyntaxFactory.ParameterList();
        var syntax = SyntaxFactory.ConstructorDeclaration(default(SyntaxList<AttributeListSyntax>), default(SyntaxTokenList), identifierToken, parameterListSyntax, default(ConstructorInitializerSyntax), null, null, default(SyntaxToken));
        var builder = new ConstructorDeclarationBuilder(syntax);
        constructorDeclarationCallback?.Invoke(builder);
        return builder.Syntax;
    }

    public IConstructorDeclarationBuilder WithExpressionBody(Action<IExpressionBuilder> expressionCallback)
    {
        var expressionBodySyntax = ArrowExpressionClauseBuilder.CreateSyntax(expressionCallback);
        Syntax = Syntax.WithExpressionBody(expressionBodySyntax);
        return this;
    }

    public IConstructorDeclarationBuilder WithExpressionBody(ArrowExpressionClauseSyntax expressionBody)
    {
        Syntax = Syntax.WithExpressionBody(expressionBody);
        return this;
    }

    public IConstructorDeclarationBuilder WithBody(Action<IBlockBuilder>? blockCallback = null)
    {
        var bodySyntax = BlockBuilder.CreateSyntax(blockCallback);
        Syntax = Syntax.WithBody(bodySyntax);
        return this;
    }

    public IConstructorDeclarationBuilder WithBody(BlockSyntax body)
    {
        Syntax = Syntax.WithBody(body);
        return this;
    }

    public IConstructorDeclarationBuilder AddAttribute(Action<INameBuilder> nameCallback, Action<IAttributeBuilder>? attributeCallback = null)
    {
        var attribute = AttributeBuilder.CreateSyntax(nameCallback, attributeCallback);
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public IConstructorDeclarationBuilder AddAttribute(AttributeSyntax attribute)
    {
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public IConstructorDeclarationBuilder AddModifierToken(SyntaxKind modifier)
    {
        Syntax = Syntax.AddModifiers(SyntaxFactory.Token(modifier));
        return this;
    }

    public IConstructorDeclarationBuilder AddParameter(string identifier, Action<IParameterBuilder>? parameterCallback = null)
    {
        var parameter = ParameterBuilder.CreateSyntax(identifier, parameterCallback);
        Syntax = Syntax.AddParameterListParameters(parameter);
        return this;
    }

    public IConstructorDeclarationBuilder AddParameter(ParameterSyntax parameter)
    {
        Syntax = Syntax.AddParameterListParameters(parameter);
        return this;
    }

    public IConstructorDeclarationBuilder WithInitializer(ConstructorInitializerKind kind, Action<IConstructorInitializerBuilder>? constructorInitializerCallback = null)
    {
        var initializerSyntax = ConstructorInitializerBuilder.CreateSyntax(kind, constructorInitializerCallback);
        Syntax = Syntax.WithInitializer(initializerSyntax);
        return this;
    }

    public IConstructorDeclarationBuilder WithInitializer(ConstructorInitializerSyntax initializer)
    {
        Syntax = Syntax.WithInitializer(initializer);
        return this;
    }
}