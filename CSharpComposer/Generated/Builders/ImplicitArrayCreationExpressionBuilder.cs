using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IImplicitArrayCreationExpressionBuilder
{
    IImplicitArrayCreationExpressionBuilder AddCommaToken(SyntaxToken comma);
}

public partial class ImplicitArrayCreationExpressionBuilder : IImplicitArrayCreationExpressionBuilder
{
    public ImplicitArrayCreationExpressionSyntax Syntax { get; set; }

    public ImplicitArrayCreationExpressionBuilder(ImplicitArrayCreationExpressionSyntax syntax)
    {
        Syntax = syntax;
    }

    public static ImplicitArrayCreationExpressionSyntax CreateSyntax(InitializerExpressionKind initializerKind, Action<IInitializerExpressionBuilder> initializerInitializerExpressionCallback, Action<IImplicitArrayCreationExpressionBuilder> implicitArrayCreationExpressionCallback)
    {
        var newKeywordToken = SyntaxFactory.Token(SyntaxKind.NewKeyword);
        var openBracketTokenToken = SyntaxFactory.Token(SyntaxKind.OpenBracketToken);
        var closeBracketTokenToken = SyntaxFactory.Token(SyntaxKind.CloseBracketToken);
        var initializerSyntax = InitializerExpressionBuilder.CreateSyntax(initializerKind, initializerInitializerExpressionCallback);
        var syntax = SyntaxFactory.ImplicitArrayCreationExpression(newKeywordToken, openBracketTokenToken, default(SyntaxTokenList), closeBracketTokenToken, initializerSyntax);
        var builder = new ImplicitArrayCreationExpressionBuilder(syntax);
        implicitArrayCreationExpressionCallback?.Invoke(builder);
        return builder.Syntax;
    }

    public IImplicitArrayCreationExpressionBuilder AddCommaToken(SyntaxToken comma)
    {
        Syntax = Syntax.AddCommas(comma);
        return this;
    }
}