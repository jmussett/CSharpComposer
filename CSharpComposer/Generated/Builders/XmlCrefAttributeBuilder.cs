using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithXmlCrefAttributeBuilder<TBuilder>
{
    TBuilder WithXmlCrefAttribute(string nameLocalName, Action<IXmlNameBuilder> nameXmlNameCallback, XmlCrefAttributeStartQuoteToken xmlCrefAttributeStartQuoteToken, Action<ICrefBuilder> crefCallback, XmlCrefAttributeEndQuoteToken xmlCrefAttributeEndQuoteToken);
    TBuilder WithXmlCrefAttribute(XmlCrefAttributeSyntax xmlCrefAttributeSyntax);
}

public partial class XmlCrefAttributeBuilder
{
    public static XmlCrefAttributeSyntax CreateSyntax(string nameLocalName, Action<IXmlNameBuilder> nameXmlNameCallback, XmlCrefAttributeStartQuoteToken xmlCrefAttributeStartQuoteToken, Action<ICrefBuilder> crefCallback, XmlCrefAttributeEndQuoteToken xmlCrefAttributeEndQuoteToken)
    {
        var nameSyntax = XmlNameBuilder.CreateSyntax(nameLocalName, nameXmlNameCallback);
        var equalsTokenToken = SyntaxFactory.Token(SyntaxKind.EqualsToken);
        var startQuoteTokenToken = xmlCrefAttributeStartQuoteToken switch
        {
            XmlCrefAttributeStartQuoteToken.SingleQuoteToken => SyntaxFactory.Token(SyntaxKind.SingleQuoteToken),
            XmlCrefAttributeStartQuoteToken.DoubleQuoteToken => SyntaxFactory.Token(SyntaxKind.DoubleQuoteToken),
            _ => throw new NotSupportedException()
        };
        var crefSyntax = CrefBuilder.CreateSyntax(crefCallback);
        var endQuoteTokenToken = xmlCrefAttributeEndQuoteToken switch
        {
            XmlCrefAttributeEndQuoteToken.SingleQuoteToken => SyntaxFactory.Token(SyntaxKind.SingleQuoteToken),
            XmlCrefAttributeEndQuoteToken.DoubleQuoteToken => SyntaxFactory.Token(SyntaxKind.DoubleQuoteToken),
            _ => throw new NotSupportedException()
        };
        return SyntaxFactory.XmlCrefAttribute(nameSyntax, equalsTokenToken, startQuoteTokenToken, crefSyntax, endQuoteTokenToken);
    }
}