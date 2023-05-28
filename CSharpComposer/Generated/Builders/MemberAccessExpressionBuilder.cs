using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithMemberAccessExpressionBuilder<TBuilder>
{
    TBuilder WithMemberAccessExpression(MemberAccessExpressionKind kind, Action<IExpressionBuilder> expressionCallback, Action<ISimpleNameBuilder> nameCallback);
    TBuilder WithMemberAccessExpression(MemberAccessExpressionSyntax memberAccessExpressionSyntax);
}

public partial class MemberAccessExpressionBuilder
{
    public static MemberAccessExpressionSyntax CreateSyntax(MemberAccessExpressionKind kind, Action<IExpressionBuilder> expressionCallback, Action<ISimpleNameBuilder> nameCallback)
    {
        var syntaxKind = kind switch
        {
            MemberAccessExpressionKind.SimpleMemberAccessExpression => SyntaxKind.SimpleMemberAccessExpression,
            MemberAccessExpressionKind.PointerMemberAccessExpression => SyntaxKind.PointerMemberAccessExpression,
            _ => throw new NotSupportedException()
        };
        var expressionSyntax = ExpressionBuilder.CreateSyntax(expressionCallback);
        var operatorTokenToken = kind switch
        {
            MemberAccessExpressionKind.SimpleMemberAccessExpression => SyntaxFactory.Token(SyntaxKind.DotToken),
            MemberAccessExpressionKind.PointerMemberAccessExpression => SyntaxFactory.Token(SyntaxKind.MinusGreaterThanToken),
            _ => throw new NotSupportedException()
        };
        var nameSyntax = SimpleNameBuilder.CreateSyntax(nameCallback);
        return SyntaxFactory.MemberAccessExpression(syntaxKind, expressionSyntax, operatorTokenToken, nameSyntax);
    }
}