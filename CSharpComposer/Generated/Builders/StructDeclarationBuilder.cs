using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IStructDeclarationBuilder : ITypeDeclarationBuilder<IStructDeclarationBuilder>
{
}

public interface IWithStructDeclarationBuilder<TBuilder>
{
    TBuilder WithStructDeclaration(string identifier, Action<IStructDeclarationBuilder> structDeclarationCallback);
    TBuilder WithStructDeclaration(StructDeclarationSyntax structDeclarationSyntax);
}

public partial class StructDeclarationBuilder : IStructDeclarationBuilder
{
    public StructDeclarationSyntax Syntax { get; set; }

    public StructDeclarationBuilder(StructDeclarationSyntax syntax)
    {
        Syntax = syntax;
    }

    public static StructDeclarationSyntax CreateSyntax(string identifier, Action<IStructDeclarationBuilder> structDeclarationCallback)
    {
        var keywordToken = SyntaxFactory.Token(SyntaxKind.StructKeyword);
        var identifierToken = SyntaxFactory.Identifier(identifier);
        var openBraceTokenToken = SyntaxFactory.Token(SyntaxKind.OpenBraceToken);
        var closeBraceTokenToken = SyntaxFactory.Token(SyntaxKind.CloseBraceToken);
        var syntax = SyntaxFactory.StructDeclaration(default(SyntaxList<AttributeListSyntax>), default(SyntaxTokenList), keywordToken, identifierToken, default(TypeParameterListSyntax), default(BaseListSyntax), default(SyntaxList<TypeParameterConstraintClauseSyntax>), openBraceTokenToken, default(SyntaxList<MemberDeclarationSyntax>), closeBraceTokenToken, default(SyntaxToken));
        var builder = new StructDeclarationBuilder(syntax);
        structDeclarationCallback(builder);
        return builder.Syntax;
    }

    public IStructDeclarationBuilder AddAttribute(Action<INameBuilder> nameCallback, Action<IAttributeBuilder> attributeCallback)
    {
        var attribute = AttributeBuilder.CreateSyntax(nameCallback, attributeCallback);
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public IStructDeclarationBuilder AddAttribute(AttributeSyntax attribute)
    {
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public IStructDeclarationBuilder AddTypeParameter(string identifier, Action<ITypeParameterBuilder> typeParameterCallback)
    {
        var parameter = TypeParameterBuilder.CreateSyntax(identifier, typeParameterCallback);
        Syntax = Syntax.AddTypeParameterListParameters(parameter);
        return this;
    }

    public IStructDeclarationBuilder AddTypeParameter(TypeParameterSyntax parameter)
    {
        Syntax = Syntax.AddTypeParameterListParameters(parameter);
        return this;
    }

    public IStructDeclarationBuilder AddBase(Action<IBaseTypeBuilder> typeCallback)
    {
        var type = BaseTypeBuilder.CreateSyntax(typeCallback);
        Syntax = Syntax.AddBaseListTypes(type);
        return this;
    }

    public IStructDeclarationBuilder AddBase(BaseTypeSyntax type)
    {
        Syntax = Syntax.AddBaseListTypes(type);
        return this;
    }

    public IStructDeclarationBuilder AddConstraintClause(string nameIdentifier, Action<ITypeParameterConstraintClauseBuilder> typeParameterConstraintClauseCallback)
    {
        var constraintClause = TypeParameterConstraintClauseBuilder.CreateSyntax(nameIdentifier, typeParameterConstraintClauseCallback);
        Syntax = Syntax.AddConstraintClauses(constraintClause);
        return this;
    }

    public IStructDeclarationBuilder AddConstraintClause(TypeParameterConstraintClauseSyntax constraintClause)
    {
        Syntax = Syntax.AddConstraintClauses(constraintClause);
        return this;
    }

    public IStructDeclarationBuilder AddMember(Action<IMemberDeclarationBuilder> memberCallback)
    {
        var member = MemberDeclarationBuilder.CreateSyntax(memberCallback);
        Syntax = Syntax.AddMembers(member);
        return this;
    }

    public IStructDeclarationBuilder AddMember(MemberDeclarationSyntax member)
    {
        Syntax = Syntax.AddMembers(member);
        return this;
    }

    public IStructDeclarationBuilder WithSemicolonToken()
    {
        Syntax = Syntax.WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken));
        return this;
    }
}