using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithBinaryPatternBuilder<TBuilder>
{
    TBuilder WithBinaryPattern(BinaryPatternKind kind, Action<IPatternBuilder> leftCallback, Action<IPatternBuilder> rightCallback);
    TBuilder WithBinaryPattern(BinaryPatternSyntax binaryPatternSyntax);
}

public partial class BinaryPatternBuilder
{
    public static BinaryPatternSyntax CreateSyntax(BinaryPatternKind kind, Action<IPatternBuilder> leftCallback, Action<IPatternBuilder> rightCallback)
    {
        var syntaxKind = kind switch
        {
            BinaryPatternKind.OrPattern => SyntaxKind.OrPattern,
            BinaryPatternKind.AndPattern => SyntaxKind.AndPattern,
            _ => throw new NotSupportedException()
        };
        var leftSyntax = PatternBuilder.CreateSyntax(leftCallback);
        var operatorTokenToken = kind switch
        {
            BinaryPatternKind.OrPattern => SyntaxFactory.Token(SyntaxKind.OrKeyword),
            BinaryPatternKind.AndPattern => SyntaxFactory.Token(SyntaxKind.AndKeyword),
            _ => throw new NotSupportedException()
        };
        var rightSyntax = PatternBuilder.CreateSyntax(rightCallback);
        return SyntaxFactory.BinaryPattern(syntaxKind, leftSyntax, operatorTokenToken, rightSyntax);
    }
}