using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface ILineDirectiveTriviaBuilder : ILineOrSpanDirectiveTriviaBuilder<ILineDirectiveTriviaBuilder>
{
    ILineDirectiveTriviaBuilder WithFile();
}

public interface IWithLineDirectiveTriviaBuilder<TBuilder>
{
    TBuilder WithLineDirectiveTrivia(LineDirectiveTriviaLine lineDirectiveTriviaLine, bool isActive, Action<ILineDirectiveTriviaBuilder> lineDirectiveTriviaCallback);
    TBuilder WithLineDirectiveTrivia(LineDirectiveTriviaSyntax lineDirectiveTriviaSyntax);
}

public partial class LineDirectiveTriviaBuilder : ILineDirectiveTriviaBuilder
{
    public LineDirectiveTriviaSyntax Syntax { get; set; }

    public LineDirectiveTriviaBuilder(LineDirectiveTriviaSyntax syntax)
    {
        Syntax = syntax;
    }

    public static LineDirectiveTriviaSyntax CreateSyntax(LineDirectiveTriviaLine lineDirectiveTriviaLine, bool isActive, Action<ILineDirectiveTriviaBuilder> lineDirectiveTriviaCallback)
    {
        var hashTokenToken = SyntaxFactory.Token(SyntaxKind.HashToken);
        var lineKeywordToken = SyntaxFactory.Token(SyntaxKind.LineKeyword);
        var lineToken = lineDirectiveTriviaLine switch
        {
            LineDirectiveTriviaLine.NumericLiteralToken => SyntaxFactory.Token(SyntaxKind.NumericLiteralToken),
            LineDirectiveTriviaLine.DefaultKeyword => SyntaxFactory.Token(SyntaxKind.DefaultKeyword),
            LineDirectiveTriviaLine.HiddenKeyword => SyntaxFactory.Token(SyntaxKind.HiddenKeyword),
            _ => throw new NotSupportedException()
        };
        var endOfDirectiveTokenToken = SyntaxFactory.Token(SyntaxKind.EndOfDirectiveToken);
        var syntax = SyntaxFactory.LineDirectiveTrivia(hashTokenToken, lineKeywordToken, lineToken, default(SyntaxToken), endOfDirectiveTokenToken, isActive);
        var builder = new LineDirectiveTriviaBuilder(syntax);
        lineDirectiveTriviaCallback(builder);
        return builder.Syntax;
    }

    public ILineDirectiveTriviaBuilder WithFile()
    {
        Syntax = Syntax.WithFile(SyntaxFactory.Token(SyntaxKind.StringLiteralToken));
        return this;
    }
}