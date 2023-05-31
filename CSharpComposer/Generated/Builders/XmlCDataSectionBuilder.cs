﻿using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IXmlCDataSectionBuilder
{
    IXmlCDataSectionBuilder AddToken(SyntaxToken textToken);
}

public interface IWithXmlCDataSection<TBuilder>
{
    TBuilder WithXmlCDataSection(Action<IXmlCDataSectionBuilder> xmlCDataSectionCallback);
    TBuilder WithXmlCDataSection(XmlCDataSectionSyntax xmlCDataSectionSyntax);
}

public interface IAddXmlCDataSection<TBuilder>
{
    TBuilder AddXmlCDataSection(Action<IXmlCDataSectionBuilder> xmlCDataSectionCallback);
    TBuilder AddXmlCDataSection(XmlCDataSectionSyntax xmlCDataSectionSyntax);
}

public partial class XmlCDataSectionBuilder : IXmlCDataSectionBuilder
{
    public XmlCDataSectionSyntax Syntax { get; set; }

    public XmlCDataSectionBuilder(XmlCDataSectionSyntax syntax)
    {
        Syntax = syntax;
    }

    public static XmlCDataSectionSyntax CreateSyntax(Action<IXmlCDataSectionBuilder> xmlCDataSectionCallback)
    {
        var startCDataTokenToken = SyntaxFactory.Token(SyntaxKind.XmlCDataStartToken);
        var endCDataTokenToken = SyntaxFactory.Token(SyntaxKind.XmlCDataEndToken);
        var syntax = SyntaxFactory.XmlCDataSection(startCDataTokenToken, default(SyntaxTokenList), endCDataTokenToken);
        var builder = new XmlCDataSectionBuilder(syntax);
        xmlCDataSectionCallback(builder);
        return builder.Syntax;
    }

    public IXmlCDataSectionBuilder AddToken(SyntaxToken textToken)
    {
        Syntax = Syntax.AddTextTokens(textToken);
        return this;
    }
}