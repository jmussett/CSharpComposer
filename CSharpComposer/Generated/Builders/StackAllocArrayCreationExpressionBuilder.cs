using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IStackAllocArrayCreationExpressionBuilder
{
    IStackAllocArrayCreationExpressionBuilder WithInitializer(InitializerExpressionKind kind, Action<IInitializerExpressionBuilder> initializerExpressionCallback);
    IStackAllocArrayCreationExpressionBuilder WithInitializer(InitializerExpressionSyntax initializer);
}

public interface IWithStackAllocArrayCreationExpressionBuilder<TBuilder>
{
    TBuilder WithStackAllocArrayCreationExpression(Action<ITypeBuilder> typeCallback, Action<IStackAllocArrayCreationExpressionBuilder> stackAllocArrayCreationExpressionCallback);
    TBuilder WithStackAllocArrayCreationExpression(StackAllocArrayCreationExpressionSyntax stackAllocArrayCreationExpressionSyntax);
}

public partial class StackAllocArrayCreationExpressionBuilder : IStackAllocArrayCreationExpressionBuilder
{
    public StackAllocArrayCreationExpressionSyntax Syntax { get; set; }

    public StackAllocArrayCreationExpressionBuilder(StackAllocArrayCreationExpressionSyntax syntax)
    {
        Syntax = syntax;
    }

    public static StackAllocArrayCreationExpressionSyntax CreateSyntax(Action<ITypeBuilder> typeCallback, Action<IStackAllocArrayCreationExpressionBuilder> stackAllocArrayCreationExpressionCallback)
    {
        var stackAllocKeywordToken = SyntaxFactory.Token(SyntaxKind.StackAllocKeyword);
        var typeSyntax = TypeBuilder.CreateSyntax(typeCallback);
        var syntax = SyntaxFactory.StackAllocArrayCreationExpression(stackAllocKeywordToken, typeSyntax, default(InitializerExpressionSyntax));
        var builder = new StackAllocArrayCreationExpressionBuilder(syntax);
        stackAllocArrayCreationExpressionCallback(builder);
        return builder.Syntax;
    }

    public IStackAllocArrayCreationExpressionBuilder WithInitializer(InitializerExpressionKind kind, Action<IInitializerExpressionBuilder> initializerExpressionCallback)
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