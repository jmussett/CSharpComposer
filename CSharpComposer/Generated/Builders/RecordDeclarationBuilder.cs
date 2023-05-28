using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IRecordDeclarationBuilder : ITypeDeclarationBuilder<IRecordDeclarationBuilder>
{
    IRecordDeclarationBuilder WithClassOrStructKeyword(ClassOrStructKeyword classOrStructKeyword);
    IRecordDeclarationBuilder AddParameter(string identifier, Action<IParameterBuilder> parameterCallback);
    IRecordDeclarationBuilder AddParameter(ParameterSyntax parameter);
    IRecordDeclarationBuilder WithOpenBraceToken();
    IRecordDeclarationBuilder WithCloseBraceToken();
}

public interface IWithRecordDeclarationBuilder<TBuilder>
{
    TBuilder WithRecordDeclaration(RecordDeclarationKind kind, string identifier, Action<IRecordDeclarationBuilder> recordDeclarationCallback);
    TBuilder WithRecordDeclaration(RecordDeclarationSyntax recordDeclarationSyntax);
}

public partial class RecordDeclarationBuilder : IRecordDeclarationBuilder
{
    public RecordDeclarationSyntax Syntax { get; set; }

    public RecordDeclarationBuilder(RecordDeclarationSyntax syntax)
    {
        Syntax = syntax;
    }

    public static RecordDeclarationSyntax CreateSyntax(RecordDeclarationKind kind, string identifier, Action<IRecordDeclarationBuilder> recordDeclarationCallback)
    {
        var syntaxKind = kind switch
        {
            RecordDeclarationKind.RecordDeclaration => SyntaxKind.RecordDeclaration,
            RecordDeclarationKind.RecordStructDeclaration => SyntaxKind.RecordStructDeclaration,
            _ => throw new NotSupportedException()
        };
        var keywordToken = SyntaxFactory.Token(SyntaxKind.RecordKeyword);
        var identifierToken = SyntaxFactory.Identifier(identifier);
        var syntax = SyntaxFactory.RecordDeclaration(syntaxKind, default(SyntaxList<AttributeListSyntax>), default(SyntaxTokenList), keywordToken, default(SyntaxToken), identifierToken, default(TypeParameterListSyntax), default(ParameterListSyntax), default(BaseListSyntax), default(SyntaxList<TypeParameterConstraintClauseSyntax>), default(SyntaxToken), default(SyntaxList<MemberDeclarationSyntax>), default(SyntaxToken), default(SyntaxToken));
        var builder = new RecordDeclarationBuilder(syntax);
        recordDeclarationCallback(builder);
        return builder.Syntax;
    }

    public IRecordDeclarationBuilder AddAttribute(Action<INameBuilder> nameCallback, Action<IAttributeBuilder> attributeCallback)
    {
        var attribute = AttributeBuilder.CreateSyntax(nameCallback, attributeCallback);
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public IRecordDeclarationBuilder AddAttribute(AttributeSyntax attribute)
    {
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public IRecordDeclarationBuilder WithClassOrStructKeyword(ClassOrStructKeyword classOrStructKeyword)
    {
        Syntax = Syntax.WithClassOrStructKeyword(SyntaxFactory.Token(classOrStructKeyword switch
        {
            ClassOrStructKeyword.ClassKeyword => SyntaxKind.ClassKeyword,
            ClassOrStructKeyword.StructKeyword => SyntaxKind.StructKeyword,
            _ => throw new NotSupportedException()
        }));
        return this;
    }

    public IRecordDeclarationBuilder AddTypeParameter(string identifier, Action<ITypeParameterBuilder> typeParameterCallback)
    {
        var parameter = TypeParameterBuilder.CreateSyntax(identifier, typeParameterCallback);
        Syntax = Syntax.AddTypeParameterListParameters(parameter);
        return this;
    }

    public IRecordDeclarationBuilder AddTypeParameter(TypeParameterSyntax parameter)
    {
        Syntax = Syntax.AddTypeParameterListParameters(parameter);
        return this;
    }

    public IRecordDeclarationBuilder AddParameter(string identifier, Action<IParameterBuilder> parameterCallback)
    {
        var parameter = ParameterBuilder.CreateSyntax(identifier, parameterCallback);
        Syntax = Syntax.AddParameterListParameters(parameter);
        return this;
    }

    public IRecordDeclarationBuilder AddParameter(ParameterSyntax parameter)
    {
        Syntax = Syntax.AddParameterListParameters(parameter);
        return this;
    }

    public IRecordDeclarationBuilder AddBase(Action<IBaseTypeBuilder> typeCallback)
    {
        var type = BaseTypeBuilder.CreateSyntax(typeCallback);
        Syntax = Syntax.AddBaseListTypes(type);
        return this;
    }

    public IRecordDeclarationBuilder AddBase(BaseTypeSyntax type)
    {
        Syntax = Syntax.AddBaseListTypes(type);
        return this;
    }

    public IRecordDeclarationBuilder AddConstraintClause(string nameIdentifier, Action<ITypeParameterConstraintClauseBuilder> typeParameterConstraintClauseCallback)
    {
        var constraintClause = TypeParameterConstraintClauseBuilder.CreateSyntax(nameIdentifier, typeParameterConstraintClauseCallback);
        Syntax = Syntax.AddConstraintClauses(constraintClause);
        return this;
    }

    public IRecordDeclarationBuilder AddConstraintClause(TypeParameterConstraintClauseSyntax constraintClause)
    {
        Syntax = Syntax.AddConstraintClauses(constraintClause);
        return this;
    }

    public IRecordDeclarationBuilder WithOpenBraceToken()
    {
        Syntax = Syntax.WithOpenBraceToken(SyntaxFactory.Token(SyntaxKind.OpenBraceToken));
        return this;
    }

    public IRecordDeclarationBuilder AddMember(Action<IMemberDeclarationBuilder> memberCallback)
    {
        var member = MemberDeclarationBuilder.CreateSyntax(memberCallback);
        Syntax = Syntax.AddMembers(member);
        return this;
    }

    public IRecordDeclarationBuilder AddMember(MemberDeclarationSyntax member)
    {
        Syntax = Syntax.AddMembers(member);
        return this;
    }

    public IRecordDeclarationBuilder WithCloseBraceToken()
    {
        Syntax = Syntax.WithCloseBraceToken(SyntaxFactory.Token(SyntaxKind.CloseBraceToken));
        return this;
    }

    public IRecordDeclarationBuilder WithSemicolonToken()
    {
        Syntax = Syntax.WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken));
        return this;
    }
}