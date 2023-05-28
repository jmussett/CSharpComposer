using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface ILineSpanDirectiveTriviaBuilder : ILineOrSpanDirectiveTriviaBuilder<ILineSpanDirectiveTriviaBuilder>
{
    ILineSpanDirectiveTriviaBuilder WithCharacterOffset();
}

public interface IWithLineSpanDirectiveTriviaBuilder<TBuilder>
{
    TBuilder WithLineSpanDirectiveTrivia(int startLine, int startCharacter, int endLine, int endCharacter, string file, bool isActive, Action<ILineSpanDirectiveTriviaBuilder> lineSpanDirectiveTriviaCallback);
    TBuilder WithLineSpanDirectiveTrivia(LineSpanDirectiveTriviaSyntax lineSpanDirectiveTriviaSyntax);
}

public partial class LineSpanDirectiveTriviaBuilder : ILineSpanDirectiveTriviaBuilder
{
    public LineSpanDirectiveTriviaSyntax Syntax { get; set; }

    public LineSpanDirectiveTriviaBuilder(LineSpanDirectiveTriviaSyntax syntax)
    {
        Syntax = syntax;
    }

    public static LineSpanDirectiveTriviaSyntax CreateSyntax(int startLine, int startCharacter, int endLine, int endCharacter, string file, bool isActive, Action<ILineSpanDirectiveTriviaBuilder> lineSpanDirectiveTriviaCallback)
    {
        var hashTokenToken = SyntaxFactory.Token(SyntaxKind.HashToken);
        var lineKeywordToken = SyntaxFactory.Token(SyntaxKind.LineKeyword);
        var startSyntax = LineDirectivePositionBuilder.CreateSyntax(startLine, startCharacter);
        var minusTokenToken = SyntaxFactory.Token(SyntaxKind.MinusToken);
        var endSyntax = LineDirectivePositionBuilder.CreateSyntax(endLine, endCharacter);
        var fileToken = SyntaxFactory.Literal(file);
        var endOfDirectiveTokenToken = SyntaxFactory.Token(SyntaxKind.EndOfDirectiveToken);
        var syntax = SyntaxFactory.LineSpanDirectiveTrivia(hashTokenToken, lineKeywordToken, startSyntax, minusTokenToken, endSyntax, default(SyntaxToken), fileToken, endOfDirectiveTokenToken, isActive);
        var builder = new LineSpanDirectiveTriviaBuilder(syntax);
        lineSpanDirectiveTriviaCallback(builder);
        return builder.Syntax;
    }

    public ILineSpanDirectiveTriviaBuilder WithCharacterOffset()
    {
        Syntax = Syntax.WithCharacterOffset(SyntaxFactory.Token(SyntaxKind.NumericLiteralToken));
        return this;
    }
}