﻿using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface ILocalFunctionStatementBuilder : IStatementBuilder<ILocalFunctionStatementBuilder>, IAddTypeParameter<ILocalFunctionStatementBuilder>, IAddParameter<ILocalFunctionStatementBuilder>
{
    ILocalFunctionStatementBuilder WithExpressionBody(Action<IExpressionBuilder> expressionCallback);
    ILocalFunctionStatementBuilder WithExpressionBody(ArrowExpressionClauseSyntax expressionBody);
    ILocalFunctionStatementBuilder WithBody(Action<IBlockBuilder>? blockCallback = null);
    ILocalFunctionStatementBuilder WithBody(BlockSyntax body);
    ILocalFunctionStatementBuilder AddModifierToken(SyntaxKind modifier);
    ILocalFunctionStatementBuilder AddTypeParameterConstraintClause(string nameIdentifier, Action<ITypeParameterConstraintClauseBuilder> typeParameterConstraintClauseCallback);
    ILocalFunctionStatementBuilder AddTypeParameterConstraintClause(TypeParameterConstraintClauseSyntax constraintClause);
}

internal partial class LocalFunctionStatementBuilder : ILocalFunctionStatementBuilder
{
    public LocalFunctionStatementSyntax Syntax { get; set; }

    public LocalFunctionStatementBuilder(LocalFunctionStatementSyntax syntax)
    {
        Syntax = syntax;
    }

    public static LocalFunctionStatementSyntax CreateSyntax(Action<ITypeBuilder> returnTypeCallback, string identifier, Action<ILocalFunctionStatementBuilder>? localFunctionStatementCallback = null)
    {
        var returnTypeSyntax = TypeBuilder.CreateSyntax(returnTypeCallback);
        var identifierToken = SyntaxFactory.Identifier(identifier);
        var parameterListSyntax = SyntaxFactory.ParameterList();
        var syntax = SyntaxFactory.LocalFunctionStatement(default(SyntaxList<AttributeListSyntax>), default(SyntaxTokenList), returnTypeSyntax, identifierToken, default(TypeParameterListSyntax), parameterListSyntax, default(SyntaxList<TypeParameterConstraintClauseSyntax>), null, null, default(SyntaxToken));
        var builder = new LocalFunctionStatementBuilder(syntax);
        localFunctionStatementCallback?.Invoke(builder);
        return builder.Syntax;
    }

    public ILocalFunctionStatementBuilder WithExpressionBody(Action<IExpressionBuilder> expressionCallback)
    {
        var expressionBodySyntax = ArrowExpressionClauseBuilder.CreateSyntax(expressionCallback);
        Syntax = Syntax.WithExpressionBody(expressionBodySyntax);
        return this;
    }

    public ILocalFunctionStatementBuilder WithExpressionBody(ArrowExpressionClauseSyntax expressionBody)
    {
        Syntax = Syntax.WithExpressionBody(expressionBody);
        return this;
    }

    public ILocalFunctionStatementBuilder WithBody(Action<IBlockBuilder>? blockCallback = null)
    {
        var bodySyntax = BlockBuilder.CreateSyntax(blockCallback);
        Syntax = Syntax.WithBody(bodySyntax);
        return this;
    }

    public ILocalFunctionStatementBuilder WithBody(BlockSyntax body)
    {
        Syntax = Syntax.WithBody(body);
        return this;
    }

    public ILocalFunctionStatementBuilder AddAttribute(Action<INameBuilder> nameCallback, Action<IAttributeBuilder>? attributeCallback = null)
    {
        var attribute = AttributeBuilder.CreateSyntax(nameCallback, attributeCallback);
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public ILocalFunctionStatementBuilder AddAttribute(AttributeSyntax attribute)
    {
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public ILocalFunctionStatementBuilder AddModifierToken(SyntaxKind modifier)
    {
        Syntax = Syntax.AddModifiers(SyntaxFactory.Token(modifier));
        return this;
    }

    public ILocalFunctionStatementBuilder AddTypeParameter(string identifier, Action<ITypeParameterBuilder>? typeParameterCallback = null)
    {
        var parameter = TypeParameterBuilder.CreateSyntax(identifier, typeParameterCallback);
        Syntax = Syntax.AddTypeParameterListParameters(parameter);
        return this;
    }

    public ILocalFunctionStatementBuilder AddTypeParameter(TypeParameterSyntax parameter)
    {
        Syntax = Syntax.AddTypeParameterListParameters(parameter);
        return this;
    }

    public ILocalFunctionStatementBuilder AddParameter(string identifier, Action<IParameterBuilder>? parameterCallback = null)
    {
        var parameter = ParameterBuilder.CreateSyntax(identifier, parameterCallback);
        Syntax = Syntax.AddParameterListParameters(parameter);
        return this;
    }

    public ILocalFunctionStatementBuilder AddParameter(ParameterSyntax parameter)
    {
        Syntax = Syntax.AddParameterListParameters(parameter);
        return this;
    }

    public ILocalFunctionStatementBuilder AddTypeParameterConstraintClause(string nameIdentifier, Action<ITypeParameterConstraintClauseBuilder> typeParameterConstraintClauseCallback)
    {
        var constraintClause = TypeParameterConstraintClauseBuilder.CreateSyntax(nameIdentifier, typeParameterConstraintClauseCallback);
        Syntax = Syntax.AddConstraintClauses(constraintClause);
        return this;
    }

    public ILocalFunctionStatementBuilder AddTypeParameterConstraintClause(TypeParameterConstraintClauseSyntax constraintClause)
    {
        Syntax = Syntax.AddConstraintClauses(constraintClause);
        return this;
    }
}