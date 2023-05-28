using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IXmlNodeBuilder
{
    void AsXmlElement(string nameStartTagLocalName, Action<IXmlNameBuilder> nameStartTagXmlNameCallback, Action<IXmlElementStartTagBuilder> startTagXmlElementStartTagCallback, string nameEndTagLocalName, Action<IXmlNameBuilder> nameEndTagXmlNameCallback, Action<IXmlElementBuilder> xmlElementCallback);
    void AsXmlEmptyElement(string nameLocalName, Action<IXmlNameBuilder> nameXmlNameCallback, Action<IXmlEmptyElementBuilder> xmlEmptyElementCallback);
    void AsXmlText(Action<IXmlTextBuilder> xmlTextCallback);
    void AsXmlCDataSection(Action<IXmlCDataSectionBuilder> xmlCDataSectionCallback);
    void AsXmlProcessingInstruction(string nameLocalName, Action<IXmlNameBuilder> nameXmlNameCallback, Action<IXmlProcessingInstructionBuilder> xmlProcessingInstructionCallback);
    void AsXmlComment(Action<IXmlCommentBuilder> xmlCommentCallback);
}

public interface IWithXmlNodeBuilder<TBuilder>
{
    TBuilder WithXmlNode(Action<IXmlNodeBuilder> xmlNodeCallback);
    TBuilder WithXmlNode(XmlNodeSyntax xmlNodeSyntax);
}

public partial class XmlNodeBuilder : IXmlNodeBuilder
{
    public XmlNodeSyntax? Syntax { get; set; }

    public static XmlNodeSyntax CreateSyntax(Action<IXmlNodeBuilder> callback)
    {
        var builder = new XmlNodeBuilder();
        callback(builder);
        if (builder.Syntax is null)
        {
            throw new InvalidOperationException("XmlNodeSyntax has not been specified");
        }

        return builder.Syntax;
    }

    public void AsXmlElement(string nameStartTagLocalName, Action<IXmlNameBuilder> nameStartTagXmlNameCallback, Action<IXmlElementStartTagBuilder> startTagXmlElementStartTagCallback, string nameEndTagLocalName, Action<IXmlNameBuilder> nameEndTagXmlNameCallback, Action<IXmlElementBuilder> xmlElementCallback)
    {
        Syntax = XmlElementBuilder.CreateSyntax(nameStartTagLocalName, nameStartTagXmlNameCallback, startTagXmlElementStartTagCallback, nameEndTagLocalName, nameEndTagXmlNameCallback, xmlElementCallback);
    }

    public void AsXmlEmptyElement(string nameLocalName, Action<IXmlNameBuilder> nameXmlNameCallback, Action<IXmlEmptyElementBuilder> xmlEmptyElementCallback)
    {
        Syntax = XmlEmptyElementBuilder.CreateSyntax(nameLocalName, nameXmlNameCallback, xmlEmptyElementCallback);
    }

    public void AsXmlText(Action<IXmlTextBuilder> xmlTextCallback)
    {
        Syntax = XmlTextBuilder.CreateSyntax(xmlTextCallback);
    }

    public void AsXmlCDataSection(Action<IXmlCDataSectionBuilder> xmlCDataSectionCallback)
    {
        Syntax = XmlCDataSectionBuilder.CreateSyntax(xmlCDataSectionCallback);
    }

    public void AsXmlProcessingInstruction(string nameLocalName, Action<IXmlNameBuilder> nameXmlNameCallback, Action<IXmlProcessingInstructionBuilder> xmlProcessingInstructionCallback)
    {
        Syntax = XmlProcessingInstructionBuilder.CreateSyntax(nameLocalName, nameXmlNameCallback, xmlProcessingInstructionCallback);
    }

    public void AsXmlComment(Action<IXmlCommentBuilder> xmlCommentCallback)
    {
        Syntax = XmlCommentBuilder.CreateSyntax(xmlCommentCallback);
    }
}