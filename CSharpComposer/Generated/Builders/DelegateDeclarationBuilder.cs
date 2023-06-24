﻿using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IDelegateDeclarationBuilder : IMemberDeclarationBuilder<IDelegateDeclarationBuilder>, IAddTypeParameter<IDelegateDeclarationBuilder>, IAddParameter<IDelegateDeclarationBuilder>
{
    IDelegateDeclarationBuilder AddTypeParameterConstraintClause(string nameIdentifier, Action<ITypeParameterConstraintClauseBuilder> typeParameterConstraintClauseCallback);
    IDelegateDeclarationBuilder AddTypeParameterConstraintClause(TypeParameterConstraintClauseSyntax constraintClause);
}

public interface IWithDelegateDeclaration<TBuilder>
{
    TBuilder WithDelegateDeclaration(Action<ITypeBuilder> returnTypeCallback, string identifier, Action<IDelegateDeclarationBuilder> delegateDeclarationCallback);
    TBuilder WithDelegateDeclaration(DelegateDeclarationSyntax delegateDeclarationSyntax);
}

public interface IAddDelegateDeclaration<TBuilder>
{
    TBuilder AddDelegateDeclaration(Action<ITypeBuilder> returnTypeCallback, string identifier, Action<IDelegateDeclarationBuilder> delegateDeclarationCallback);
    TBuilder AddDelegateDeclaration(DelegateDeclarationSyntax delegateDeclarationSyntax);
}

public partial class DelegateDeclarationBuilder : IDelegateDeclarationBuilder
{
    public DelegateDeclarationSyntax Syntax { get; set; }

    public DelegateDeclarationBuilder(DelegateDeclarationSyntax syntax)
    {
        Syntax = syntax;
    }

    public static DelegateDeclarationSyntax CreateSyntax(Action<ITypeBuilder> returnTypeCallback, string identifier, Action<IDelegateDeclarationBuilder> delegateDeclarationCallback)
    {
        var delegateKeywordToken = SyntaxFactory.Token(SyntaxKind.DelegateKeyword);
        var returnTypeSyntax = TypeBuilder.CreateSyntax(returnTypeCallback);
        var identifierToken = SyntaxFactory.Identifier(identifier);
        var parameterListSyntax = SyntaxFactory.ParameterList();
        var semicolonTokenToken = SyntaxFactory.Token(SyntaxKind.SemicolonToken);
        var syntax = SyntaxFactory.DelegateDeclaration(default(SyntaxList<AttributeListSyntax>), default(SyntaxTokenList), delegateKeywordToken, returnTypeSyntax, identifierToken, default(TypeParameterListSyntax), parameterListSyntax, default(SyntaxList<TypeParameterConstraintClauseSyntax>), semicolonTokenToken);
        var builder = new DelegateDeclarationBuilder(syntax);
        delegateDeclarationCallback(builder);
        return builder.Syntax;
    }

    public IDelegateDeclarationBuilder AddAttribute(Action<INameBuilder> nameCallback, Action<IAttributeBuilder> attributeCallback)
    {
        var attribute = AttributeBuilder.CreateSyntax(nameCallback, attributeCallback);
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public IDelegateDeclarationBuilder AddAttribute(AttributeSyntax attribute)
    {
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public IDelegateDeclarationBuilder AddModifierToken(SyntaxToken modifier)
    {
        Syntax = Syntax.AddModifiers(modifier);
        return this;
    }

    public IDelegateDeclarationBuilder AddTypeParameter(string identifier, Action<ITypeParameterBuilder> typeParameterCallback)
    {
        var parameter = TypeParameterBuilder.CreateSyntax(identifier, typeParameterCallback);
        Syntax = Syntax.AddTypeParameterListParameters(parameter);
        return this;
    }

    public IDelegateDeclarationBuilder AddTypeParameter(TypeParameterSyntax parameter)
    {
        Syntax = Syntax.AddTypeParameterListParameters(parameter);
        return this;
    }

    public IDelegateDeclarationBuilder AddParameter(string identifier, Action<IParameterBuilder> parameterCallback)
    {
        var parameter = ParameterBuilder.CreateSyntax(identifier, parameterCallback);
        Syntax = Syntax.AddParameterListParameters(parameter);
        return this;
    }

    public IDelegateDeclarationBuilder AddParameter(ParameterSyntax parameter)
    {
        Syntax = Syntax.AddParameterListParameters(parameter);
        return this;
    }

    public IDelegateDeclarationBuilder AddTypeParameterConstraintClause(string nameIdentifier, Action<ITypeParameterConstraintClauseBuilder> typeParameterConstraintClauseCallback)
    {
        var constraintClause = TypeParameterConstraintClauseBuilder.CreateSyntax(nameIdentifier, typeParameterConstraintClauseCallback);
        Syntax = Syntax.AddConstraintClauses(constraintClause);
        return this;
    }

    public IDelegateDeclarationBuilder AddTypeParameterConstraintClause(TypeParameterConstraintClauseSyntax constraintClause)
    {
        Syntax = Syntax.AddConstraintClauses(constraintClause);
        return this;
    }
}