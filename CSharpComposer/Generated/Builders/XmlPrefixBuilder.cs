using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial class XmlPrefixBuilder
{
    public static XmlPrefixSyntax CreateSyntax(string prefix)
    {
        var prefixToken = SyntaxFactory.Identifier(prefix);
        var colonTokenToken = SyntaxFactory.Token(SyntaxKind.ColonToken);
        return SyntaxFactory.XmlPrefix(prefixToken, colonTokenToken);
    }
}