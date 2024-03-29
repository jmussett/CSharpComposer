﻿using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
internal partial class SimpleBaseTypeBuilder
{
    public static SimpleBaseTypeSyntax CreateSyntax(Action<ITypeBuilder> typeCallback)
    {
        var typeSyntax = TypeBuilder.CreateSyntax(typeCallback);
        return SyntaxFactory.SimpleBaseType(typeSyntax);
    }
}