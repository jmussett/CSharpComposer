using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface ILabeledStatementBuilder : IStatementBuilder<ILabeledStatementBuilder>
{
}

public interface IWithLabeledStatementBuilder<TBuilder>
{
    TBuilder WithLabeledStatement(string identifier, Action<IStatementBuilder> statementCallback, Action<ILabeledStatementBuilder> labeledStatementCallback);
    TBuilder WithLabeledStatement(LabeledStatementSyntax labeledStatementSyntax);
}

public partial class LabeledStatementBuilder : ILabeledStatementBuilder
{
    public LabeledStatementSyntax Syntax { get; set; }

    public LabeledStatementBuilder(LabeledStatementSyntax syntax)
    {
        Syntax = syntax;
    }

    public static LabeledStatementSyntax CreateSyntax(string identifier, Action<IStatementBuilder> statementCallback, Action<ILabeledStatementBuilder> labeledStatementCallback)
    {
        var identifierToken = SyntaxFactory.Identifier(identifier);
        var colonTokenToken = SyntaxFactory.Token(SyntaxKind.ColonToken);
        var statementSyntax = StatementBuilder.CreateSyntax(statementCallback);
        var syntax = SyntaxFactory.LabeledStatement(default(SyntaxList<AttributeListSyntax>), identifierToken, colonTokenToken, statementSyntax);
        var builder = new LabeledStatementBuilder(syntax);
        labeledStatementCallback(builder);
        return builder.Syntax;
    }

    public ILabeledStatementBuilder AddAttribute(Action<INameBuilder> nameCallback, Action<IAttributeBuilder> attributeCallback)
    {
        var attribute = AttributeBuilder.CreateSyntax(nameCallback, attributeCallback);
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public ILabeledStatementBuilder AddAttribute(AttributeSyntax attribute)
    {
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }
}