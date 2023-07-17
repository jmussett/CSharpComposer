using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IBaseNamespaceDeclarationBuilder
{
    void AsNamespaceDeclaration(Action<INameBuilder> nameCallback, Action<INamespaceDeclarationBuilder>? namespaceDeclarationCallback = null);
    void AsFileScopedNamespaceDeclaration(Action<INameBuilder> nameCallback, Action<IFileScopedNamespaceDeclarationBuilder>? fileScopedNamespaceDeclarationCallback = null);
}

public partial interface IBaseNamespaceDeclarationBuilder<TBuilder> : IMemberDeclarationBuilder<TBuilder>
{
    TBuilder AddExternAliasDirective(string identifier);
    TBuilder AddExternAliasDirective(ExternAliasDirectiveSyntax @extern);
    TBuilder AddUsingDirective(Action<INameBuilder> nameCallback, Action<IUsingDirectiveBuilder>? usingDirectiveCallback = null);
    TBuilder AddUsingDirective(UsingDirectiveSyntax @using);
    TBuilder AddMemberDeclaration(Action<IMemberDeclarationBuilder> memberCallback);
    TBuilder AddMemberDeclaration(MemberDeclarationSyntax member);
}

internal partial class BaseNamespaceDeclarationBuilder : IBaseNamespaceDeclarationBuilder
{
    public BaseNamespaceDeclarationSyntax? Syntax { get; set; }

    public static BaseNamespaceDeclarationSyntax CreateSyntax(Action<IBaseNamespaceDeclarationBuilder> callback)
    {
        var builder = new BaseNamespaceDeclarationBuilder();
        callback(builder);
        if (builder.Syntax is null)
        {
            throw new InvalidOperationException("BaseNamespaceDeclarationSyntax has not been specified");
        }

        return builder.Syntax;
    }

    public void AsNamespaceDeclaration(Action<INameBuilder> nameCallback, Action<INamespaceDeclarationBuilder>? namespaceDeclarationCallback = null)
    {
        Syntax = NamespaceDeclarationBuilder.CreateSyntax(nameCallback, namespaceDeclarationCallback);
    }

    public void AsFileScopedNamespaceDeclaration(Action<INameBuilder> nameCallback, Action<IFileScopedNamespaceDeclarationBuilder>? fileScopedNamespaceDeclarationCallback = null)
    {
        Syntax = FileScopedNamespaceDeclarationBuilder.CreateSyntax(nameCallback, fileScopedNamespaceDeclarationCallback);
    }
}