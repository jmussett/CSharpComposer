using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithUnaryPatternBuilder<TBuilder>
{
    TBuilder WithUnaryPattern(Action<IPatternBuilder> patternCallback);
    TBuilder WithUnaryPattern(UnaryPatternSyntax unaryPatternSyntax);
}

public partial class UnaryPatternBuilder
{
    public static UnaryPatternSyntax CreateSyntax(Action<IPatternBuilder> patternCallback)
    {
        var operatorTokenToken = SyntaxFactory.Token(SyntaxKind.NotKeyword);
        var patternSyntax = PatternBuilder.CreateSyntax(patternCallback);
        return SyntaxFactory.UnaryPattern(operatorTokenToken, patternSyntax);
    }
}