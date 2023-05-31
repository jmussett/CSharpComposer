using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithMemberBindingExpression<TBuilder>
{
    TBuilder WithMemberBindingExpression(Action<ISimpleNameBuilder> nameCallback);
    TBuilder WithMemberBindingExpression(MemberBindingExpressionSyntax memberBindingExpressionSyntax);
}

public interface IAddMemberBindingExpression<TBuilder>
{
    TBuilder AddMemberBindingExpression(Action<ISimpleNameBuilder> nameCallback);
    TBuilder AddMemberBindingExpression(MemberBindingExpressionSyntax memberBindingExpressionSyntax);
}

public partial class MemberBindingExpressionBuilder
{
    public static MemberBindingExpressionSyntax CreateSyntax(Action<ISimpleNameBuilder> nameCallback)
    {
        var operatorTokenToken = SyntaxFactory.Token(SyntaxKind.DotToken);
        var nameSyntax = SimpleNameBuilder.CreateSyntax(nameCallback);
        return SyntaxFactory.MemberBindingExpression(operatorTokenToken, nameSyntax);
    }
}