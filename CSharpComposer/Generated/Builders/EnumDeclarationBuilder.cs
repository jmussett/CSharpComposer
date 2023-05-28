using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IEnumDeclarationBuilder : IBaseTypeDeclarationBuilder<IEnumDeclarationBuilder>
{
    IEnumDeclarationBuilder AddMember(string identifier, Action<IEnumMemberDeclarationBuilder> enumMemberDeclarationCallback);
    IEnumDeclarationBuilder AddMember(EnumMemberDeclarationSyntax member);
}

public interface IWithEnumDeclarationBuilder<TBuilder>
{
    TBuilder WithEnumDeclaration(string identifier, Action<IEnumDeclarationBuilder> enumDeclarationCallback);
    TBuilder WithEnumDeclaration(EnumDeclarationSyntax enumDeclarationSyntax);
}

public partial class EnumDeclarationBuilder : IEnumDeclarationBuilder
{
    public EnumDeclarationSyntax Syntax { get; set; }

    public EnumDeclarationBuilder(EnumDeclarationSyntax syntax)
    {
        Syntax = syntax;
    }

    public static EnumDeclarationSyntax CreateSyntax(string identifier, Action<IEnumDeclarationBuilder> enumDeclarationCallback)
    {
        var enumKeywordToken = SyntaxFactory.Token(SyntaxKind.EnumKeyword);
        var identifierToken = SyntaxFactory.Identifier(identifier);
        var openBraceTokenToken = SyntaxFactory.Token(SyntaxKind.OpenBraceToken);
        var closeBraceTokenToken = SyntaxFactory.Token(SyntaxKind.CloseBraceToken);
        var syntax = SyntaxFactory.EnumDeclaration(default(SyntaxList<AttributeListSyntax>), default(SyntaxTokenList), enumKeywordToken, identifierToken, default(BaseListSyntax), openBraceTokenToken, default(SeparatedSyntaxList<EnumMemberDeclarationSyntax>), closeBraceTokenToken, default(SyntaxToken));
        var builder = new EnumDeclarationBuilder(syntax);
        enumDeclarationCallback(builder);
        return builder.Syntax;
    }

    public IEnumDeclarationBuilder AddAttribute(Action<INameBuilder> nameCallback, Action<IAttributeBuilder> attributeCallback)
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

    public IEnumDeclarationBuilder AddBase(Action<IBaseTypeBuilder> typeCallback)
    {
        var type = BaseTypeBuilder.CreateSyntax(typeCallback);
        Syntax = Syntax.AddBaseListTypes(type);
        return this;
    }

    public IEnumDeclarationBuilder AddBase(BaseTypeSyntax type)
    {
        Syntax = Syntax.AddBaseListTypes(type);
        return this;
    }

    public IEnumDeclarationBuilder AddMember(string identifier, Action<IEnumMemberDeclarationBuilder> enumMemberDeclarationCallback)
    {
        var member = EnumMemberDeclarationBuilder.CreateSyntax(identifier, enumMemberDeclarationCallback);
        Syntax = Syntax.AddMembers(member);
        return this;
    }

    public IEnumDeclarationBuilder AddMember(EnumMemberDeclarationSyntax member)
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