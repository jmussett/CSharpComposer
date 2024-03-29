﻿using Microsoft.CodeAnalysis.CSharp;

namespace CSharpComposer;

public partial interface IAccessorDeclarationBuilder
{
    IAccessorDeclarationBuilder WithSemicolon();
}

internal partial class AccessorDeclarationBuilder : IAccessorDeclarationBuilder
{
    public IAccessorDeclarationBuilder WithSemicolon()
    {
        Syntax = Syntax.WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken));

        return this;
    }
}

public partial interface IMethodDeclarationBuilder
{
    IMethodDeclarationBuilder WithSemicolon();
}

internal partial class MethodDeclarationBuilder : IMethodDeclarationBuilder
{
    public IMethodDeclarationBuilder WithSemicolon()
    {
        Syntax = Syntax.WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken));

        return this;
    }
}
