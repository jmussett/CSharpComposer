using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IBlockBuilder : IStatementBuilder<IBlockBuilder>
{
    IBlockBuilder AddStatement(Action<IStatementBuilder> statementCallback);
    IBlockBuilder AddStatement(StatementSyntax statement);
}

public interface IWithBlockBuilder<TBuilder>
{
    TBuilder WithBlock(Action<IBlockBuilder> blockCallback);
    TBuilder WithBlock(BlockSyntax blockSyntax);
}

public partial class BlockBuilder : IBlockBuilder
{
    public BlockSyntax Syntax { get; set; }

    public BlockBuilder(BlockSyntax syntax)
    {
        Syntax = syntax;
    }

    public static BlockSyntax CreateSyntax(Action<IBlockBuilder> blockCallback)
    {
        var openBraceTokenToken = SyntaxFactory.Token(SyntaxKind.OpenBraceToken);
        var closeBraceTokenToken = SyntaxFactory.Token(SyntaxKind.CloseBraceToken);
        var syntax = SyntaxFactory.Block(default(SyntaxList<AttributeListSyntax>), openBraceTokenToken, default(SyntaxList<StatementSyntax>), closeBraceTokenToken);
        var builder = new BlockBuilder(syntax);
        blockCallback(builder);
        return builder.Syntax;
    }

    public IBlockBuilder AddAttribute(Action<INameBuilder> nameCallback, Action<IAttributeBuilder> attributeCallback)
    {
        var attribute = AttributeBuilder.CreateSyntax(nameCallback, attributeCallback);
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public IBlockBuilder AddAttribute(AttributeSyntax attribute)
    {
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public IBlockBuilder AddStatement(Action<IStatementBuilder> statementCallback)
    {
        var statement = StatementBuilder.CreateSyntax(statementCallback);
        Syntax = Syntax.AddStatements(statement);
        return this;
    }

    public IBlockBuilder AddStatement(StatementSyntax statement)
    {
        Syntax = Syntax.AddStatements(statement);
        return this;
    }
}