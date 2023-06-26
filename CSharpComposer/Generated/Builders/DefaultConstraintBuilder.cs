using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial class DefaultConstraintBuilder
{
    public static DefaultConstraintSyntax CreateSyntax()
    {
        var defaultKeywordToken = SyntaxFactory.Token(SyntaxKind.DefaultKeyword);
        return SyntaxFactory.DefaultConstraint(defaultKeywordToken);
    }
}