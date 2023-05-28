using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IXmlElementBuilder
{
    IXmlElementBuilder AddContent(Action<IXmlNodeBuilder> contentCallback);
    IXmlElementBuilder AddContent(XmlNodeSyntax content);
}

public interface IWithXmlElementBuilder<TBuilder>
{
    TBuilder WithXmlElement(string nameStartTagLocalName, Action<IXmlNameBuilder> nameStartTagXmlNameCallback, Action<IXmlElementStartTagBuilder> startTagXmlElementStartTagCallback, string nameEndTagLocalName, Action<IXmlNameBuilder> nameEndTagXmlNameCallback, Action<IXmlElementBuilder> xmlElementCallback);
    TBuilder WithXmlElement(XmlElementSyntax xmlElementSyntax);
}

public partial class XmlElementBuilder : IXmlElementBuilder
{
    public XmlElementSyntax Syntax { get; set; }

    public XmlElementBuilder(XmlElementSyntax syntax)
    {
        Syntax = syntax;
    }

    public static XmlElementSyntax CreateSyntax(string nameStartTagLocalName, Action<IXmlNameBuilder> nameStartTagXmlNameCallback, Action<IXmlElementStartTagBuilder> startTagXmlElementStartTagCallback, string nameEndTagLocalName, Action<IXmlNameBuilder> nameEndTagXmlNameCallback, Action<IXmlElementBuilder> xmlElementCallback)
    {
        var startTagSyntax = XmlElementStartTagBuilder.CreateSyntax(nameStartTagLocalName, nameStartTagXmlNameCallback, startTagXmlElementStartTagCallback);
        var endTagSyntax = XmlElementEndTagBuilder.CreateSyntax(nameEndTagLocalName, nameEndTagXmlNameCallback);
        var syntax = SyntaxFactory.XmlElement(startTagSyntax, default(SyntaxList<XmlNodeSyntax>), endTagSyntax);
        var builder = new XmlElementBuilder(syntax);
        xmlElementCallback(builder);
        return builder.Syntax;
    }

    public IXmlElementBuilder AddContent(Action<IXmlNodeBuilder> contentCallback)
    {
        var content = XmlNodeBuilder.CreateSyntax(contentCallback);
        Syntax = Syntax.AddContent(content);
        return this;
    }

    public IXmlElementBuilder AddContent(XmlNodeSyntax content)
    {
        Syntax = Syntax.AddContent(content);
        return this;
    }
}