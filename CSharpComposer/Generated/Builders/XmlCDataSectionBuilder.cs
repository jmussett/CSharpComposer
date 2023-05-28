using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IXmlCDataSectionBuilder
{
}

public interface IWithXmlCDataSectionBuilder<TBuilder>
{
    TBuilder WithXmlCDataSection(Action<IXmlCDataSectionBuilder> xmlCDataSectionCallback);
    TBuilder WithXmlCDataSection(XmlCDataSectionSyntax xmlCDataSectionSyntax);
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
}