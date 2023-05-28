using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IAnonymousObjectCreationExpressionBuilder
{
    IAnonymousObjectCreationExpressionBuilder AddInitializer(Action<IExpressionBuilder> expressionCallback, Action<IAnonymousObjectMemberDeclaratorBuilder> anonymousObjectMemberDeclaratorCallback);
    IAnonymousObjectCreationExpressionBuilder AddInitializer(AnonymousObjectMemberDeclaratorSyntax initializer);
}

public interface IWithAnonymousObjectCreationExpressionBuilder<TBuilder>
{
    TBuilder WithAnonymousObjectCreationExpression(Action<IAnonymousObjectCreationExpressionBuilder> anonymousObjectCreationExpressionCallback);
    TBuilder WithAnonymousObjectCreationExpression(AnonymousObjectCreationExpressionSyntax anonymousObjectCreationExpressionSyntax);
}

public partial class AnonymousObjectCreationExpressionBuilder : IAnonymousObjectCreationExpressionBuilder
{
    public AnonymousObjectCreationExpressionSyntax Syntax { get; set; }

    public AnonymousObjectCreationExpressionBuilder(AnonymousObjectCreationExpressionSyntax syntax)
    {
        Syntax = syntax;
    }

    public static AnonymousObjectCreationExpressionSyntax CreateSyntax(Action<IAnonymousObjectCreationExpressionBuilder> anonymousObjectCreationExpressionCallback)
    {
        var newKeywordToken = SyntaxFactory.Token(SyntaxKind.NewKeyword);
        var openBraceTokenToken = SyntaxFactory.Token(SyntaxKind.OpenBraceToken);
        var closeBraceTokenToken = SyntaxFactory.Token(SyntaxKind.CloseBraceToken);
        var syntax = SyntaxFactory.AnonymousObjectCreationExpression(newKeywordToken, openBraceTokenToken, default(SeparatedSyntaxList<AnonymousObjectMemberDeclaratorSyntax>), closeBraceTokenToken);
        var builder = new AnonymousObjectCreationExpressionBuilder(syntax);
        anonymousObjectCreationExpressionCallback(builder);
        return builder.Syntax;
    }

    public IAnonymousObjectCreationExpressionBuilder AddInitializer(Action<IExpressionBuilder> expressionCallback, Action<IAnonymousObjectMemberDeclaratorBuilder> anonymousObjectMemberDeclaratorCallback)
    {
        var initializer = AnonymousObjectMemberDeclaratorBuilder.CreateSyntax(expressionCallback, anonymousObjectMemberDeclaratorCallback);
        Syntax = Syntax.AddInitializers(initializer);
        return this;
    }

    public IAnonymousObjectCreationExpressionBuilder AddInitializer(AnonymousObjectMemberDeclaratorSyntax initializer)
    {
        Syntax = Syntax.AddInitializers(initializer);
        return this;
    }
}