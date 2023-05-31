using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IAnonymousObjectCreationExpressionBuilder
{
    IAnonymousObjectCreationExpressionBuilder AddInitializerAnonymousObjectMemberDeclarator(Action<IExpressionBuilder> expressionCallback, Action<IAnonymousObjectMemberDeclaratorBuilder> anonymousObjectMemberDeclaratorCallback);
    IAnonymousObjectCreationExpressionBuilder AddInitializerAnonymousObjectMemberDeclarator(AnonymousObjectMemberDeclaratorSyntax initializer);
}

public interface IWithAnonymousObjectCreationExpression<TBuilder>
{
    TBuilder WithAnonymousObjectCreationExpression(Action<IAnonymousObjectCreationExpressionBuilder> anonymousObjectCreationExpressionCallback);
    TBuilder WithAnonymousObjectCreationExpression(AnonymousObjectCreationExpressionSyntax anonymousObjectCreationExpressionSyntax);
}

public interface IAddAnonymousObjectCreationExpression<TBuilder>
{
    TBuilder AddAnonymousObjectCreationExpression(Action<IAnonymousObjectCreationExpressionBuilder> anonymousObjectCreationExpressionCallback);
    TBuilder AddAnonymousObjectCreationExpression(AnonymousObjectCreationExpressionSyntax anonymousObjectCreationExpressionSyntax);
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

    public IAnonymousObjectCreationExpressionBuilder AddInitializerAnonymousObjectMemberDeclarator(Action<IExpressionBuilder> expressionCallback, Action<IAnonymousObjectMemberDeclaratorBuilder> anonymousObjectMemberDeclaratorCallback)
    {
        var initializer = AnonymousObjectMemberDeclaratorBuilder.CreateSyntax(expressionCallback, anonymousObjectMemberDeclaratorCallback);
        Syntax = Syntax.AddInitializers(initializer);
        return this;
    }

    public IAnonymousObjectCreationExpressionBuilder AddInitializerAnonymousObjectMemberDeclarator(AnonymousObjectMemberDeclaratorSyntax initializer)
    {
        Syntax = Syntax.AddInitializers(initializer);
        return this;
    }
}