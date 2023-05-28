using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IXmlEmptyElementBuilder
{
    IXmlEmptyElementBuilder AddAttribute(Action<IXmlAttributeBuilder> attributeCallback);
    IXmlEmptyElementBuilder AddAttribute(XmlAttributeSyntax attribute);
}

public interface IWithXmlEmptyElementBuilder<TBuilder>
{
    TBuilder WithXmlEmptyElement(string nameLocalName, Action<IXmlNameBuilder> nameXmlNameCallback, Action<IXmlEmptyElementBuilder> xmlEmptyElementCallback);
    TBuilder WithXmlEmptyElement(XmlEmptyElementSyntax xmlEmptyElementSyntax);
}

public partial class XmlEmptyElementBuilder : IXmlEmptyElementBuilder
{
    public XmlEmptyElementSyntax Syntax { get; set; }

    public XmlEmptyElementBuilder(XmlEmptyElementSyntax syntax)
    {
        Syntax = syntax;
    }

    public static XmlEmptyElementSyntax CreateSyntax(string nameLocalName, Action<IXmlNameBuilder> nameXmlNameCallback, Action<IXmlEmptyElementBuilder> xmlEmptyElementCallback)
    {
        var lessThanTokenToken = SyntaxFactory.Token(SyntaxKind.LessThanToken);
        var nameSyntax = XmlNameBuilder.CreateSyntax(nameLocalName, nameXmlNameCallback);
        var slashGreaterThanTokenToken = SyntaxFactory.Token(SyntaxKind.SlashGreaterThanToken);
        var syntax = SyntaxFactory.XmlEmptyElement(lessThanTokenToken, nameSyntax, default(SyntaxList<XmlAttributeSyntax>), slashGreaterThanTokenToken);
        var builder = new XmlEmptyElementBuilder(syntax);
        xmlEmptyElementCallback(builder);
        return builder.Syntax;
    }

    public IXmlEmptyElementBuilder AddAttribute(Action<IXmlAttributeBuilder> attributeCallback)
    {
        var attribute = XmlAttributeBuilder.CreateSyntax(attributeCallback);
        Syntax = Syntax.AddAttributes(attribute);
        return this;
    }

    public IXmlEmptyElementBuilder AddAttribute(XmlAttributeSyntax attribute)
    {
        Syntax = Syntax.AddAttributes(attribute);
        return this;
    }
}