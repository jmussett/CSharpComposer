using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IArrayCreationExpressionBuilder
{
    IArrayCreationExpressionBuilder WithInitializer(InitializerExpressionKind kind, Action<IInitializerExpressionBuilder> initializerExpressionCallback);
    IArrayCreationExpressionBuilder WithInitializer(InitializerExpressionSyntax initializer);
}

public interface IWithArrayCreationExpressionBuilder<TBuilder>
{
    TBuilder WithArrayCreationExpression(Action<ITypeBuilder> typeElementTypeCallback, Action<IArrayTypeBuilder> typeArrayTypeCallback, Action<IArrayCreationExpressionBuilder> arrayCreationExpressionCallback);
    TBuilder WithArrayCreationExpression(ArrayCreationExpressionSyntax arrayCreationExpressionSyntax);
}

public partial class ArrayCreationExpressionBuilder : IArrayCreationExpressionBuilder
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
        arrayCreationExpressionCallback(builder);
        return builder.Syntax;
    }

    public IArrayCreationExpressionBuilder WithInitializer(InitializerExpressionKind kind, Action<IInitializerExpressionBuilder> initializerExpressionCallback)
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