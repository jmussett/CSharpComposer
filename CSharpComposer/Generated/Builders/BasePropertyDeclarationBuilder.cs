﻿using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IBasePropertyDeclarationBuilder
{
    void AsPropertyDeclaration(Action<ITypeBuilder> typeCallback, string identifier, Action<IPropertyDeclarationBuilder> propertyDeclarationCallback);
    void AsEventDeclaration(Action<ITypeBuilder> typeCallback, string identifier, Action<IEventDeclarationBuilder> eventDeclarationCallback);
    void AsIndexerDeclaration(Action<ITypeBuilder> typeCallback, Action<IIndexerDeclarationBuilder> indexerDeclarationCallback);
}

public partial interface IBasePropertyDeclarationBuilder<TBuilder> : IMemberDeclarationBuilder<TBuilder>, IWithExplicitInterfaceSpecifier<TBuilder>, IAddAccessorDeclaration<TBuilder>
{
}

public interface IWithBasePropertyDeclaration<TBuilder>
{
    TBuilder WithBasePropertyDeclaration(Action<IBasePropertyDeclarationBuilder> basePropertyDeclarationCallback);
    TBuilder WithBasePropertyDeclaration(BasePropertyDeclarationSyntax basePropertyDeclarationSyntax);
}

public interface IAddBasePropertyDeclaration<TBuilder>
{
    TBuilder AddBasePropertyDeclaration(Action<IBasePropertyDeclarationBuilder> basePropertyDeclarationCallback);
    TBuilder AddBasePropertyDeclaration(BasePropertyDeclarationSyntax basePropertyDeclarationSyntax);
}

public partial class BasePropertyDeclarationBuilder : IBasePropertyDeclarationBuilder
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

    public void AsPropertyDeclaration(Action<ITypeBuilder> typeCallback, string identifier, Action<IPropertyDeclarationBuilder> propertyDeclarationCallback)
    {
        Syntax = PropertyDeclarationBuilder.CreateSyntax(typeCallback, identifier, propertyDeclarationCallback);
    }

    public void AsEventDeclaration(Action<ITypeBuilder> typeCallback, string identifier, Action<IEventDeclarationBuilder> eventDeclarationCallback)
    {
        Syntax = EventDeclarationBuilder.CreateSyntax(typeCallback, identifier, eventDeclarationCallback);
    }

    public void AsIndexerDeclaration(Action<ITypeBuilder> typeCallback, Action<IIndexerDeclarationBuilder> indexerDeclarationCallback)
    {
        Syntax = IndexerDeclarationBuilder.CreateSyntax(typeCallback, indexerDeclarationCallback);
    }
}