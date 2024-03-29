﻿using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
internal partial class XmlElementEndTagBuilder
{
    public static XmlElementEndTagSyntax CreateSyntax(string nameLocalName, Action<IXmlNameBuilder> nameXmlNameCallback)
    {
        var lessThanSlashTokenToken = SyntaxFactory.Token(SyntaxKind.LessThanSlashToken);
        var nameSyntax = XmlNameBuilder.CreateSyntax(nameLocalName, nameXmlNameCallback);
        var greaterThanTokenToken = SyntaxFactory.Token(SyntaxKind.GreaterThanToken);
        return SyntaxFactory.XmlElementEndTag(lessThanSlashTokenToken, nameSyntax, greaterThanTokenToken);
    }
}