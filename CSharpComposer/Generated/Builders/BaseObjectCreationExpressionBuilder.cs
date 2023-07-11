using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IBaseObjectCreationExpressionBuilder
{
    void AsImplicitObjectCreationExpression(Action<IImplicitObjectCreationExpressionBuilder>? implicitObjectCreationExpressionCallback = null);
    void AsObjectCreationExpression(Action<ITypeBuilder> typeCallback, Action<IObjectCreationExpressionBuilder>? objectCreationExpressionCallback = null);
}

public partial interface IBaseObjectCreationExpressionBuilder<TBuilder> : IAddArgument<TBuilder>
{
    TBuilder WithInitializer(InitializerExpressionKind kind, Action<IInitializerExpressionBuilder>? initializerExpressionCallback = null);
    TBuilder WithInitializer(InitializerExpressionSyntax initializer);
}

public partial class BaseObjectCreationExpressionBuilder : IBaseObjectCreationExpressionBuilder
{
    public BaseObjectCreationExpressionSyntax? Syntax { get; set; }

    public static BaseObjectCreationExpressionSyntax CreateSyntax(Action<IBaseObjectCreationExpressionBuilder> callback)
    {
        var builder = new BaseObjectCreationExpressionBuilder();
        callback(builder);
        if (builder.Syntax is null)
        {
            throw new InvalidOperationException("BaseObjectCreationExpressionSyntax has not been specified");
        }

        return builder.Syntax;
    }

    public void AsImplicitObjectCreationExpression(Action<IImplicitObjectCreationExpressionBuilder>? implicitObjectCreationExpressionCallback = null)
    {
        Syntax = ImplicitObjectCreationExpressionBuilder.CreateSyntax(implicitObjectCreationExpressionCallback);
    }

    public void AsObjectCreationExpression(Action<ITypeBuilder> typeCallback, Action<IObjectCreationExpressionBuilder>? objectCreationExpressionCallback = null)
    {
        Syntax = ObjectCreationExpressionBuilder.CreateSyntax(typeCallback, objectCreationExpressionCallback);
    }
}