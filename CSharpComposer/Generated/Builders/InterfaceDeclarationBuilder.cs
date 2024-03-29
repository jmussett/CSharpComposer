﻿using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IInterfaceDeclarationBuilder : ITypeDeclarationBuilder<IInterfaceDeclarationBuilder>
{
}

internal partial class InterfaceDeclarationBuilder : IInterfaceDeclarationBuilder
{
    public InterfaceDeclarationSyntax Syntax { get; set; }

    public InterfaceDeclarationBuilder(InterfaceDeclarationSyntax syntax)
    {
        Syntax = syntax;
    }

    public static InterfaceDeclarationSyntax CreateSyntax(string identifier, Action<IInterfaceDeclarationBuilder>? interfaceDeclarationCallback = null)
    {
        var keywordToken = SyntaxFactory.Token(SyntaxKind.InterfaceKeyword);
        var identifierToken = SyntaxFactory.Identifier(identifier);
        var openBraceTokenToken = SyntaxFactory.Token(SyntaxKind.OpenBraceToken);
        var closeBraceTokenToken = SyntaxFactory.Token(SyntaxKind.CloseBraceToken);
        var syntax = SyntaxFactory.InterfaceDeclaration(default(SyntaxList<AttributeListSyntax>), default(SyntaxTokenList), keywordToken, identifierToken, default(TypeParameterListSyntax), default(BaseListSyntax), default(SyntaxList<TypeParameterConstraintClauseSyntax>), openBraceTokenToken, default(SyntaxList<MemberDeclarationSyntax>), closeBraceTokenToken, default(SyntaxToken));
        var builder = new InterfaceDeclarationBuilder(syntax);
        interfaceDeclarationCallback?.Invoke(builder);
        return builder.Syntax;
    }

    public IInterfaceDeclarationBuilder AddAttribute(Action<INameBuilder> nameCallback, Action<IAttributeBuilder>? attributeCallback = null)
    {
        var attribute = AttributeBuilder.CreateSyntax(nameCallback, attributeCallback);
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public IInterfaceDeclarationBuilder AddAttribute(AttributeSyntax attribute)
    {
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public IInterfaceDeclarationBuilder AddModifierToken(SyntaxKind modifier)
    {
        Syntax = Syntax.AddModifiers(SyntaxFactory.Token(modifier));
        return this;
    }

    public IInterfaceDeclarationBuilder AddTypeParameter(string identifier, Action<ITypeParameterBuilder>? typeParameterCallback = null)
    {
        var parameter = TypeParameterBuilder.CreateSyntax(identifier, typeParameterCallback);
        Syntax = Syntax.AddTypeParameterListParameters(parameter);
        return this;
    }

    public IInterfaceDeclarationBuilder AddTypeParameter(TypeParameterSyntax parameter)
    {
        Syntax = Syntax.AddTypeParameterListParameters(parameter);
        return this;
    }

    public IInterfaceDeclarationBuilder AddBaseType(Action<IBaseTypeBuilder> typeCallback)
    {
        var type = BaseTypeBuilder.CreateSyntax(typeCallback);
        Syntax = Syntax.AddBaseListTypes(type);
        return this;
    }

    public IInterfaceDeclarationBuilder AddBaseType(BaseTypeSyntax type)
    {
        Syntax = Syntax.AddBaseListTypes(type);
        return this;
    }

    public IInterfaceDeclarationBuilder AddTypeParameterConstraintClause(string nameIdentifier, Action<ITypeParameterConstraintClauseBuilder> typeParameterConstraintClauseCallback)
    {
        var constraintClause = TypeParameterConstraintClauseBuilder.CreateSyntax(nameIdentifier, typeParameterConstraintClauseCallback);
        Syntax = Syntax.AddConstraintClauses(constraintClause);
        return this;
    }

    public IInterfaceDeclarationBuilder AddTypeParameterConstraintClause(TypeParameterConstraintClauseSyntax constraintClause)
    {
        Syntax = Syntax.AddConstraintClauses(constraintClause);
        return this;
    }

    public IInterfaceDeclarationBuilder AddMemberDeclaration(Action<IMemberDeclarationBuilder> memberCallback)
    {
        var member = MemberDeclarationBuilder.CreateSyntax(memberCallback);
        Syntax = Syntax.AddMembers(member);
        return this;
    }

    public IInterfaceDeclarationBuilder AddMemberDeclaration(MemberDeclarationSyntax member)
    {
        Syntax = Syntax.AddMembers(member);
        return this;
    }

    public IInterfaceDeclarationBuilder WithSemicolonToken()
    {
        Syntax = Syntax.WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken));
        return this;
    }
}