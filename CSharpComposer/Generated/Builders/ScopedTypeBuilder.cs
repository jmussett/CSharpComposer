﻿using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
internal partial class ScopedTypeBuilder
{
    public static ScopedTypeSyntax CreateSyntax(Action<ITypeBuilder> typeCallback)
    {
        var scopedKeywordToken = SyntaxFactory.Token(SyntaxKind.ScopedKeyword);
        var typeSyntax = TypeBuilder.CreateSyntax(typeCallback);
        return SyntaxFactory.ScopedType(scopedKeywordToken, typeSyntax);
    }
}