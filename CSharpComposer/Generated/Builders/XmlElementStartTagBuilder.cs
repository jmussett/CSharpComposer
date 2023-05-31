﻿using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IXmlElementStartTagBuilder
{
    IXmlElementStartTagBuilder AddXmlAttribute(Action<IXmlAttributeBuilder> attributeCallback);
    IXmlElementStartTagBuilder AddXmlAttribute(XmlAttributeSyntax attribute);
}

public interface IWithXmlElementStartTag<TBuilder>
{
    TBuilder WithXmlElementStartTag(string nameLocalName, Action<IXmlNameBuilder> nameXmlNameCallback, Action<IXmlElementStartTagBuilder> xmlElementStartTagCallback);
    TBuilder WithXmlElementStartTag(XmlElementStartTagSyntax xmlElementStartTagSyntax);
}

public interface IAddXmlElementStartTag<TBuilder>
{
    TBuilder AddXmlElementStartTag(string nameLocalName, Action<IXmlNameBuilder> nameXmlNameCallback, Action<IXmlElementStartTagBuilder> xmlElementStartTagCallback);
    TBuilder AddXmlElementStartTag(XmlElementStartTagSyntax xmlElementStartTagSyntax);
}

public partial class XmlElementStartTagBuilder : IXmlElementStartTagBuilder
{
    public XmlElementStartTagSyntax Syntax { get; set; }

    public XmlElementStartTagBuilder(XmlElementStartTagSyntax syntax)
    {
        Syntax = syntax;
    }

    public static XmlElementStartTagSyntax CreateSyntax(string nameLocalName, Action<IXmlNameBuilder> nameXmlNameCallback, Action<IXmlElementStartTagBuilder> xmlElementStartTagCallback)
    {
        var lessThanTokenToken = SyntaxFactory.Token(SyntaxKind.LessThanToken);
        var nameSyntax = XmlNameBuilder.CreateSyntax(nameLocalName, nameXmlNameCallback);
        var greaterThanTokenToken = SyntaxFactory.Token(SyntaxKind.GreaterThanToken);
        var syntax = SyntaxFactory.XmlElementStartTag(lessThanTokenToken, nameSyntax, default(SyntaxList<XmlAttributeSyntax>), greaterThanTokenToken);
        var builder = new XmlElementStartTagBuilder(syntax);
        xmlElementStartTagCallback(builder);
        return builder.Syntax;
    }

    public IXmlElementStartTagBuilder AddXmlAttribute(Action<IXmlAttributeBuilder> attributeCallback)
    {
        var attribute = XmlAttributeBuilder.CreateSyntax(attributeCallback);
        Syntax = Syntax.AddAttributes(attribute);
        return this;
    }

    public IXmlElementStartTagBuilder AddXmlAttribute(XmlAttributeSyntax attribute)
    {
        Syntax = Syntax.AddAttributes(attribute);
        return this;
    }
}