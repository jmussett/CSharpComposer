using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithSizeOfExpressionBuilder<TBuilder>
{
    TBuilder WithSizeOfExpression(Action<ITypeBuilder> typeCallback);
    TBuilder WithSizeOfExpression(SizeOfExpressionSyntax sizeOfExpressionSyntax);
}

public partial class SizeOfExpressionBuilder
{
    public static SizeOfExpressionSyntax CreateSyntax(Action<ITypeBuilder> typeCallback)
    {
        var keywordToken = SyntaxFactory.Token(SyntaxKind.SizeOfKeyword);
        var openParenTokenToken = SyntaxFactory.Token(SyntaxKind.OpenParenToken);
        var typeSyntax = TypeBuilder.CreateSyntax(typeCallback);
        var closeParenTokenToken = SyntaxFactory.Token(SyntaxKind.CloseParenToken);
        return SyntaxFactory.SizeOfExpression(keywordToken, openParenTokenToken, typeSyntax, closeParenTokenToken);
    }
}