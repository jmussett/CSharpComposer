using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface ICheckedStatementBuilder : IStatementBuilder<ICheckedStatementBuilder>
{
}

public interface IWithCheckedStatementBuilder<TBuilder>
{
    TBuilder WithCheckedStatement(CheckedStatementKind kind, Action<IBlockBuilder> blockBlockCallback, Action<ICheckedStatementBuilder> checkedStatementCallback);
    TBuilder WithCheckedStatement(CheckedStatementSyntax checkedStatementSyntax);
}

public partial class CheckedStatementBuilder : ICheckedStatementBuilder
{
    public CheckedStatementSyntax Syntax { get; set; }

    public CheckedStatementBuilder(CheckedStatementSyntax syntax)
    {
        Syntax = syntax;
    }

    public static CheckedStatementSyntax CreateSyntax(CheckedStatementKind kind, Action<IBlockBuilder> blockBlockCallback, Action<ICheckedStatementBuilder> checkedStatementCallback)
    {
        var syntaxKind = kind switch
        {
            CheckedStatementKind.CheckedStatement => SyntaxKind.CheckedStatement,
            CheckedStatementKind.UncheckedStatement => SyntaxKind.UncheckedStatement,
            _ => throw new NotSupportedException()
        };
        var keywordToken = kind switch
        {
            CheckedStatementKind.CheckedStatement => SyntaxFactory.Token(SyntaxKind.CheckedKeyword),
            CheckedStatementKind.UncheckedStatement => SyntaxFactory.Token(SyntaxKind.UncheckedKeyword),
            _ => throw new NotSupportedException()
        };
        var blockSyntax = BlockBuilder.CreateSyntax(blockBlockCallback);
        var syntax = SyntaxFactory.CheckedStatement(syntaxKind, default(SyntaxList<AttributeListSyntax>), keywordToken, blockSyntax);
        var builder = new CheckedStatementBuilder(syntax);
        checkedStatementCallback(builder);
        return builder.Syntax;
    }

    public ICheckedStatementBuilder AddAttribute(Action<INameBuilder> nameCallback, Action<IAttributeBuilder> attributeCallback)
    {
        var attribute = AttributeBuilder.CreateSyntax(nameCallback, attributeCallback);
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public ICheckedStatementBuilder AddAttribute(AttributeSyntax attribute)
    {
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }
}