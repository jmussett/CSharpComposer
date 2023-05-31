﻿using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithXmlPrefix<TBuilder>
{
    TBuilder WithXmlPrefix(string prefix);
    TBuilder WithXmlPrefix(XmlPrefixSyntax xmlPrefixSyntax);
}

public interface IAddXmlPrefix<TBuilder>
{
    TBuilder AddXmlPrefix(string prefix);
    TBuilder AddXmlPrefix(XmlPrefixSyntax xmlPrefixSyntax);
}

public partial class XmlPrefixBuilder
{
    public static XmlPrefixSyntax CreateSyntax(string prefix)
    {
        var prefixToken = SyntaxFactory.Identifier(prefix);
        var colonTokenToken = SyntaxFactory.Token(SyntaxKind.ColonToken);
        return SyntaxFactory.XmlPrefix(prefixToken, colonTokenToken);
    }
}