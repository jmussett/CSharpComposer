using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IObjectCreationExpressionBuilder : IBaseObjectCreationExpressionBuilder<IObjectCreationExpressionBuilder>
{
}

public partial class ObjectCreationExpressionBuilder : IObjectCreationExpressionBuilder
{
    public ObjectCreationExpressionSyntax Syntax { get; set; }

    public ObjectCreationExpressionBuilder(ObjectCreationExpressionSyntax syntax)
    {
        Syntax = syntax;
    }

    public static ObjectCreationExpressionSyntax CreateSyntax(Action<ITypeBuilder> typeCallback, Action<IObjectCreationExpressionBuilder>? objectCreationExpressionCallback = null)
    {
        var newKeywordToken = SyntaxFactory.Token(SyntaxKind.NewKeyword);
        var typeSyntax = TypeBuilder.CreateSyntax(typeCallback);
        var syntax = SyntaxFactory.ObjectCreationExpression(newKeywordToken, typeSyntax, default(ArgumentListSyntax), default(InitializerExpressionSyntax));
        var builder = new ObjectCreationExpressionBuilder(syntax);
        objectCreationExpressionCallback?.Invoke(builder);
        return builder.Syntax;
    }

    public IObjectCreationExpressionBuilder AddArgument(Action<IExpressionBuilder> expressionCallback, Action<IArgumentBuilder>? argumentCallback = null)
    {
        var argument = ArgumentBuilder.CreateSyntax(expressionCallback, argumentCallback);
        Syntax = Syntax.AddArgumentListArguments(argument);
        return this;
    }

    public IObjectCreationExpressionBuilder AddArgument(ArgumentSyntax argument)
    {
        Syntax = Syntax.AddArgumentListArguments(argument);
        return this;
    }

    public IObjectCreationExpressionBuilder WithInitializer(InitializerExpressionKind kind, Action<IInitializerExpressionBuilder>? initializerExpressionCallback = null)
    {
        var initializerSyntax = InitializerExpressionBuilder.CreateSyntax(kind, initializerExpressionCallback);
        Syntax = Syntax.WithInitializer(initializerSyntax);
        return this;
    }

    public IObjectCreationExpressionBuilder WithInitializer(InitializerExpressionSyntax initializer)
    {
        Syntax = Syntax.WithInitializer(initializer);
        return this;
    }
}