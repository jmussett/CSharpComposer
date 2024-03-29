﻿using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IImplicitObjectCreationExpressionBuilder : IBaseObjectCreationExpressionBuilder<IImplicitObjectCreationExpressionBuilder>
{
}

internal partial class ImplicitObjectCreationExpressionBuilder : IImplicitObjectCreationExpressionBuilder
{
    public ImplicitObjectCreationExpressionSyntax Syntax { get; set; }

    public ImplicitObjectCreationExpressionBuilder(ImplicitObjectCreationExpressionSyntax syntax)
    {
        Syntax = syntax;
    }

    public static ImplicitObjectCreationExpressionSyntax CreateSyntax(Action<IImplicitObjectCreationExpressionBuilder>? implicitObjectCreationExpressionCallback = null)
    {
        var newKeywordToken = SyntaxFactory.Token(SyntaxKind.NewKeyword);
        var argumentListSyntax = SyntaxFactory.ArgumentList();
        var syntax = SyntaxFactory.ImplicitObjectCreationExpression(newKeywordToken, argumentListSyntax, default(InitializerExpressionSyntax));
        var builder = new ImplicitObjectCreationExpressionBuilder(syntax);
        implicitObjectCreationExpressionCallback?.Invoke(builder);
        return builder.Syntax;
    }

    public IImplicitObjectCreationExpressionBuilder AddArgument(Action<IExpressionBuilder> expressionCallback, Action<IArgumentBuilder>? argumentCallback = null)
    {
        var argument = ArgumentBuilder.CreateSyntax(expressionCallback, argumentCallback);
        Syntax = Syntax.AddArgumentListArguments(argument);
        return this;
    }

    public IImplicitObjectCreationExpressionBuilder AddArgument(ArgumentSyntax argument)
    {
        Syntax = Syntax.AddArgumentListArguments(argument);
        return this;
    }

    public IImplicitObjectCreationExpressionBuilder WithInitializer(InitializerExpressionKind kind, Action<IInitializerExpressionBuilder>? initializerExpressionCallback = null)
    {
        var initializerSyntax = InitializerExpressionBuilder.CreateSyntax(kind, initializerExpressionCallback);
        Syntax = Syntax.WithInitializer(initializerSyntax);
        return this;
    }

    public IImplicitObjectCreationExpressionBuilder WithInitializer(InitializerExpressionSyntax initializer)
    {
        Syntax = Syntax.WithInitializer(initializer);
        return this;
    }
}