using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IOperatorMemberCrefBuilder
{
    IOperatorMemberCrefBuilder WithCheckedKeyword();
    IOperatorMemberCrefBuilder AddCrefParameter(Action<ITypeBuilder> typeCallback, Action<ICrefParameterBuilder>? crefParameterCallback = null);
    IOperatorMemberCrefBuilder AddCrefParameter(CrefParameterSyntax parameter);
}

internal partial class OperatorMemberCrefBuilder : IOperatorMemberCrefBuilder
{
    public OperatorMemberCrefSyntax Syntax { get; set; }

    public OperatorMemberCrefBuilder(OperatorMemberCrefSyntax syntax)
    {
        Syntax = syntax;
    }

    public static OperatorMemberCrefSyntax CreateSyntax(OperatorMemberCrefOperatorToken operatorMemberCrefOperatorToken, Action<IOperatorMemberCrefBuilder>? operatorMemberCrefCallback = null)
    {
        var operatorKeywordToken = SyntaxFactory.Token(SyntaxKind.OperatorKeyword);
        var operatorTokenToken = operatorMemberCrefOperatorToken switch
        {
            OperatorMemberCrefOperatorToken.PlusToken => SyntaxFactory.Token(SyntaxKind.PlusToken),
            OperatorMemberCrefOperatorToken.MinusToken => SyntaxFactory.Token(SyntaxKind.MinusToken),
            OperatorMemberCrefOperatorToken.ExclamationToken => SyntaxFactory.Token(SyntaxKind.ExclamationToken),
            OperatorMemberCrefOperatorToken.TildeToken => SyntaxFactory.Token(SyntaxKind.TildeToken),
            OperatorMemberCrefOperatorToken.PlusPlusToken => SyntaxFactory.Token(SyntaxKind.PlusPlusToken),
            OperatorMemberCrefOperatorToken.MinusMinusToken => SyntaxFactory.Token(SyntaxKind.MinusMinusToken),
            OperatorMemberCrefOperatorToken.AsteriskToken => SyntaxFactory.Token(SyntaxKind.AsteriskToken),
            OperatorMemberCrefOperatorToken.SlashToken => SyntaxFactory.Token(SyntaxKind.SlashToken),
            OperatorMemberCrefOperatorToken.PercentToken => SyntaxFactory.Token(SyntaxKind.PercentToken),
            OperatorMemberCrefOperatorToken.LessThanLessThanToken => SyntaxFactory.Token(SyntaxKind.LessThanLessThanToken),
            OperatorMemberCrefOperatorToken.GreaterThanGreaterThanToken => SyntaxFactory.Token(SyntaxKind.GreaterThanGreaterThanToken),
            OperatorMemberCrefOperatorToken.GreaterThanGreaterThanGreaterThanToken => SyntaxFactory.Token(SyntaxKind.GreaterThanGreaterThanGreaterThanToken),
            OperatorMemberCrefOperatorToken.BarToken => SyntaxFactory.Token(SyntaxKind.BarToken),
            OperatorMemberCrefOperatorToken.AmpersandToken => SyntaxFactory.Token(SyntaxKind.AmpersandToken),
            OperatorMemberCrefOperatorToken.CaretToken => SyntaxFactory.Token(SyntaxKind.CaretToken),
            OperatorMemberCrefOperatorToken.EqualsEqualsToken => SyntaxFactory.Token(SyntaxKind.EqualsEqualsToken),
            OperatorMemberCrefOperatorToken.ExclamationEqualsToken => SyntaxFactory.Token(SyntaxKind.ExclamationEqualsToken),
            OperatorMemberCrefOperatorToken.LessThanToken => SyntaxFactory.Token(SyntaxKind.LessThanToken),
            OperatorMemberCrefOperatorToken.LessThanEqualsToken => SyntaxFactory.Token(SyntaxKind.LessThanEqualsToken),
            OperatorMemberCrefOperatorToken.GreaterThanToken => SyntaxFactory.Token(SyntaxKind.GreaterThanToken),
            OperatorMemberCrefOperatorToken.GreaterThanEqualsToken => SyntaxFactory.Token(SyntaxKind.GreaterThanEqualsToken),
            OperatorMemberCrefOperatorToken.FalseKeyword => SyntaxFactory.Token(SyntaxKind.FalseKeyword),
            OperatorMemberCrefOperatorToken.TrueKeyword => SyntaxFactory.Token(SyntaxKind.TrueKeyword),
            _ => throw new NotSupportedException()
        };
        var syntax = SyntaxFactory.OperatorMemberCref(operatorKeywordToken, default(SyntaxToken), operatorTokenToken, default(CrefParameterListSyntax));
        var builder = new OperatorMemberCrefBuilder(syntax);
        operatorMemberCrefCallback?.Invoke(builder);
        return builder.Syntax;
    }

    public IOperatorMemberCrefBuilder WithCheckedKeyword()
    {
        Syntax = Syntax.WithCheckedKeyword(SyntaxFactory.Token(SyntaxKind.CheckedKeyword));
        return this;
    }

    public IOperatorMemberCrefBuilder AddCrefParameter(Action<ITypeBuilder> typeCallback, Action<ICrefParameterBuilder>? crefParameterCallback = null)
    {
        var parameter = CrefParameterBuilder.CreateSyntax(typeCallback, crefParameterCallback);
        Syntax = Syntax.AddParametersParameters(parameter);
        return this;
    }

    public IOperatorMemberCrefBuilder AddCrefParameter(CrefParameterSyntax parameter)
    {
        Syntax = Syntax.AddParametersParameters(parameter);
        return this;
    }
}