using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IContinueStatementBuilder : IStatementBuilder<IContinueStatementBuilder>
{
}

public interface IWithContinueStatementBuilder<TBuilder>
{
    TBuilder WithContinueStatement(Action<IContinueStatementBuilder> continueStatementCallback);
    TBuilder WithContinueStatement(ContinueStatementSyntax continueStatementSyntax);
}

public partial class ContinueStatementBuilder : IContinueStatementBuilder
{
    public ContinueStatementSyntax Syntax { get; set; }

    public ContinueStatementBuilder(ContinueStatementSyntax syntax)
    {
        Syntax = syntax;
    }

    public static ContinueStatementSyntax CreateSyntax(Action<IContinueStatementBuilder> continueStatementCallback)
    {
        var continueKeywordToken = SyntaxFactory.Token(SyntaxKind.ContinueKeyword);
        var semicolonTokenToken = SyntaxFactory.Token(SyntaxKind.SemicolonToken);
        var syntax = SyntaxFactory.ContinueStatement(default(SyntaxList<AttributeListSyntax>), continueKeywordToken, semicolonTokenToken);
        var builder = new ContinueStatementBuilder(syntax);
        continueStatementCallback(builder);
        return builder.Syntax;
    }

    public IContinueStatementBuilder AddAttribute(Action<INameBuilder> nameCallback, Action<IAttributeBuilder> attributeCallback)
    {
        var attribute = AttributeBuilder.CreateSyntax(nameCallback, attributeCallback);
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public IContinueStatementBuilder AddAttribute(AttributeSyntax attribute)
    {
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }
}