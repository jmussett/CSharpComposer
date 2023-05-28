using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithIsPatternExpressionBuilder<TBuilder>
{
    TBuilder WithIsPatternExpression(Action<IExpressionBuilder> expressionCallback, Action<IPatternBuilder> patternCallback);
    TBuilder WithIsPatternExpression(IsPatternExpressionSyntax isPatternExpressionSyntax);
}

public partial class IsPatternExpressionBuilder
{
    public static IsPatternExpressionSyntax CreateSyntax(Action<IExpressionBuilder> expressionCallback, Action<IPatternBuilder> patternCallback)
    {
        var expressionSyntax = ExpressionBuilder.CreateSyntax(expressionCallback);
        var isKeywordToken = SyntaxFactory.Token(SyntaxKind.IsKeyword);
        var patternSyntax = PatternBuilder.CreateSyntax(patternCallback);
        return SyntaxFactory.IsPatternExpression(expressionSyntax, isKeywordToken, patternSyntax);
    }
}