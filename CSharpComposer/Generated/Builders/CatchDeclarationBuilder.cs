﻿using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface ICatchDeclarationBuilder
{
    ICatchDeclarationBuilder WithIdentifier(string identifier);
}

internal partial class CatchDeclarationBuilder : ICatchDeclarationBuilder
{
    public CatchDeclarationSyntax Syntax { get; set; }

    public CatchDeclarationBuilder(CatchDeclarationSyntax syntax)
    {
        Syntax = syntax;
    }

    public static CatchDeclarationSyntax CreateSyntax(Action<ITypeBuilder> typeCallback, Action<ICatchDeclarationBuilder>? catchDeclarationCallback = null)
    {
        var openParenTokenToken = SyntaxFactory.Token(SyntaxKind.OpenParenToken);
        var typeSyntax = TypeBuilder.CreateSyntax(typeCallback);
        var closeParenTokenToken = SyntaxFactory.Token(SyntaxKind.CloseParenToken);
        var syntax = SyntaxFactory.CatchDeclaration(openParenTokenToken, typeSyntax, default(SyntaxToken), closeParenTokenToken);
        var builder = new CatchDeclarationBuilder(syntax);
        catchDeclarationCallback?.Invoke(builder);
        return builder.Syntax;
    }

    public ICatchDeclarationBuilder WithIdentifier(string identifier)
    {
        Syntax = Syntax.WithIdentifier(SyntaxFactory.Identifier(identifier));
        return this;
    }
}