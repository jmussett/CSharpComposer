using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IDirectiveTriviaBuilder
{
    void AsIfDirectiveTrivia(Action<IExpressionBuilder> conditionCallback, bool isActive, bool branchTaken, bool conditionValue);
    void AsElifDirectiveTrivia(Action<IExpressionBuilder> conditionCallback, bool isActive, bool branchTaken, bool conditionValue);
    void AsElseDirectiveTrivia(bool isActive, bool branchTaken);
    void AsEndIfDirectiveTrivia(bool isActive);
    void AsRegionDirectiveTrivia(bool isActive);
    void AsEndRegionDirectiveTrivia(bool isActive);
    void AsErrorDirectiveTrivia(bool isActive);
    void AsWarningDirectiveTrivia(bool isActive);
    void AsBadDirectiveTrivia(string identifier, bool isActive);
    void AsDefineDirectiveTrivia(string name, bool isActive);
    void AsUndefDirectiveTrivia(string name, bool isActive);
    void AsLineDirectiveTrivia(LineDirectiveTriviaLine lineDirectiveTriviaLine, bool isActive, Action<ILineDirectiveTriviaBuilder> lineDirectiveTriviaCallback);
    void AsLineSpanDirectiveTrivia(int startLine, int startCharacter, int endLine, int endCharacter, string file, bool isActive, Action<ILineSpanDirectiveTriviaBuilder> lineSpanDirectiveTriviaCallback);
    void AsPragmaWarningDirectiveTrivia(PragmaWarningDirectiveTriviaDisableOrRestoreKeyword pragmaWarningDirectiveTriviaDisableOrRestoreKeyword, bool isActive, Action<IPragmaWarningDirectiveTriviaBuilder> pragmaWarningDirectiveTriviaCallback);
    void AsPragmaChecksumDirectiveTrivia(string file, string guid, string bytes, bool isActive);
    void AsReferenceDirectiveTrivia(string file, bool isActive);
    void AsLoadDirectiveTrivia(string file, bool isActive);
    void AsShebangDirectiveTrivia(bool isActive);
    void AsNullableDirectiveTrivia(NullableDirectiveTriviaSettingToken nullableDirectiveTriviaSettingToken, bool isActive, Action<INullableDirectiveTriviaBuilder> nullableDirectiveTriviaCallback);
}

public interface IWithDirectiveTriviaBuilder<TBuilder>
{
    TBuilder WithDirectiveTrivia(Action<IDirectiveTriviaBuilder> directiveTriviaCallback);
    TBuilder WithDirectiveTrivia(DirectiveTriviaSyntax directiveTriviaSyntax);
}

public partial class DirectiveTriviaBuilder : IDirectiveTriviaBuilder
{
    public DirectiveTriviaSyntax? Syntax { get; set; }

    public static DirectiveTriviaSyntax CreateSyntax(Action<IDirectiveTriviaBuilder> callback)
    {
        var builder = new DirectiveTriviaBuilder();
        callback(builder);
        if (builder.Syntax is null)
        {
            throw new InvalidOperationException("DirectiveTriviaSyntax has not been specified");
        }

        return builder.Syntax;
    }

    public void AsIfDirectiveTrivia(Action<IExpressionBuilder> conditionCallback, bool isActive, bool branchTaken, bool conditionValue)
    {
        Syntax = IfDirectiveTriviaBuilder.CreateSyntax(conditionCallback, isActive, branchTaken, conditionValue);
    }

    public void AsElifDirectiveTrivia(Action<IExpressionBuilder> conditionCallback, bool isActive, bool branchTaken, bool conditionValue)
    {
        Syntax = ElifDirectiveTriviaBuilder.CreateSyntax(conditionCallback, isActive, branchTaken, conditionValue);
    }

    public void AsElseDirectiveTrivia(bool isActive, bool branchTaken)
    {
        Syntax = ElseDirectiveTriviaBuilder.CreateSyntax(isActive, branchTaken);
    }

