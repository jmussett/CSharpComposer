using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithXmlNameAttributeBuilder<TBuilder>
{
    TBuilder WithXmlNameAttribute(string nameLocalName, Action<IXmlNameBuilder> nameXmlNameCallback, XmlNameAttributeStartQuoteToken xmlNameAttributeStartQuoteToken, string identifierIdentifier, XmlNameAttributeEndQuoteToken xmlNameAttributeEndQuoteToken);
    TBuilder WithXmlNameAttribute(XmlNameAttributeSyntax xmlNameAttributeSyntax);
}

public partial class XmlNameAttributeBuilder
{
    public static XmlNameAttributeSyntax CreateSyntax(string nameLocalName, Action<IXmlNameBuilder> nameXmlNameCallback, XmlNameAttributeStartQuoteToken xmlNameAttributeStartQuoteToken, string identifierIdentifier, XmlNameAttributeEndQuoteToken xmlNameAttributeEndQuoteToken)
    {
        var nameSyntax = XmlNameBuilder.CreateSyntax(nameLocalName, nameXmlNameCallback);
        var equalsTokenToken = SyntaxFactory.Token(SyntaxKind.EqualsToken);
        var startQuoteTokenToken = xmlNameAttributeStartQuoteToken switch
        {
            XmlNameAttributeStartQuoteToken.SingleQuoteToken => SyntaxFactory.Token(SyntaxKind.SingleQuoteToken),
            XmlNameAttributeStartQuoteToken.DoubleQuoteToken => SyntaxFactory.Token(SyntaxKind.DoubleQuoteToken),
            _ => throw new NotSupportedException()
        };
        var identifierSyntax = IdentifierNameBuilder.CreateSyntax(identifierIdentifier);
        var endQuoteTokenToken = xmlNameAttributeEndQuoteToken switch
        {
            XmlNameAttributeEndQuoteToken.SingleQuoteToken => SyntaxFactory.Token(SyntaxKind.SingleQuoteToken),
            XmlNameAttributeEndQuoteToken.DoubleQuoteToken => SyntaxFactory.Token(SyntaxKind.DoubleQuoteToken),
            _ => throw new NotSupportedException()
        };
        return SyntaxFactory.XmlNameAttribute(nameSyntax, equalsTokenToken, startQuoteTokenToken, identifierSyntax, endQuoteTokenToken);
    }
}