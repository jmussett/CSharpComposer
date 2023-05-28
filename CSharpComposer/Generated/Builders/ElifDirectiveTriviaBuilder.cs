using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithElifDirectiveTriviaBuilder<TBuilder>
{
    TBuilder WithElifDirectiveTrivia(Action<IExpressionBuilder> conditionCallback, bool isActive, bool branchTaken, bool conditionValue);
    TBuilder WithElifDirectiveTrivia(ElifDirectiveTriviaSyntax elifDirectiveTriviaSyntax);
}

public partial class ElifDirectiveTriviaBuilder
{
    public static ElifDirectiveTriviaSyntax CreateSyntax(Action<IExpressionBuilder> conditionCallback, bool isActive, bool branchTaken, bool conditionValue)
    {
        var hashTokenToken = SyntaxFactory.Token(SyntaxKind.HashToken);
        var elifKeywordToken = SyntaxFactory.Token(SyntaxKind.ElifKeyword);
        var conditionSyntax = ExpressionBuilder.CreateSyntax(conditionCallback);
        var endOfDirectiveTokenToken = SyntaxFactory.Token(SyntaxKind.EndOfDirectiveToken);
        return SyntaxFactory.ElifDirectiveTrivia(hashTokenToken, elifKeywordToken, conditionSyntax, endOfDirectiveTokenToken, isActive, branchTaken, conditionValue);
    }
}