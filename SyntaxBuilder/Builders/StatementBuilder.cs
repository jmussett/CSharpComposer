using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SyntaxBuilder.Builders;

public interface IStatementBuilder
{
    IStatementBuilder WithAttribute(Func<INameBuilder, INameBuilder> nameCallback, Func<IAttributeBuilder, IAttributeBuilder>? attributeCallback = null);
}

public sealed class StatementBuilder : IStatementBuilder
{
    public StatementSyntax Syntax { get; set; }

    public StatementBuilder(StatementSyntax syntax)
    {
        Syntax = syntax;
    }

    public static StatementSyntax ParseStatement(string statement)
    {
        return SyntaxFactory.ParseStatement(statement);
    }

    // TODO: name extension
    public IStatementBuilder WithAttribute(Func<INameBuilder, INameBuilder> nameCallback, Func<IAttributeBuilder, IAttributeBuilder>? attributeCallback = null)
    {
        var attributeSyntax = AttributeBuilder.CreateSyntax(nameCallback, attributeCallback);

        Syntax = Syntax.AddAttributeLists(
            SyntaxFactory.AttributeList(
                SyntaxFactory.SingletonSeparatedList(attributeSyntax)
            )
        );

        return this;
    }
}
