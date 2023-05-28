using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IBaseNamespaceDeclarationBuilder
{
    void AsNamespaceDeclaration(Action<INameBuilder> nameCallback, Action<INamespaceDeclarationBuilder> namespaceDeclarationCallback);
    void AsFileScopedNamespaceDeclaration(Action<INameBuilder> nameCallback, Action<IFileScopedNamespaceDeclarationBuilder> fileScopedNamespaceDeclarationCallback);
}

public partial interface IBaseNamespaceDeclarationBuilder<TBuilder> : IMemberDeclarationBuilder<TBuilder>
{
}

public interface IWithBaseNamespaceDeclarationBuilder<TBuilder>
{
    TBuilder WithBaseNamespaceDeclaration(Action<IBaseNamespaceDeclarationBuilder> baseNamespaceDeclarationCallback);
    TBuilder WithBaseNamespaceDeclaration(BaseNamespaceDeclarationSyntax baseNamespaceDeclarationSyntax);
}

public partial class BaseNamespaceDeclarationBuilder : IBaseNamespaceDeclarationBuilder
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

    public void AsNamespaceDeclaration(Action<INameBuilder> nameCallback, Action<INamespaceDeclarationBuilder> namespaceDeclarationCallback)
    {
        Syntax = NamespaceDeclarationBuilder.CreateSyntax(nameCallback, namespaceDeclarationCallback);
    }

    public void AsFileScopedNamespaceDeclaration(Action<INameBuilder> nameCallback, Action<IFileScopedNamespaceDeclarationBuilder> fileScopedNamespaceDeclarationCallback)
    {
        Syntax = FileScopedNamespaceDeclarationBuilder.CreateSyntax(nameCallback, fileScopedNamespaceDeclarationCallback);
    }
}