using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IExpressionStatementBuilder : IStatementBuilder<IExpressionStatementBuilder>
{
}

internal partial class ExpressionStatementBuilder : IExpressionStatementBuilder
{
    public ExpressionStatementSyntax Syntax { get; set; }

    public ExpressionStatementBuilder(ExpressionStatementSyntax syntax)
    {
        Syntax = syntax;
    }

    public static ExpressionStatementSyntax CreateSyntax(Action<IExpressionBuilder> expressionCallback, Action<IExpressionStatementBuilder>? expressionStatementCallback = null)
    {
        var expressionSyntax = ExpressionBuilder.CreateSyntax(expressionCallback);
        var semicolonTokenToken = SyntaxFactory.Token(SyntaxKind.SemicolonToken);
        var syntax = SyntaxFactory.ExpressionStatement(default(SyntaxList<AttributeListSyntax>), expressionSyntax, semicolonTokenToken);
        var builder = new ExpressionStatementBuilder(syntax);
        expressionStatementCallback?.Invoke(builder);
        return builder.Syntax;
    }

    public IExpressionStatementBuilder AddAttribute(Action<INameBuilder> nameCallback, Action<IAttributeBuilder>? attributeCallback = null)
    {
        var attribute = AttributeBuilder.CreateSyntax(nameCallback, attributeCallback);
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public IExpressionStatementBuilder AddAttribute(AttributeSyntax attribute)
    {
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }
}