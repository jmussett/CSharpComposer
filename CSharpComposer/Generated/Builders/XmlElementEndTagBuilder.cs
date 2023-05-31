using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithXmlElementEndTag<TBuilder>
{
    TBuilder WithXmlElementEndTag(string nameLocalName, Action<IXmlNameBuilder> nameXmlNameCallback);
    TBuilder WithXmlElementEndTag(XmlElementEndTagSyntax xmlElementEndTagSyntax);
}

public interface IAddXmlElementEndTag<TBuilder>
{
    TBuilder AddXmlElementEndTag(string nameLocalName, Action<IXmlNameBuilder> nameXmlNameCallback);
    TBuilder AddXmlElementEndTag(XmlElementEndTagSyntax xmlElementEndTagSyntax);
}

public partial class XmlElementEndTagBuilder
{
    public static XmlElementEndTagSyntax CreateSyntax(string nameLocalName, Action<IXmlNameBuilder> nameXmlNameCallback)
    {
        var lessThanSlashTokenToken = SyntaxFactory.Token(SyntaxKind.LessThanSlashToken);
        var nameSyntax = XmlNameBuilder.CreateSyntax(nameLocalName, nameXmlNameCallback);
        var greaterThanTokenToken = SyntaxFactory.Token(SyntaxKind.GreaterThanToken);
        return SyntaxFactory.XmlElementEndTag(lessThanSlashTokenToken, nameSyntax, greaterThanTokenToken);
    }
}