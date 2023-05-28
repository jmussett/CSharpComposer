using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IBreakStatementBuilder : IStatementBuilder<IBreakStatementBuilder>
{
}

public interface IWithBreakStatementBuilder<TBuilder>
{
    TBuilder WithBreakStatement(Action<IBreakStatementBuilder> breakStatementCallback);
    TBuilder WithBreakStatement(BreakStatementSyntax breakStatementSyntax);
}

public partial class BreakStatementBuilder : IBreakStatementBuilder
{
    public BreakStatementSyntax Syntax { get; set; }

    public BreakStatementBuilder(BreakStatementSyntax syntax)
    {
        Syntax = syntax;
    }

    public static BreakStatementSyntax CreateSyntax(Action<IBreakStatementBuilder> breakStatementCallback)
    {
        var breakKeywordToken = SyntaxFactory.Token(SyntaxKind.BreakKeyword);
        var semicolonTokenToken = SyntaxFactory.Token(SyntaxKind.SemicolonToken);
        var syntax = SyntaxFactory.BreakStatement(default(SyntaxList<AttributeListSyntax>), breakKeywordToken, semicolonTokenToken);
        var builder = new BreakStatementBuilder(syntax);
        breakStatementCallback(builder);
        return builder.Syntax;
    }

    public IBreakStatementBuilder AddAttribute(Action<INameBuilder> nameCallback, Action<IAttributeBuilder> attributeCallback)
    {
        var attribute = AttributeBuilder.CreateSyntax(nameCallback, attributeCallback);
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public IBreakStatementBuilder AddAttribute(AttributeSyntax attribute)
    {
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }
}