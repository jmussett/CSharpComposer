using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IXmlCDataSectionBuilder
{
    IXmlCDataSectionBuilder AddToken(SyntaxKind textToken);
}

public partial class XmlCDataSectionBuilder : IXmlCDataSectionBuilder
{
    public XmlCDataSectionSyntax Syntax { get; set; }

    public XmlCDataSectionBuilder(XmlCDataSectionSyntax syntax)
    {
        Syntax = syntax;
    }

    public static XmlCDataSectionSyntax CreateSyntax(Action<IXmlCDataSectionBuilder>? xmlCDataSectionCallback = null)
    {
        var startCDataTokenToken = SyntaxFactory.Token(SyntaxKind.XmlCDataStartToken);
        var endCDataTokenToken = SyntaxFactory.Token(SyntaxKind.XmlCDataEndToken);
        var syntax = SyntaxFactory.XmlCDataSection(startCDataTokenToken, default(SyntaxTokenList), endCDataTokenToken);
        var builder = new XmlCDataSectionBuilder(syntax);
        xmlCDataSectionCallback?.Invoke(builder);
        return builder.Syntax;
    }

    public IXmlCDataSectionBuilder AddToken(SyntaxKind textToken)
    {
        Syntax = Syntax.AddTextTokens(SyntaxFactory.Token(textToken));
        return this;
    }
}