﻿using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithScopedType<TBuilder>
{
    TBuilder WithScopedType(Action<ITypeBuilder> typeCallback);
    TBuilder WithScopedType(ScopedTypeSyntax scopedTypeSyntax);
}

public interface IAddScopedType<TBuilder>
{
    TBuilder AddScopedType(Action<ITypeBuilder> typeCallback);
    TBuilder AddScopedType(ScopedTypeSyntax scopedTypeSyntax);
}

public partial class ScopedTypeBuilder
{
    public static ScopedTypeSyntax CreateSyntax(Action<ITypeBuilder> typeCallback)
    {
        var scopedKeywordToken = SyntaxFactory.Token(SyntaxKind.ScopedKeyword);
        var typeSyntax = TypeBuilder.CreateSyntax(typeCallback);
        return SyntaxFactory.ScopedType(scopedKeywordToken, typeSyntax);
    }
}