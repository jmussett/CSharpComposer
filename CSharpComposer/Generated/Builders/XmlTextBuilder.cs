using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IXmlTextBuilder
{
}

public interface IWithXmlTextBuilder<TBuilder>
{
    TBuilder WithXmlText(Action<IXmlTextBuilder> xmlTextCallback);
    TBuilder WithXmlText(XmlTextSyntax xmlTextSyntax);
}

public partial class XmlTextBuilder : IXmlTextBuilder
{
    public XmlTextSyntax Syntax { get; set; }

    public XmlTextBuilder(XmlTextSyntax syntax)
    {
        Syntax = syntax;
    }

    public static XmlTextSyntax CreateSyntax(Action<IXmlTextBuilder> xmlTextCallback)
    {
        var syntax = SyntaxFactory.XmlText(default(SyntaxTokenList));
        var builder = new XmlTextBuilder(syntax);
        xmlTextCallback(builder);
        return builder.Syntax;
    }
}