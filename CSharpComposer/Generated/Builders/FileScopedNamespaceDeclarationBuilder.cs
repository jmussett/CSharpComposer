﻿using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IFileScopedNamespaceDeclarationBuilder : IBaseNamespaceDeclarationBuilder<IFileScopedNamespaceDeclarationBuilder>, IAddAttribute<IFileScopedNamespaceDeclarationBuilder>
{
}

public interface IWithFileScopedNamespaceDeclaration<TBuilder>
{
    TBuilder WithFileScopedNamespaceDeclaration(Action<INameBuilder> nameCallback, Action<IFileScopedNamespaceDeclarationBuilder> fileScopedNamespaceDeclarationCallback);
    TBuilder WithFileScopedNamespaceDeclaration(FileScopedNamespaceDeclarationSyntax fileScopedNamespaceDeclarationSyntax);
}

public interface IAddFileScopedNamespaceDeclaration<TBuilder>
{
    TBuilder AddFileScopedNamespaceDeclaration(Action<INameBuilder> nameCallback, Action<IFileScopedNamespaceDeclarationBuilder> fileScopedNamespaceDeclarationCallback);
    TBuilder AddFileScopedNamespaceDeclaration(FileScopedNamespaceDeclarationSyntax fileScopedNamespaceDeclarationSyntax);
}

public partial class FileScopedNamespaceDeclarationBuilder : IFileScopedNamespaceDeclarationBuilder
{
    public FileScopedNamespaceDeclarationSyntax Syntax { get; set; }

    public FileScopedNamespaceDeclarationBuilder(FileScopedNamespaceDeclarationSyntax syntax)
    {
        Syntax = syntax;
    }

    public static FileScopedNamespaceDeclarationSyntax CreateSyntax(Action<INameBuilder> nameCallback, Action<IFileScopedNamespaceDeclarationBuilder> fileScopedNamespaceDeclarationCallback)
    {
        var namespaceKeywordToken = SyntaxFactory.Token(SyntaxKind.NamespaceKeyword);
        var nameSyntax = NameBuilder.CreateSyntax(nameCallback);
        var semicolonTokenToken = SyntaxFactory.Token(SyntaxKind.SemicolonToken);
        var syntax = SyntaxFactory.FileScopedNamespaceDeclaration(default(SyntaxList<AttributeListSyntax>), default(SyntaxTokenList), namespaceKeywordToken, nameSyntax, semicolonTokenToken, default(SyntaxList<ExternAliasDirectiveSyntax>), default(SyntaxList<UsingDirectiveSyntax>), default(SyntaxList<MemberDeclarationSyntax>));
        var builder = new FileScopedNamespaceDeclarationBuilder(syntax);
        fileScopedNamespaceDeclarationCallback(builder);
        return builder.Syntax;
    }

    public IFileScopedNamespaceDeclarationBuilder AddAttribute(Action<INameBuilder> nameCallback, Action<IAttributeBuilder> attributeCallback)
    {
        var attribute = AttributeBuilder.CreateSyntax(nameCallback, attributeCallback);
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public IFileScopedNamespaceDeclarationBuilder AddAttribute(AttributeSyntax attribute)
    {
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public IFileScopedNamespaceDeclarationBuilder AddModifierToken(SyntaxToken modifier)
    {
        Syntax = Syntax.AddModifiers(modifier);
        return this;
    }

    public IFileScopedNamespaceDeclarationBuilder AddExternAliasDirective(string identifier)
    {
        var @extern = ExternAliasDirectiveBuilder.CreateSyntax(identifier);
        Syntax = Syntax.AddExterns(@extern);
        return this;
    }

    public IFileScopedNamespaceDeclarationBuilder AddExternAliasDirective(ExternAliasDirectiveSyntax @extern)
    {
        Syntax = Syntax.AddExterns(@extern);
        return this;
    }

    public IFileScopedNamespaceDeclarationBuilder AddUsingDirective(Action<INameBuilder> nameCallback, Action<IUsingDirectiveBuilder> usingDirectiveCallback)
    {
        var @using = UsingDirectiveBuilder.CreateSyntax(nameCallback, usingDirectiveCallback);
        Syntax = Syntax.AddUsings(@using);
        return this;
    }

    public IFileScopedNamespaceDeclarationBuilder AddUsingDirective(UsingDirectiveSyntax @using)
    {
        Syntax = Syntax.AddUsings(@using);
        return this;
    }

    public IFileScopedNamespaceDeclarationBuilder AddMemberDeclaration(Action<IMemberDeclarationBuilder> memberCallback)
    {
        var member = MemberDeclarationBuilder.CreateSyntax(memberCallback);
        Syntax = Syntax.AddMembers(member);
        return this;
    }

    public IFileScopedNamespaceDeclarationBuilder AddMemberDeclaration(MemberDeclarationSyntax member)
    {
        Syntax = Syntax.AddMembers(member);
        return this;
    }
}