using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IXmlTextBuilder
{
    IXmlTextBuilder AddToken(SyntaxKind textToken);
}

internal partial class XmlTextBuilder : IXmlTextBuilder
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

    public IXmlTextBuilder AddToken(SyntaxKind textToken)
    {
        Syntax = Syntax.AddTextTokens(SyntaxFactory.Token(textToken));
        return this;
    }
}