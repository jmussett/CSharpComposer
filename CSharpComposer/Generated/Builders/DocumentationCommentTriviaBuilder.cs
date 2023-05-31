using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IDocumentationCommentTriviaBuilder
{
    IDocumentationCommentTriviaBuilder AddContentXmlNode(Action<IXmlNodeBuilder> contentCallback);
    IDocumentationCommentTriviaBuilder AddContentXmlNode(XmlNodeSyntax content);
}

public interface IWithDocumentationCommentTrivia<TBuilder>
{
    TBuilder WithDocumentationCommentTrivia(DocumentationCommentTriviaKind kind, Action<IDocumentationCommentTriviaBuilder> documentationCommentTriviaCallback);
    TBuilder WithDocumentationCommentTrivia(DocumentationCommentTriviaSyntax documentationCommentTriviaSyntax);
}

public interface IAddDocumentationCommentTrivia<TBuilder>
{
    TBuilder AddDocumentationCommentTrivia(DocumentationCommentTriviaKind kind, Action<IDocumentationCommentTriviaBuilder> documentationCommentTriviaCallback);
    TBuilder AddDocumentationCommentTrivia(DocumentationCommentTriviaSyntax documentationCommentTriviaSyntax);
}

public partial class DocumentationCommentTriviaBuilder : IDocumentationCommentTriviaBuilder
{
    public DocumentationCommentTriviaSyntax Syntax { get; set; }

    public DocumentationCommentTriviaBuilder(DocumentationCommentTriviaSyntax syntax)
    {
        Syntax = syntax;
    }

    public static DocumentationCommentTriviaSyntax CreateSyntax(DocumentationCommentTriviaKind kind, Action<IDocumentationCommentTriviaBuilder> documentationCommentTriviaCallback)
    {
        var syntaxKind = kind switch
        {
            DocumentationCommentTriviaKind.SingleLineDocumentationCommentTrivia => SyntaxKind.SingleLineDocumentationCommentTrivia,
            DocumentationCommentTriviaKind.MultiLineDocumentationCommentTrivia => SyntaxKind.MultiLineDocumentationCommentTrivia,
            _ => throw new NotSupportedException()
        };
        var endOfCommentToken = SyntaxFactory.Token(SyntaxKind.EndOfDocumentationCommentToken);
        var syntax = SyntaxFactory.DocumentationCommentTrivia(syntaxKind, default(SyntaxList<XmlNodeSyntax>), endOfCommentToken);
        var builder = new DocumentationCommentTriviaBuilder(syntax);
        documentationCommentTriviaCallback(builder);
        return builder.Syntax;
    }

    public IDocumentationCommentTriviaBuilder AddContentXmlNode(Action<IXmlNodeBuilder> contentCallback)
    {
        var content = XmlNodeBuilder.CreateSyntax(contentCallback);
        Syntax = Syntax.AddContent(content);
        return this;
    }

    public IDocumentationCommentTriviaBuilder AddContentXmlNode(XmlNodeSyntax content)
    {
        Syntax = Syntax.AddContent(content);
        return this;
    }
}