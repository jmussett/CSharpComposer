using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithImplicitStackAllocArrayCreationExpressionBuilder<TBuilder>
{
    TBuilder WithImplicitStackAllocArrayCreationExpression(InitializerExpressionKind initializerKind, Action<IInitializerExpressionBuilder> initializerInitializerExpressionCallback);
    TBuilder WithImplicitStackAllocArrayCreationExpression(ImplicitStackAllocArrayCreationExpressionSyntax implicitStackAllocArrayCreationExpressionSyntax);
}

public partial class ImplicitStackAllocArrayCreationExpressionBuilder
{
    public static ImplicitStackAllocArrayCreationExpressionSyntax CreateSyntax(InitializerExpressionKind initializerKind, Action<IInitializerExpressionBuilder> initializerInitializerExpressionCallback)
    {
        var stackAllocKeywordToken = SyntaxFactory.Token(SyntaxKind.StackAllocKeyword);
        var openBracketTokenToken = SyntaxFactory.Token(SyntaxKind.OpenBracketToken);
        var closeBracketTokenToken = SyntaxFactory.Token(SyntaxKind.CloseBracketToken);
        var initializerSyntax = InitializerExpressionBuilder.CreateSyntax(initializerKind, initializerInitializerExpressionCallback);
        return SyntaxFactory.ImplicitStackAllocArrayCreationExpression(stackAllocKeywordToken, openBracketTokenToken, closeBracketTokenToken, initializerSyntax);
    }
}