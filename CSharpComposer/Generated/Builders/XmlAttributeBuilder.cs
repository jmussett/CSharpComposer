using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IXmlAttributeBuilder
{
    void FromSyntax(XmlAttributeSyntax syntax);
    void AsXmlTextAttribute(string nameLocalName, Action<IXmlNameBuilder> nameXmlNameCallback, XmlTextAttributeStartQuoteToken xmlTextAttributeStartQuoteToken, XmlTextAttributeEndQuoteToken xmlTextAttributeEndQuoteToken, Action<IXmlTextAttributeBuilder> xmlTextAttributeCallback);
    void AsXmlCrefAttribute(string nameLocalName, Action<IXmlNameBuilder> nameXmlNameCallback, XmlCrefAttributeStartQuoteToken xmlCrefAttributeStartQuoteToken, Action<ICrefBuilder> crefCallback, XmlCrefAttributeEndQuoteToken xmlCrefAttributeEndQuoteToken);
    void AsXmlNameAttribute(string nameLocalName, Action<IXmlNameBuilder> nameXmlNameCallback, XmlNameAttributeStartQuoteToken xmlNameAttributeStartQuoteToken, string identifierIdentifier, XmlNameAttributeEndQuoteToken xmlNameAttributeEndQuoteToken);
}

internal partial class XmlAttributeBuilder : IXmlAttributeBuilder
{
    public XmlAttributeSyntax? Syntax { get; set; }

    public static XmlAttributeSyntax CreateSyntax(Action<IXmlAttributeBuilder> callback)
    {
        var builder = new XmlAttributeBuilder();
        callback(builder);
        if (builder.Syntax is null)
        {
            throw new InvalidOperationException("XmlAttributeSyntax has not been specified");
        }

        return builder.Syntax;
    }

    public void FromSyntax(XmlAttributeSyntax syntax)
    {
        Syntax = syntax;
    }

    public void AsXmlTextAttribute(string nameLocalName, Action<IXmlNameBuilder> nameXmlNameCallback, XmlTextAttributeStartQuoteToken xmlTextAttributeStartQuoteToken, XmlTextAttributeEndQuoteToken xmlTextAttributeEndQuoteToken, Action<IXmlTextAttributeBuilder> xmlTextAttributeCallback)
    {
        Syntax = XmlTextAttributeBuilder.CreateSyntax(nameLocalName, nameXmlNameCallback, xmlTextAttributeStartQuoteToken, xmlTextAttributeEndQuoteToken, xmlTextAttributeCallback);
    }

    public void AsXmlCrefAttribute(string nameLocalName, Action<IXmlNameBuilder> nameXmlNameCallback, XmlCrefAttributeStartQuoteToken xmlCrefAttributeStartQuoteToken, Action<ICrefBuilder> crefCallback, XmlCrefAttributeEndQuoteToken xmlCrefAttributeEndQuoteToken)
    {
        Syntax = XmlCrefAttributeBuilder.CreateSyntax(nameLocalName, nameXmlNameCallback, xmlCrefAttributeStartQuoteToken, crefCallback, xmlCrefAttributeEndQuoteToken);
    }

    public void AsXmlNameAttribute(string nameLocalName, Action<IXmlNameBuilder> nameXmlNameCallback, XmlNameAttributeStartQuoteToken xmlNameAttributeStartQuoteToken, string identifierIdentifier, XmlNameAttributeEndQuoteToken xmlNameAttributeEndQuoteToken)
    {
        Syntax = XmlNameAttributeBuilder.CreateSyntax(nameLocalName, nameXmlNameCallback, xmlNameAttributeStartQuoteToken, identifierIdentifier, xmlNameAttributeEndQuoteToken);
    }
}