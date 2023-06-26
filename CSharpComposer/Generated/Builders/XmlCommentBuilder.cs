using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IXmlCommentBuilder
{
    IXmlCommentBuilder AddToken(SyntaxToken textToken);
}

public partial class XmlCommentBuilder : IXmlCommentBuilder
{
    public XmlCommentSyntax Syntax { get; set; }

    public XmlCommentBuilder(XmlCommentSyntax syntax)
    {
        Syntax = syntax;
    }

    public static XmlCommentSyntax CreateSyntax(Action<IXmlCommentBuilder> xmlCommentCallback)
    {
        var lessThanExclamationMinusMinusTokenToken = SyntaxFactory.Token(SyntaxKind.XmlCommentStartToken);
        var minusMinusGreaterThanTokenToken = SyntaxFactory.Token(SyntaxKind.XmlCommentEndToken);
        var syntax = SyntaxFactory.XmlComment(lessThanExclamationMinusMinusTokenToken, default(SyntaxTokenList), minusMinusGreaterThanTokenToken);
        var builder = new XmlCommentBuilder(syntax);
        xmlCommentCallback(builder);
        return builder.Syntax;
    }

    public IXmlCommentBuilder AddToken(SyntaxToken textToken)
    {
        Syntax = Syntax.AddTextTokens(textToken);
        return this;
    }
}