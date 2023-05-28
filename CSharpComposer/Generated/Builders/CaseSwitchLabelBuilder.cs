using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithCaseSwitchLabelBuilder<TBuilder>
{
    TBuilder WithCaseSwitchLabel(Action<IExpressionBuilder> valueCallback);
    TBuilder WithCaseSwitchLabel(CaseSwitchLabelSyntax caseSwitchLabelSyntax);
}

public partial class CaseSwitchLabelBuilder
{
    public static CaseSwitchLabelSyntax CreateSyntax(Action<IExpressionBuilder> valueCallback)
    {
        var keywordToken = SyntaxFactory.Token(SyntaxKind.CaseKeyword);
        var valueSyntax = ExpressionBuilder.CreateSyntax(valueCallback);
        var colonTokenToken = SyntaxFactory.Token(SyntaxKind.ColonToken);
        return SyntaxFactory.CaseSwitchLabel(keywordToken, valueSyntax, colonTokenToken);
    }
}