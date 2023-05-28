using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithTypeOfExpressionBuilder<TBuilder>
{
    TBuilder WithTypeOfExpression(Action<ITypeBuilder> typeCallback);
    TBuilder WithTypeOfExpression(TypeOfExpressionSyntax typeOfExpressionSyntax);
}

public partial class TypeOfExpressionBuilder
{
    public static TypeOfExpressionSyntax CreateSyntax(Action<ITypeBuilder> typeCallback)
    {
        var keywordToken = SyntaxFactory.Token(SyntaxKind.TypeOfKeyword);
        var openParenTokenToken = SyntaxFactory.Token(SyntaxKind.OpenParenToken);
        var typeSyntax = TypeBuilder.CreateSyntax(typeCallback);
        var closeParenTokenToken = SyntaxFactory.Token(SyntaxKind.CloseParenToken);
        return SyntaxFactory.TypeOfExpression(keywordToken, openParenTokenToken, typeSyntax, closeParenTokenToken);
    }
}