using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithDefaultExpressionBuilder<TBuilder>
{
    TBuilder WithDefaultExpression(Action<ITypeBuilder> typeCallback);
    TBuilder WithDefaultExpression(DefaultExpressionSyntax defaultExpressionSyntax);
}

public partial class DefaultExpressionBuilder
{
    public static DefaultExpressionSyntax CreateSyntax(Action<ITypeBuilder> typeCallback)
    {
        var keywordToken = SyntaxFactory.Token(SyntaxKind.DefaultKeyword);
        var openParenTokenToken = SyntaxFactory.Token(SyntaxKind.OpenParenToken);
        var typeSyntax = TypeBuilder.CreateSyntax(typeCallback);
        var closeParenTokenToken = SyntaxFactory.Token(SyntaxKind.CloseParenToken);
        return SyntaxFactory.DefaultExpression(keywordToken, openParenTokenToken, typeSyntax, closeParenTokenToken);
    }
}