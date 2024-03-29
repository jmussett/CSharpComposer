﻿using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IRefTypeBuilder
{
    IRefTypeBuilder WithReadOnlyKeyword();
}

internal partial class RefTypeBuilder : IRefTypeBuilder
{
    public RefTypeSyntax Syntax { get; set; }

    public RefTypeBuilder(RefTypeSyntax syntax)
    {
        Syntax = syntax;
    }

    public static RefTypeSyntax CreateSyntax(Action<ITypeBuilder> typeCallback, Action<IRefTypeBuilder>? refTypeCallback = null)
    {
        var refKeywordToken = SyntaxFactory.Token(SyntaxKind.RefKeyword);
        var typeSyntax = TypeBuilder.CreateSyntax(typeCallback);
        var syntax = SyntaxFactory.RefType(refKeywordToken, default(SyntaxToken), typeSyntax);
        var builder = new RefTypeBuilder(syntax);
        refTypeCallback?.Invoke(builder);
        return builder.Syntax;
    }

    public IRefTypeBuilder WithReadOnlyKeyword()
    {
        Syntax = Syntax.WithReadOnlyKeyword(SyntaxFactory.Token(SyntaxKind.ReadOnlyKeyword));
        return this;
    }
}