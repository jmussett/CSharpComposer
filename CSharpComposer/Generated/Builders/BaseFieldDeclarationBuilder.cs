﻿using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IBaseFieldDeclarationBuilder
{
    void FromSyntax(BaseFieldDeclarationSyntax syntax);
    void AsFieldDeclaration(Action<ITypeBuilder> declarationTypeCallback, Action<IVariableDeclarationBuilder> declarationVariableDeclarationCallback, Action<IFieldDeclarationBuilder> fieldDeclarationCallback);
    void AsEventFieldDeclaration(Action<ITypeBuilder> declarationTypeCallback, Action<IVariableDeclarationBuilder> declarationVariableDeclarationCallback, Action<IEventFieldDeclarationBuilder> eventFieldDeclarationCallback);
}

public partial interface IBaseFieldDeclarationBuilder<TBuilder> : IMemberDeclarationBuilder<TBuilder>
{
}

internal partial class BaseFieldDeclarationBuilder : IBaseFieldDeclarationBuilder
{
    public BaseFieldDeclarationSyntax? Syntax { get; set; }

    public static BaseFieldDeclarationSyntax CreateSyntax(Action<IBaseFieldDeclarationBuilder> callback)
    {
        var builder = new BaseFieldDeclarationBuilder();
        callback(builder);
        if (builder.Syntax is null)
        {
            throw new InvalidOperationException("BaseFieldDeclarationSyntax has not been specified");
        }

        return builder.Syntax;
    }

    public void FromSyntax(BaseFieldDeclarationSyntax syntax)
    {
        Syntax = syntax;
    }

    public void AsFieldDeclaration(Action<ITypeBuilder> declarationTypeCallback, Action<IVariableDeclarationBuilder> declarationVariableDeclarationCallback, Action<IFieldDeclarationBuilder> fieldDeclarationCallback)
    {
        Syntax = FieldDeclarationBuilder.CreateSyntax(declarationTypeCallback, declarationVariableDeclarationCallback, fieldDeclarationCallback);
    }

    public void AsEventFieldDeclaration(Action<ITypeBuilder> declarationTypeCallback, Action<IVariableDeclarationBuilder> declarationVariableDeclarationCallback, Action<IEventFieldDeclarationBuilder> eventFieldDeclarationCallback)
    {
        Syntax = EventFieldDeclarationBuilder.CreateSyntax(declarationTypeCallback, declarationVariableDeclarationCallback, eventFieldDeclarationCallback);
    }
}