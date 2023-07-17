using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
internal partial class OmittedTypeArgumentBuilder
{
    public static OmittedTypeArgumentSyntax CreateSyntax()
    {
        var omittedTypeArgumentTokenToken = SyntaxFactory.Token(SyntaxKind.OmittedTypeArgumentToken);
        return SyntaxFactory.OmittedTypeArgument(omittedTypeArgumentTokenToken);
    }
}