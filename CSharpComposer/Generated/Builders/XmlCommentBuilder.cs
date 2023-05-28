using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IXmlCommentBuilder
{
}

public interface IWithXmlCommentBuilder<TBuilder>
{
    TBuilder WithXmlComment(Action<IXmlCommentBuilder> xmlCommentCallback);
    TBuilder WithXmlComment(XmlCommentSyntax xmlCommentSyntax);
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
}