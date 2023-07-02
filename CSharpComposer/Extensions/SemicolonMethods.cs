using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpComposer;

public partial interface IAccessorDeclarationBuilder
{
    IAccessorDeclarationBuilder WithSemicolon();
}

public partial class AccessorDeclarationBuilder : IAccessorDeclarationBuilder
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

public partial class MethodDeclarationBuilder : IMethodDeclarationBuilder
{
    public IMethodDeclarationBuilder WithSemicolon()
    {
        Syntax = Syntax.WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken));

        return this;
    }
}
