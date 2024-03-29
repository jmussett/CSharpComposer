﻿using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IXmlNodeBuilder
{
    void FromSyntax(XmlNodeSyntax syntax);
    void AsXmlElement(string nameStartTagLocalName, Action<IXmlNameBuilder> nameStartTagXmlNameCallback, Action<IXmlElementStartTagBuilder> startTagXmlElementStartTagCallback, string nameEndTagLocalName, Action<IXmlNameBuilder> nameEndTagXmlNameCallback, Action<IXmlElementBuilder> xmlElementCallback);
    void AsXmlEmptyElement(string nameLocalName, Action<IXmlNameBuilder> nameXmlNameCallback, Action<IXmlEmptyElementBuilder> xmlEmptyElementCallback);
    void AsXmlText(Action<IXmlTextBuilder>? xmlTextCallback = null);
    void AsXmlCDataSection(Action<IXmlCDataSectionBuilder>? xmlCDataSectionCallback = null);
    void AsXmlProcessingInstruction(string nameLocalName, Action<IXmlNameBuilder> nameXmlNameCallback, Action<IXmlProcessingInstructionBuilder> xmlProcessingInstructionCallback);
    void AsXmlComment(Action<IXmlCommentBuilder>? xmlCommentCallback = null);
}

internal partial class XmlNodeBuilder : IXmlNodeBuilder
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

    public void FromSyntax(XmlNodeSyntax syntax)
    {
        Syntax = syntax;
    }

    public void AsXmlElement(string nameStartTagLocalName, Action<IXmlNameBuilder> nameStartTagXmlNameCallback, Action<IXmlElementStartTagBuilder> startTagXmlElementStartTagCallback, string nameEndTagLocalName, Action<IXmlNameBuilder> nameEndTagXmlNameCallback, Action<IXmlElementBuilder> xmlElementCallback)
    {
        Syntax = XmlElementBuilder.CreateSyntax(nameStartTagLocalName, nameStartTagXmlNameCallback, startTagXmlElementStartTagCallback, nameEndTagLocalName, nameEndTagXmlNameCallback, xmlElementCallback);
    }

    public void AsXmlEmptyElement(string nameLocalName, Action<IXmlNameBuilder> nameXmlNameCallback, Action<IXmlEmptyElementBuilder> xmlEmptyElementCallback)
    {
        Syntax = XmlEmptyElementBuilder.CreateSyntax(nameLocalName, nameXmlNameCallback, xmlEmptyElementCallback);
    }

    public void AsXmlText(Action<IXmlTextBuilder>? xmlTextCallback = null)
    {
        Syntax = XmlTextBuilder.CreateSyntax(xmlTextCallback);
    }

    public void AsXmlCDataSection(Action<IXmlCDataSectionBuilder>? xmlCDataSectionCallback = null)
    {
        Syntax = XmlCDataSectionBuilder.CreateSyntax(xmlCDataSectionCallback);
    }

    public void AsXmlProcessingInstruction(string nameLocalName, Action<IXmlNameBuilder> nameXmlNameCallback, Action<IXmlProcessingInstructionBuilder> xmlProcessingInstructionCallback)
    {
        Syntax = XmlProcessingInstructionBuilder.CreateSyntax(nameLocalName, nameXmlNameCallback, xmlProcessingInstructionCallback);
    }

    public void AsXmlComment(Action<IXmlCommentBuilder>? xmlCommentCallback = null)
    {
        Syntax = XmlCommentBuilder.CreateSyntax(xmlCommentCallback);
    }
}