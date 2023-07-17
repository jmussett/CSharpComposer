using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IXmlElementBuilder
{
    IXmlElementBuilder AddContentXmlNode(Action<IXmlNodeBuilder> contentCallback);
    IXmlElementBuilder AddContentXmlNode(XmlNodeSyntax content);
}

internal partial class XmlElementBuilder : IXmlElementBuilder
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
        xmlElementCallback?.Invoke(builder);
        return builder.Syntax;
    }

    public IXmlElementBuilder AddContentXmlNode(Action<IXmlNodeBuilder> contentCallback)
    {
        var content = XmlNodeBuilder.CreateSyntax(contentCallback);
        Syntax = Syntax.AddContent(content);
        return this;
    }

    public IXmlElementBuilder AddContentXmlNode(XmlNodeSyntax content)
    {
        Syntax = Syntax.AddContent(content);
        return this;
    }
}