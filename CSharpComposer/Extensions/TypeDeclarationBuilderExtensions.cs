﻿namespace CSharpComposer.Extensions;

public static class TypeDeclarationBuilderExtensions
{
    public static TBuilder AddMethodDeclaration<TBuilder>(this TBuilder builder, Action<ITypeBuilder> returnTypeCallback, string identifier, Action<IMethodDeclarationBuilder> methodDeclarationCallback)
        where TBuilder : ITypeDeclarationBuilder<TBuilder>
    {
        return builder.AddMemberDeclaration(x => x.AsMethodDeclaration(returnTypeCallback, identifier, methodDeclarationCallback));
    }

    public static TBuilder AddPropertyDeclaration<TBuilder>(this TBuilder builder, Action<ITypeBuilder> typeCallback, string identifier, Action<IPropertyDeclarationBuilder> propertyDeclarationCallback)
        where TBuilder : ITypeDeclarationBuilder<TBuilder>
    {
        return builder.AddMemberDeclaration(x => x.AsPropertyDeclaration(typeCallback, identifier, propertyDeclarationCallback));
    }

    public static TBuilder AddEventDeclaration<TBuilder>(this TBuilder builder, Action<ITypeBuilder> typeCallback, string identifier, Action<IEventDeclarationBuilder> eventDeclarationCallback)
        where TBuilder : ITypeDeclarationBuilder<TBuilder>
    {
        return builder.AddMemberDeclaration(x => x.AsEventDeclaration(typeCallback, identifier, eventDeclarationCallback));
    }

    public static TBuilder AddIndexerDeclaration<TBuilder>(this TBuilder builder, Action<ITypeBuilder> typeCallback, Action<IIndexerDeclarationBuilder> indexerDeclarationCallback)
        where TBuilder : ITypeDeclarationBuilder<TBuilder>
    {
        return builder.AddMemberDeclaration(x => x.AsIndexerDeclaration(typeCallback, indexerDeclarationCallback));
    }
}