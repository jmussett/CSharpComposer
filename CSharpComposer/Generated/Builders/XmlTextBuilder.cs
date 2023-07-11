using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IXmlTextBuilder
{
    IXmlTextBuilder AddToken(SyntaxToken textToken);
}

public partial class XmlTextBuilder : IXmlTextBuilder
{
    public XmlTextSyntax Syntax { get; set; }

    public XmlTextBuilder(XmlTextSyntax syntax)
    {
        Syntax = syntax;
    }

    public static XmlTextSyntax CreateSyntax(Action<IXmlTextBuilder>? xmlTextCallback = null)
    {
        var syntax = SyntaxFactory.XmlText(default(SyntaxTokenList));
        var builder = new XmlTextBuilder(syntax);
        xmlTextCallback?.Invoke(builder);
        return builder.Syntax;
    }

    public IXmlTextBuilder AddToken(SyntaxToken textToken)
    {
        Syntax = Syntax.AddTextTokens(textToken);
        return this;
    }
}