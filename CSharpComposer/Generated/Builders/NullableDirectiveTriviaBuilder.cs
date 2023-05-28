using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface INullableDirectiveTriviaBuilder
{
    INullableDirectiveTriviaBuilder WithTargetToken(TargetToken targetToken);
}

public interface IWithNullableDirectiveTriviaBuilder<TBuilder>
{
    TBuilder WithNullableDirectiveTrivia(NullableDirectiveTriviaSettingToken nullableDirectiveTriviaSettingToken, bool isActive, Action<INullableDirectiveTriviaBuilder> nullableDirectiveTriviaCallback);
    TBuilder WithNullableDirectiveTrivia(NullableDirectiveTriviaSyntax nullableDirectiveTriviaSyntax);
}

public partial class NullableDirectiveTriviaBuilder : INullableDirectiveTriviaBuilder
{
    public NullableDirectiveTriviaSyntax Syntax { get; set; }

    public NullableDirectiveTriviaBuilder(NullableDirectiveTriviaSyntax syntax)
    {
        Syntax = syntax;
    }

    public static NullableDirectiveTriviaSyntax CreateSyntax(NullableDirectiveTriviaSettingToken nullableDirectiveTriviaSettingToken, bool isActive, Action<INullableDirectiveTriviaBuilder> nullableDirectiveTriviaCallback)
    {
        var hashTokenToken = SyntaxFactory.Token(SyntaxKind.HashToken);
        var nullableKeywordToken = SyntaxFactory.Token(SyntaxKind.NullableKeyword);
        var settingTokenToken = nullableDirectiveTriviaSettingToken switch
        {
            NullableDirectiveTriviaSettingToken.EnableKeyword => SyntaxFactory.Token(SyntaxKind.EnableKeyword),
            NullableDirectiveTriviaSettingToken.DisableKeyword => SyntaxFactory.Token(SyntaxKind.DisableKeyword),
            NullableDirectiveTriviaSettingToken.RestoreKeyword => SyntaxFactory.Token(SyntaxKind.RestoreKeyword),
            _ => throw new NotSupportedException()
        };
        var endOfDirectiveTokenToken = SyntaxFactory.Token(SyntaxKind.EndOfDirectiveToken);
        var syntax = SyntaxFactory.NullableDirectiveTrivia(hashTokenToken, nullableKeywordToken, settingTokenToken, default(SyntaxToken), endOfDirectiveTokenToken, isActive);
        var builder = new NullableDirectiveTriviaBuilder(syntax);
        nullableDirectiveTriviaCallback(builder);
        return builder.Syntax;
    }

    public INullableDirectiveTriviaBuilder WithTargetToken(TargetToken targetToken)
    {
        Syntax = Syntax.WithTargetToken(SyntaxFactory.Token(targetToken switch
        {
            TargetToken.WarningsKeyword => SyntaxKind.WarningsKeyword,
            TargetToken.AnnotationsKeyword => SyntaxKind.AnnotationsKeyword,
            _ => throw new NotSupportedException()
        }));
        return this;
    }
}