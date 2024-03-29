﻿using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IXmlTextAttributeBuilder
{
    IXmlTextAttributeBuilder AddToken(SyntaxKind textToken);
}

internal partial class XmlTextAttributeBuilder : IXmlTextAttributeBuilder
{
    public XmlTextAttributeSyntax Syntax { get; set; }

    public XmlTextAttributeBuilder(XmlTextAttributeSyntax syntax)
    {
        Syntax = syntax;
    }

    public static XmlTextAttributeSyntax CreateSyntax(string nameLocalName, Action<IXmlNameBuilder> nameXmlNameCallback, XmlTextAttributeStartQuoteToken xmlTextAttributeStartQuoteToken, XmlTextAttributeEndQuoteToken xmlTextAttributeEndQuoteToken, Action<IXmlTextAttributeBuilder> xmlTextAttributeCallback)
    {
        var nameSyntax = XmlNameBuilder.CreateSyntax(nameLocalName, nameXmlNameCallback);
        var equalsTokenToken = SyntaxFactory.Token(SyntaxKind.EqualsToken);
        var startQuoteTokenToken = xmlTextAttributeStartQuoteToken switch
        {
            XmlTextAttributeStartQuoteToken.SingleQuoteToken => SyntaxFactory.Token(SyntaxKind.SingleQuoteToken),
            XmlTextAttributeStartQuoteToken.DoubleQuoteToken => SyntaxFactory.Token(SyntaxKind.DoubleQuoteToken),
            _ => throw new NotSupportedException()
        };
        var endQuoteTokenToken = xmlTextAttributeEndQuoteToken switch
        {
            XmlTextAttributeEndQuoteToken.SingleQuoteToken => SyntaxFactory.Token(SyntaxKind.SingleQuoteToken),
            XmlTextAttributeEndQuoteToken.DoubleQuoteToken => SyntaxFactory.Token(SyntaxKind.DoubleQuoteToken),
            _ => throw new NotSupportedException()
        };
        var syntax = SyntaxFactory.XmlTextAttribute(nameSyntax, equalsTokenToken, startQuoteTokenToken, default(SyntaxTokenList), endQuoteTokenToken);
        var builder = new XmlTextAttributeBuilder(syntax);
        xmlTextAttributeCallback?.Invoke(builder);
        return builder.Syntax;
    }

    public IXmlTextAttributeBuilder AddToken(SyntaxKind textToken)
    {
        Syntax = Syntax.AddTextTokens(SyntaxFactory.Token(textToken));
        return this;
    }
}