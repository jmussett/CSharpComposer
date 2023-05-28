using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface INamespaceDeclarationBuilder : IBaseNamespaceDeclarationBuilder<INamespaceDeclarationBuilder>
{
    INamespaceDeclarationBuilder WithSemicolonToken();
}

public interface IWithNamespaceDeclarationBuilder<TBuilder>
{
    TBuilder WithNamespaceDeclaration(Action<INameBuilder> nameCallback, Action<INamespaceDeclarationBuilder> namespaceDeclarationCallback);
    TBuilder WithNamespaceDeclaration(NamespaceDeclarationSyntax namespaceDeclarationSyntax);
}

public partial class NamespaceDeclarationBuilder : INamespaceDeclarationBuilder
{
    public NamespaceDeclarationSyntax Syntax { get; set; }

    public NamespaceDeclarationBuilder(NamespaceDeclarationSyntax syntax)
    {
        Syntax = syntax;
    }

    public static NamespaceDeclarationSyntax CreateSyntax(Action<INameBuilder> nameCallback, Action<INamespaceDeclarationBuilder> namespaceDeclarationCallback)
    {
        var namespaceKeywordToken = SyntaxFactory.Token(SyntaxKind.NamespaceKeyword);
        var nameSyntax = NameBuilder.CreateSyntax(nameCallback);
        var openBraceTokenToken = SyntaxFactory.Token(SyntaxKind.OpenBraceToken);
        var closeBraceTokenToken = SyntaxFactory.Token(SyntaxKind.CloseBraceToken);
        var syntax = SyntaxFactory.NamespaceDeclaration(default(SyntaxList<AttributeListSyntax>), default(SyntaxTokenList), namespaceKeywordToken, nameSyntax, openBraceTokenToken, default(SyntaxList<ExternAliasDirectiveSyntax>), default(SyntaxList<UsingDirectiveSyntax>), default(SyntaxList<MemberDeclarationSyntax>), closeBraceTokenToken, default(SyntaxToken));
        var builder = new NamespaceDeclarationBuilder(syntax);
        namespaceDeclarationCallback(builder);
        return builder.Syntax;
    }

    public INamespaceDeclarationBuilder AddAttribute(Action<INameBuilder> nameCallback, Action<IAttributeBuilder> attributeCallback)
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

    public INamespaceDeclarationBuilder AddExtern(string identifier)
    {
        var @extern = ExternAliasDirectiveBuilder.CreateSyntax(identifier);
        Syntax = Syntax.AddExterns(@extern);
        return this;
    }

    public INamespaceDeclarationBuilder AddExtern(ExternAliasDirectiveSyntax @extern)
    {
        Syntax = Syntax.AddExterns(@extern);
        return this;
    }

    public INamespaceDeclarationBuilder AddUsing(Action<INameBuilder> nameCallback, Action<IUsingDirectiveBuilder> usingDirectiveCallback)
    {
        var @using = UsingDirectiveBuilder.CreateSyntax(nameCallback, usingDirectiveCallback);
        Syntax = Syntax.AddUsings(@using);
        return this;
    }

    public INamespaceDeclarationBuilder AddUsing(UsingDirectiveSyntax @using)
    {
        Syntax = Syntax.AddUsings(@using);
        return this;
    }

    public INamespaceDeclarationBuilder AddMember(Action<IMemberDeclarationBuilder> memberCallback)
    {
        var member = MemberDeclarationBuilder.CreateSyntax(memberCallback);
        Syntax = Syntax.AddMembers(member);
        return this;
    }

    public INamespaceDeclarationBuilder AddMember(MemberDeclarationSyntax member)
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