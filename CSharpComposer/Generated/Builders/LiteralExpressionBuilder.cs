using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface ILiteralExpressionBuilder
{
    void AsNumericLiteral(int numericLiteral);
    void AsStringLiteral(string stringLiteral);
    void AsCharacterLiteral(char characterLiteral);
    void AsTrueLiteral();
    void AsFalseLiteral();
    void AsNullLiteral();
    void AsDefaultLiteral();
}

public interface IWithLiteralExpressionBuilder<TBuilder>
{
    TBuilder WithLiteralExpression(Action<ILiteralExpressionBuilder> literalExpressionCallback);
    TBuilder WithLiteralExpression(LiteralExpressionSyntax literalExpressionSyntax);
}

public partial class LiteralExpressionBuilder : ILiteralExpressionBuilder
{
    public LiteralExpressionSyntax? Syntax { get; set; }

    public static LiteralExpressionSyntax CreateSyntax(Action<ILiteralExpressionBuilder> callback)
    {
        var builder = new LiteralExpressionBuilder();
        callback(builder);
        if (builder.Syntax is null)
        {
            throw new InvalidOperationException("LiteralExpressionSyntax has not been specified");
        }

        return builder.Syntax;
    }

    public void AsNumericLiteral(int numericLiteral)
    {
        var numericLiteralToken = SyntaxFactory.Literal(numericLiteral);
        Syntax = SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, numericLiteralToken);
    }

    public void AsStringLiteral(string stringLiteral)
    {
        var stringLiteralToken = SyntaxFactory.Literal(stringLiteral);
        Syntax = SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, stringLiteralToken);
    }

    public void AsCharacterLiteral(char characterLiteral)
    {
        var characterLiteralToken = SyntaxFactory.Literal(characterLiteral);
        Syntax = SyntaxFactory.LiteralExpression(SyntaxKind.CharacterLiteralExpression, characterLiteralToken);
    }

    public void AsTrueLiteral()
    {
        Syntax = SyntaxFactory.LiteralExpression(SyntaxKind.TrueLiteralExpression);
    }

    public void AsFalseLiteral()
    {
        Syntax = SyntaxFactory.LiteralExpression(SyntaxKind.FalseLiteralExpression);
    }

    public void AsNullLiteral()
    {
        Syntax = SyntaxFactory.LiteralExpression(SyntaxKind.NullLiteralExpression);
    }

    public void AsDefaultLiteral()
    {
        Syntax = SyntaxFactory.LiteralExpression(SyntaxKind.DefaultLiteralExpression);
    }
}