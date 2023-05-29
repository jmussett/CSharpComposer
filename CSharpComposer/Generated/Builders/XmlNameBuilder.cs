using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IXmlNameBuilder : IWithXmlPrefixBuilder<IXmlNameBuilder>
{
    IXmlNameBuilder WithXmlPrefix(string prefix);
    IXmlNameBuilder WithXmlPrefix(XmlPrefixSyntax prefix);
}

public interface IWithXmlNameBuilder<TBuilder>
{
    TBuilder WithXmlName(string localName, Action<IXmlNameBuilder> xmlNameCallback);
    TBuilder WithXmlName(XmlNameSyntax xmlNameSyntax);
}

public partial class XmlNameBuilder : IXmlNameBuilder
{
    public XmlNameSyntax Syntax { get; set; }

    public XmlNameBuilder(XmlNameSyntax syntax)
    {
        Syntax = syntax;
    }

    public static XmlNameSyntax CreateSyntax(string localName, Action<IXmlNameBuilder> xmlNameCallback)
    {
        var localNameToken = SyntaxFactory.Identifier(localName);
        var syntax = SyntaxFactory.XmlName(default(XmlPrefixSyntax), localNameToken);
        var builder = new XmlNameBuilder(syntax);
        xmlNameCallback(builder);
        return builder.Syntax;
    }

    public IXmlNameBuilder WithXmlPrefix(string prefix)
    {
        var prefixSyntax = XmlPrefixBuilder.CreateSyntax(prefix);
        Syntax = Syntax.WithPrefix(prefixSyntax);
        return this;
    }

    public IXmlNameBuilder WithXmlPrefix(XmlPrefixSyntax prefix)
    {
        Syntax = Syntax.WithPrefix(prefix);
        return this;
    }
}