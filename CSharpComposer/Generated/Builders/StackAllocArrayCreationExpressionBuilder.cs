using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IStackAllocArrayCreationExpressionBuilder
{
    IStackAllocArrayCreationExpressionBuilder WithInitializer(InitializerExpressionKind kind, Action<IInitializerExpressionBuilder>? initializerExpressionCallback = null);
    IStackAllocArrayCreationExpressionBuilder WithInitializer(InitializerExpressionSyntax initializer);
}

internal partial class StackAllocArrayCreationExpressionBuilder : IStackAllocArrayCreationExpressionBuilder
{
    public StackAllocArrayCreationExpressionSyntax Syntax { get; set; }

    public StackAllocArrayCreationExpressionBuilder(StackAllocArrayCreationExpressionSyntax syntax)
    {
        Syntax = syntax;
    }

    public static StackAllocArrayCreationExpressionSyntax CreateSyntax(Action<ITypeBuilder> typeCallback, Action<IStackAllocArrayCreationExpressionBuilder>? stackAllocArrayCreationExpressionCallback = null)
    {
        var stackAllocKeywordToken = SyntaxFactory.Token(SyntaxKind.StackAllocKeyword);
        var typeSyntax = TypeBuilder.CreateSyntax(typeCallback);
        var syntax = SyntaxFactory.StackAllocArrayCreationExpression(stackAllocKeywordToken, typeSyntax, default(InitializerExpressionSyntax));
        var builder = new StackAllocArrayCreationExpressionBuilder(syntax);
        stackAllocArrayCreationExpressionCallback?.Invoke(builder);
        return builder.Syntax;
    }

    public IStackAllocArrayCreationExpressionBuilder WithInitializer(InitializerExpressionKind kind, Action<IInitializerExpressionBuilder>? initializerExpressionCallback = null)
    {
        var initializerSyntax = InitializerExpressionBuilder.CreateSyntax(kind, initializerExpressionCallback);
        Syntax = Syntax.WithInitializer(initializerSyntax);
        return this;
    }

    public IStackAllocArrayCreationExpressionBuilder WithInitializer(InitializerExpressionSyntax initializer)
    {
        Syntax = Syntax.WithInitializer(initializer);
        return this;
    }
}