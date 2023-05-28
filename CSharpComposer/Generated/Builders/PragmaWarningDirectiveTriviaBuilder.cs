using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IPragmaWarningDirectiveTriviaBuilder
{
    IPragmaWarningDirectiveTriviaBuilder AddErrorCode(Action<IExpressionBuilder> errorCodeCallback);
    IPragmaWarningDirectiveTriviaBuilder AddErrorCode(ExpressionSyntax errorCode);
}

public interface IWithPragmaWarningDirectiveTriviaBuilder<TBuilder>
{
    TBuilder WithPragmaWarningDirectiveTrivia(PragmaWarningDirectiveTriviaDisableOrRestoreKeyword pragmaWarningDirectiveTriviaDisableOrRestoreKeyword, bool isActive, Action<IPragmaWarningDirectiveTriviaBuilder> pragmaWarningDirectiveTriviaCallback);
    TBuilder WithPragmaWarningDirectiveTrivia(PragmaWarningDirectiveTriviaSyntax pragmaWarningDirectiveTriviaSyntax);
}

public partial class PragmaWarningDirectiveTriviaBuilder : IPragmaWarningDirectiveTriviaBuilder
{
    public PragmaWarningDirectiveTriviaSyntax Syntax { get; set; }

    public PragmaWarningDirectiveTriviaBuilder(PragmaWarningDirectiveTriviaSyntax syntax)
    {
        Syntax = syntax;
    }

    public static PragmaWarningDirectiveTriviaSyntax CreateSyntax(PragmaWarningDirectiveTriviaDisableOrRestoreKeyword pragmaWarningDirectiveTriviaDisableOrRestoreKeyword, bool isActive, Action<IPragmaWarningDirectiveTriviaBuilder> pragmaWarningDirectiveTriviaCallback)
    {
        var hashTokenToken = SyntaxFactory.Token(SyntaxKind.HashToken);
        var pragmaKeywordToken = SyntaxFactory.Token(SyntaxKind.PragmaKeyword);
        var warningKeywordToken = SyntaxFactory.Token(SyntaxKind.WarningKeyword);
        var disableOrRestoreKeywordToken = pragmaWarningDirectiveTriviaDisableOrRestoreKeyword switch
        {
            PragmaWarningDirectiveTriviaDisableOrRestoreKeyword.DisableKeyword => SyntaxFactory.Token(SyntaxKind.DisableKeyword),
            PragmaWarningDirectiveTriviaDisableOrRestoreKeyword.RestoreKeyword => SyntaxFactory.Token(SyntaxKind.RestoreKeyword),
            _ => throw new NotSupportedException()
        };
        var endOfDirectiveTokenToken = SyntaxFactory.Token(SyntaxKind.EndOfDirectiveToken);
        var syntax = SyntaxFactory.PragmaWarningDirectiveTrivia(hashTokenToken, pragmaKeywordToken, warningKeywordToken, disableOrRestoreKeywordToken, default(SeparatedSyntaxList<ExpressionSyntax>), endOfDirectiveTokenToken, isActive);
        var builder = new PragmaWarningDirectiveTriviaBuilder(syntax);
        pragmaWarningDirectiveTriviaCallback(builder);
        return builder.Syntax;
    }

    public IPragmaWarningDirectiveTriviaBuilder AddErrorCode(Action<IExpressionBuilder> errorCodeCallback)
    {
        var errorCode = ExpressionBuilder.CreateSyntax(errorCodeCallback);
        Syntax = Syntax.AddErrorCodes(errorCode);
        return this;
    }

    public IPragmaWarningDirectiveTriviaBuilder AddErrorCode(ExpressionSyntax errorCode)
    {
        Syntax = Syntax.AddErrorCodes(errorCode);
        return this;
    }
}