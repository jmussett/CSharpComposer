using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IClassDeclarationBuilder : ITypeDeclarationBuilder<IClassDeclarationBuilder>
{
}

public interface IWithClassDeclarationBuilder<TBuilder>
{
    TBuilder WithClassDeclaration(string identifier, Action<IClassDeclarationBuilder> classDeclarationCallback);
    TBuilder WithClassDeclaration(ClassDeclarationSyntax classDeclarationSyntax);
}

public partial class ClassDeclarationBuilder : IClassDeclarationBuilder
{
    public ClassDeclarationSyntax Syntax { get; set; }

    public ClassDeclarationBuilder(ClassDeclarationSyntax syntax)
    {
        Syntax = syntax;
    }

    public static ClassDeclarationSyntax CreateSyntax(string identifier, Action<IClassDeclarationBuilder> classDeclarationCallback)
    {
        var keywordToken = SyntaxFactory.Token(SyntaxKind.ClassKeyword);
        var identifierToken = SyntaxFactory.Identifier(identifier);
        var openBraceTokenToken = SyntaxFactory.Token(SyntaxKind.OpenBraceToken);
        var closeBraceTokenToken = SyntaxFactory.Token(SyntaxKind.CloseBraceToken);
        var syntax = SyntaxFactory.ClassDeclaration(default(SyntaxList<AttributeListSyntax>), default(SyntaxTokenList), keywordToken, identifierToken, default(TypeParameterListSyntax), default(BaseListSyntax), default(SyntaxList<TypeParameterConstraintClauseSyntax>), openBraceTokenToken, default(SyntaxList<MemberDeclarationSyntax>), closeBraceTokenToken, default(SyntaxToken));
        var builder = new ClassDeclarationBuilder(syntax);
        classDeclarationCallback(builder);
        return builder.Syntax;
    }

    public IClassDeclarationBuilder AddAttribute(Action<INameBuilder> nameCallback, Action<IAttributeBuilder> attributeCallback)
    {
        var attribute = AttributeBuilder.CreateSyntax(nameCallback, attributeCallback);
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public IClassDeclarationBuilder AddAttribute(AttributeSyntax attribute)
    {
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public IClassDeclarationBuilder AddTypeParameter(string identifier, Action<ITypeParameterBuilder> typeParameterCallback)
    {
        var parameter = TypeParameterBuilder.CreateSyntax(identifier, typeParameterCallback);
        Syntax = Syntax.AddTypeParameterListParameters(parameter);
        return this;
    }

    public IClassDeclarationBuilder AddTypeParameter(TypeParameterSyntax parameter)
    {
        Syntax = Syntax.AddTypeParameterListParameters(parameter);
        return this;
    }

    public IClassDeclarationBuilder AddBase(Action<IBaseTypeBuilder> typeCallback)
    {
        var type = BaseTypeBuilder.CreateSyntax(typeCallback);
        Syntax = Syntax.AddBaseListTypes(type);
        return this;
    }

    public IClassDeclarationBuilder AddBase(BaseTypeSyntax type)
    {
        Syntax = Syntax.AddBaseListTypes(type);
        return this;
    }

    public IClassDeclarationBuilder AddConstraintClause(string nameIdentifier, Action<ITypeParameterConstraintClauseBuilder> typeParameterConstraintClauseCallback)
    {
        var constraintClause = TypeParameterConstraintClauseBuilder.CreateSyntax(nameIdentifier, typeParameterConstraintClauseCallback);
        Syntax = Syntax.AddConstraintClauses(constraintClause);
        return this;
    }

    public IClassDeclarationBuilder AddConstraintClause(TypeParameterConstraintClauseSyntax constraintClause)
    {
        Syntax = Syntax.AddConstraintClauses(constraintClause);
        return this;
    }

    public IClassDeclarationBuilder AddMember(Action<IMemberDeclarationBuilder> memberCallback)
    {
        var member = MemberDeclarationBuilder.CreateSyntax(memberCallback);
        Syntax = Syntax.AddMembers(member);
        return this;
    }

    public IClassDeclarationBuilder AddMember(MemberDeclarationSyntax member)
    {
        Syntax = Syntax.AddMembers(member);
        return this;
    }

    public IClassDeclarationBuilder WithSemicolonToken()
    {
        Syntax = Syntax.WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken));
        return this;
    }
}