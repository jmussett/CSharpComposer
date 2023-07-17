using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IArrayCreationExpressionBuilder
{
    IArrayCreationExpressionBuilder WithInitializer(InitializerExpressionKind kind, Action<IInitializerExpressionBuilder>? initializerExpressionCallback = null);
    IArrayCreationExpressionBuilder WithInitializer(InitializerExpressionSyntax initializer);
}

internal partial class ArrayCreationExpressionBuilder : IArrayCreationExpressionBuilder
{
    public ArrayCreationExpressionSyntax Syntax { get; set; }

    public ArrayCreationExpressionBuilder(ArrayCreationExpressionSyntax syntax)
    {
        Syntax = syntax;
    }

    public static ArrayCreationExpressionSyntax CreateSyntax(Action<ITypeBuilder> typeElementTypeCallback, Action<IArrayTypeBuilder> typeArrayTypeCallback, Action<IArrayCreationExpressionBuilder> arrayCreationExpressionCallback)
    {
        var newKeywordToken = SyntaxFactory.Token(SyntaxKind.NewKeyword);
        var typeSyntax = ArrayTypeBuilder.CreateSyntax(typeElementTypeCallback, typeArrayTypeCallback);
        var syntax = SyntaxFactory.ArrayCreationExpression(newKeywordToken, typeSyntax, default(InitializerExpressionSyntax));
        var builder = new ArrayCreationExpressionBuilder(syntax);
        arrayCreationExpressionCallback?.Invoke(builder);
        return builder.Syntax;
    }

    public IArrayCreationExpressionBuilder WithInitializer(InitializerExpressionKind kind, Action<IInitializerExpressionBuilder>? initializerExpressionCallback = null)
    {
        var initializerSyntax = InitializerExpressionBuilder.CreateSyntax(kind, initializerExpressionCallback);
        Syntax = Syntax.WithInitializer(initializerSyntax);
        return this;
    }

    public IArrayCreationExpressionBuilder WithInitializer(InitializerExpressionSyntax initializer)
    {
        Syntax = Syntax.WithInitializer(initializer);
        return this;
    }
}