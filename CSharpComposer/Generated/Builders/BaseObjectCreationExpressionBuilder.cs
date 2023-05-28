using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IBaseObjectCreationExpressionBuilder
{
    void AsImplicitObjectCreationExpression(Action<IImplicitObjectCreationExpressionBuilder> implicitObjectCreationExpressionCallback);
    void AsObjectCreationExpression(Action<ITypeBuilder> typeCallback, Action<IObjectCreationExpressionBuilder> objectCreationExpressionCallback);
}

public partial interface IBaseObjectCreationExpressionBuilder<TBuilder>
{
    TBuilder WithInitializer(InitializerExpressionKind kind, Action<IInitializerExpressionBuilder> initializerExpressionCallback);
    TBuilder WithInitializer(InitializerExpressionSyntax initializer);
}

public interface IWithBaseObjectCreationExpressionBuilder<TBuilder>
{
    TBuilder WithBaseObjectCreationExpression(Action<IBaseObjectCreationExpressionBuilder> baseObjectCreationExpressionCallback);
    TBuilder WithBaseObjectCreationExpression(BaseObjectCreationExpressionSyntax baseObjectCreationExpressionSyntax);
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

    public void AsImplicitObjectCreationExpression(Action<IImplicitObjectCreationExpressionBuilder> implicitObjectCreationExpressionCallback)
    {
        Syntax = ImplicitObjectCreationExpressionBuilder.CreateSyntax(implicitObjectCreationExpressionCallback);
    }

    public void AsObjectCreationExpression(Action<ITypeBuilder> typeCallback, Action<IObjectCreationExpressionBuilder> objectCreationExpressionCallback)
    {
        Syntax = ObjectCreationExpressionBuilder.CreateSyntax(typeCallback, objectCreationExpressionCallback);
    }
}