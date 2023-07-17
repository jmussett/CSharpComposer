using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IBasePropertyDeclarationBuilder
{
    void AsPropertyDeclaration(Action<ITypeBuilder> typeCallback, string identifier, Action<IPropertyDeclarationBuilder>? propertyDeclarationCallback = null);
    void AsEventDeclaration(Action<ITypeBuilder> typeCallback, string identifier, Action<IEventDeclarationBuilder>? eventDeclarationCallback = null);
    void AsIndexerDeclaration(Action<ITypeBuilder> typeCallback, Action<IIndexerDeclarationBuilder>? indexerDeclarationCallback = null);
}

public partial interface IBasePropertyDeclarationBuilder<TBuilder> : IMemberDeclarationBuilder<TBuilder>, IWithExplicitInterfaceSpecifier<TBuilder>, IAddAccessorDeclaration<TBuilder>
{
}

internal partial class BasePropertyDeclarationBuilder : IBasePropertyDeclarationBuilder
{
    public BasePropertyDeclarationSyntax? Syntax { get; set; }

    public static BasePropertyDeclarationSyntax CreateSyntax(Action<IBasePropertyDeclarationBuilder> callback)
    {
        var builder = new BasePropertyDeclarationBuilder();
        callback(builder);
        if (builder.Syntax is null)
        {
            throw new InvalidOperationException("BasePropertyDeclarationSyntax has not been specified");
        }

        return builder.Syntax;
    }

    public void AsPropertyDeclaration(Action<ITypeBuilder> typeCallback, string identifier, Action<IPropertyDeclarationBuilder>? propertyDeclarationCallback = null)
    {
        Syntax = PropertyDeclarationBuilder.CreateSyntax(typeCallback, identifier, propertyDeclarationCallback);
    }

    public void AsEventDeclaration(Action<ITypeBuilder> typeCallback, string identifier, Action<IEventDeclarationBuilder>? eventDeclarationCallback = null)
    {
        Syntax = EventDeclarationBuilder.CreateSyntax(typeCallback, identifier, eventDeclarationCallback);
    }

    public void AsIndexerDeclaration(Action<ITypeBuilder> typeCallback, Action<IIndexerDeclarationBuilder>? indexerDeclarationCallback = null)
    {
        Syntax = IndexerDeclarationBuilder.CreateSyntax(typeCallback, indexerDeclarationCallback);
    }
}