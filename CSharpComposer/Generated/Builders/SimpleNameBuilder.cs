﻿using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface ISimpleNameBuilder
{
    void FromSyntax(SimpleNameSyntax syntax);
    void AsIdentifierName(string identifier);
    void AsGenericName(string identifier, Action<IGenericNameBuilder>? genericNameCallback = null);
}

internal partial class SimpleNameBuilder : ISimpleNameBuilder
{
    public SimpleNameSyntax? Syntax { get; set; }

    public static SimpleNameSyntax CreateSyntax(Action<ISimpleNameBuilder> callback)
    {
        var builder = new SimpleNameBuilder();
        callback(builder);
        if (builder.Syntax is null)
        {
            throw new InvalidOperationException("SimpleNameSyntax has not been specified");
        }

        return builder.Syntax;
    }

    public void FromSyntax(SimpleNameSyntax syntax)
    {
        Syntax = syntax;
    }

    public void AsIdentifierName(string identifier)
    {
        Syntax = IdentifierNameBuilder.CreateSyntax(identifier);
    }

    public void AsGenericName(string identifier, Action<IGenericNameBuilder>? genericNameCallback = null)
    {
        Syntax = GenericNameBuilder.CreateSyntax(identifier, genericNameCallback);
    }
}