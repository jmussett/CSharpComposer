using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithRelationalPatternBuilder<TBuilder>
{
    TBuilder WithRelationalPattern(RelationalPatternOperatorToken relationalPatternOperatorToken, Action<IExpressionBuilder> expressionCallback);
    TBuilder WithRelationalPattern(RelationalPatternSyntax relationalPatternSyntax);
}

public partial class RelationalPatternBuilder
{
    public static RelationalPatternSyntax CreateSyntax(RelationalPatternOperatorToken relationalPatternOperatorToken, Action<IExpressionBuilder> expressionCallback)
    {
        var operatorTokenToken = relationalPatternOperatorToken switch
        {
            RelationalPatternOperatorToken.EqualsEqualsToken => SyntaxFactory.Token(SyntaxKind.EqualsEqualsToken),
            RelationalPatternOperatorToken.ExclamationEqualsToken => SyntaxFactory.Token(SyntaxKind.ExclamationEqualsToken),
            RelationalPatternOperatorToken.LessThanToken => SyntaxFactory.Token(SyntaxKind.LessThanToken),
            RelationalPatternOperatorToken.LessThanEqualsToken => SyntaxFactory.Token(SyntaxKind.LessThanEqualsToken),
            RelationalPatternOperatorToken.GreaterThanToken => SyntaxFactory.Token(SyntaxKind.GreaterThanToken),
            RelationalPatternOperatorToken.GreaterThanEqualsToken => SyntaxFactory.Token(SyntaxKind.GreaterThanEqualsToken),
            _ => throw new NotSupportedException()
        };
        var expressionSyntax = ExpressionBuilder.CreateSyntax(expressionCallback);
        return SyntaxFactory.RelationalPattern(operatorTokenToken, expressionSyntax);
    }
}