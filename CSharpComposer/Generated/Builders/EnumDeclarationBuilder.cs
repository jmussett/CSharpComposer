using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IEnumDeclarationBuilder : IBaseTypeDeclarationBuilder<IEnumDeclarationBuilder>
{
    IEnumDeclarationBuilder AddEnumMemberDeclaration(string identifier, Action<IEnumMemberDeclarationBuilder>? enumMemberDeclarationCallback = null);
    IEnumDeclarationBuilder AddEnumMemberDeclaration(EnumMemberDeclarationSyntax member);
}

public partial class EnumDeclarationBuilder : IEnumDeclarationBuilder
{
    public EnumDeclarationSyntax Syntax { get; set; }

    public EnumDeclarationBuilder(EnumDeclarationSyntax syntax)
    {
        Syntax = syntax;
    }

    public static EnumDeclarationSyntax CreateSyntax(string identifier, Action<IEnumDeclarationBuilder>? enumDeclarationCallback = null)
    {
        var enumKeywordToken = SyntaxFactory.Token(SyntaxKind.EnumKeyword);
        var identifierToken = SyntaxFactory.Identifier(identifier);
        var openBraceTokenToken = SyntaxFactory.Token(SyntaxKind.OpenBraceToken);
        var closeBraceTokenToken = SyntaxFactory.Token(SyntaxKind.CloseBraceToken);
        var syntax = SyntaxFactory.EnumDeclaration(default(SyntaxList<AttributeListSyntax>), default(SyntaxTokenList), enumKeywordToken, identifierToken, default(BaseListSyntax), openBraceTokenToken, default(SeparatedSyntaxList<EnumMemberDeclarationSyntax>), closeBraceTokenToken, default(SyntaxToken));
        var builder = new EnumDeclarationBuilder(syntax);
        enumDeclarationCallback?.Invoke(builder);
        return builder.Syntax;
    }

    public IEnumDeclarationBuilder AddAttribute(Action<INameBuilder> nameCallback, Action<IAttributeBuilder>? attributeCallback = null)
    {
        var attribute = AttributeBuilder.CreateSyntax(nameCallback, attributeCallback);
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public IEnumDeclarationBuilder AddAttribute(AttributeSyntax attribute)
    {
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public IEnumDeclarationBuilder AddModifierToken(SyntaxToken modifier)
    {
        Syntax = Syntax.AddModifiers(modifier);
        return this;
    }

    public IEnumDeclarationBuilder AddBaseType(Action<IBaseTypeBuilder> typeCallback)
    {
        var type = BaseTypeBuilder.CreateSyntax(typeCallback);
        Syntax = Syntax.AddBaseListTypes(type);
        return this;
    }

    public IEnumDeclarationBuilder AddBaseType(BaseTypeSyntax type)
    {
        Syntax = Syntax.AddBaseListTypes(type);
        return this;
    }

    public IEnumDeclarationBuilder AddEnumMemberDeclaration(string identifier, Action<IEnumMemberDeclarationBuilder>? enumMemberDeclarationCallback = null)
    {
        var member = EnumMemberDeclarationBuilder.CreateSyntax(identifier, enumMemberDeclarationCallback);
        Syntax = Syntax.AddMembers(member);
        return this;
    }

    public IEnumDeclarationBuilder AddEnumMemberDeclaration(EnumMemberDeclarationSyntax member)
    {
        Syntax = Syntax.AddMembers(member);
        return this;
    }

    public IEnumDeclarationBuilder WithSemicolonToken()
    {
        Syntax = Syntax.WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken));
        return this;
    }
}