    public void AsEndIfDirectiveTrivia(bool isActive)
    {
        Syntax = EndIfDirectiveTriviaBuilder.CreateSyntax(isActive);
    }

    public void AsRegionDirectiveTrivia(bool isActive)
    {
        Syntax = RegionDirectiveTriviaBuilder.CreateSyntax(isActive);
    }

    public void AsEndRegionDirectiveTrivia(bool isActive)
    {
        Syntax = EndRegionDirectiveTriviaBuilder.CreateSyntax(isActive);
    }

    public void AsErrorDirectiveTrivia(bool isActive)
    {
        Syntax = ErrorDirectiveTriviaBuilder.CreateSyntax(isActive);
    }

    public void AsWarningDirectiveTrivia(bool isActive)
    {
        Syntax = WarningDirectiveTriviaBuilder.CreateSyntax(isActive);
    }

    public void AsBadDirectiveTrivia(string identifier, bool isActive)
    {
        Syntax = BadDirectiveTriviaBuilder.CreateSyntax(identifier, isActive);
    }

    public void AsDefineDirectiveTrivia(string name, bool isActive)
    {
        Syntax = DefineDirectiveTriviaBuilder.CreateSyntax(name, isActive);
    }

    public void AsUndefDirectiveTrivia(string name, bool isActive)
    {
        Syntax = UndefDirectiveTriviaBuilder.CreateSyntax(name, isActive);
    }

    public void AsLineDirectiveTrivia(LineDirectiveTriviaLine lineDirectiveTriviaLine, bool isActive, Action<ILineDirectiveTriviaBuilder> lineDirectiveTriviaCallback)
    {
        Syntax = LineDirectiveTriviaBuilder.CreateSyntax(lineDirectiveTriviaLine, isActive, lineDirectiveTriviaCallback);
    }

    public void AsLineSpanDirectiveTrivia(int startLine, int startCharacter, int endLine, int endCharacter, string file, bool isActive, Action<ILineSpanDirectiveTriviaBuilder> lineSpanDirectiveTriviaCallback)
    {
        Syntax = LineSpanDirectiveTriviaBuilder.CreateSyntax(startLine, startCharacter, endLine, endCharacter, file, isActive, lineSpanDirectiveTriviaCallback);
    }

    public void AsPragmaWarningDirectiveTrivia(PragmaWarningDirectiveTriviaDisableOrRestoreKeyword pragmaWarningDirectiveTriviaDisableOrRestoreKeyword, bool isActive, Action<IPragmaWarningDirectiveTriviaBuilder> pragmaWarningDirectiveTriviaCallback)
    {
        Syntax = PragmaWarningDirectiveTriviaBuilder.CreateSyntax(pragmaWarningDirectiveTriviaDisableOrRestoreKeyword, isActive, pragmaWarningDirectiveTriviaCallback);
    }

    public void AsPragmaChecksumDirectiveTrivia(string file, string guid, string bytes, bool isActive)
    {
        Syntax = PragmaChecksumDirectiveTriviaBuilder.CreateSyntax(file, guid, bytes, isActive);
    }

    public void AsReferenceDirectiveTrivia(string file, bool isActive)
    {
        Syntax = ReferenceDirectiveTriviaBuilder.CreateSyntax(file, isActive);
    }

    public void AsLoadDirectiveTrivia(string file, bool isActive)
    {
        Syntax = LoadDirectiveTriviaBuilder.CreateSyntax(file, isActive);
    }

    public void AsShebangDirectiveTrivia(bool isActive)
    {
        Syntax = ShebangDirectiveTriviaBuilder.CreateSyntax(isActive);
    }

    public void AsNullableDirectiveTrivia(NullableDirectiveTriviaSettingToken nullableDirectiveTriviaSettingToken, bool isActive, Action<INullableDirectiveTriviaBuilder> nullableDirectiveTriviaCallback)
    {
        Syntax = NullableDirectiveTriviaBuilder.CreateSyntax(nullableDirectiveTriviaSettingToken, isActive, nullableDirectiveTriviaCallback);
    }
}