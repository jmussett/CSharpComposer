using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface INamespaceDeclarationBuilder : IBaseNamespaceDeclarationBuilder<INamespaceDeclarationBuilder>
{
    INamespaceDeclarationBuilder WithSemicolonToken();
}

public partial class NamespaceDeclarationBuilder : INamespaceDeclarationBuilder
{
    public NamespaceDeclarationSyntax Syntax { get; set; }

    public NamespaceDeclarationBuilder(NamespaceDeclarationSyntax syntax)
    {
        Syntax = syntax;
    }

    public static NamespaceDeclarationSyntax CreateSyntax(Action<INameBuilder> nameCallback, Action<INamespaceDeclarationBuilder>? namespaceDeclarationCallback = null)
    {
        var namespaceKeywordToken = SyntaxFactory.Token(SyntaxKind.NamespaceKeyword);
        var nameSyntax = NameBuilder.CreateSyntax(nameCallback);
        var openBraceTokenToken = SyntaxFactory.Token(SyntaxKind.OpenBraceToken);
        var closeBraceTokenToken = SyntaxFactory.Token(SyntaxKind.CloseBraceToken);
        var syntax = SyntaxFactory.NamespaceDeclaration(default(SyntaxList<AttributeListSyntax>), default(SyntaxTokenList), namespaceKeywordToken, nameSyntax, openBraceTokenToken, default(SyntaxList<ExternAliasDirectiveSyntax>), default(SyntaxList<UsingDirectiveSyntax>), default(SyntaxList<MemberDeclarationSyntax>), closeBraceTokenToken, default(SyntaxToken));
        var builder = new NamespaceDeclarationBuilder(syntax);
        namespaceDeclarationCallback?.Invoke(builder);
        return builder.Syntax;
    }

    public INamespaceDeclarationBuilder AddAttribute(Action<INameBuilder> nameCallback, Action<IAttributeBuilder>? attributeCallback = null)
    {
        var attribute = AttributeBuilder.CreateSyntax(nameCallback, attributeCallback);
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public INamespaceDeclarationBuilder AddAttribute(AttributeSyntax attribute)
    {
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public INamespaceDeclarationBuilder AddModifierToken(SyntaxToken modifier)
    {
        Syntax = Syntax.AddModifiers(modifier);
        return this;
    }

    public INamespaceDeclarationBuilder AddExternAliasDirective(string identifier)
    {
        var @extern = ExternAliasDirectiveBuilder.CreateSyntax(identifier);
        Syntax = Syntax.AddExterns(@extern);
        return this;
    }

    public INamespaceDeclarationBuilder AddExternAliasDirective(ExternAliasDirectiveSyntax @extern)
    {
        Syntax = Syntax.AddExterns(@extern);
        return this;
    }

    public INamespaceDeclarationBuilder AddUsingDirective(Action<INameBuilder> nameCallback, Action<IUsingDirectiveBuilder>? usingDirectiveCallback = null)
    {
        var @using = UsingDirectiveBuilder.CreateSyntax(nameCallback, usingDirectiveCallback);
        Syntax = Syntax.AddUsings(@using);
        return this;
    }

    public INamespaceDeclarationBuilder AddUsingDirective(UsingDirectiveSyntax @using)
    {
        Syntax = Syntax.AddUsings(@using);
        return this;
    }

    public INamespaceDeclarationBuilder AddMemberDeclaration(Action<IMemberDeclarationBuilder> memberCallback)
    {
        var member = MemberDeclarationBuilder.CreateSyntax(memberCallback);
        Syntax = Syntax.AddMembers(member);
        return this;
    }

    public INamespaceDeclarationBuilder AddMemberDeclaration(MemberDeclarationSyntax member)
    {
        Syntax = Syntax.AddMembers(member);
        return this;
    }

    public INamespaceDeclarationBuilder WithSemicolonToken()
    {
        Syntax = Syntax.WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken));
        return this;
    }
}