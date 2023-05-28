using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IUnsafeStatementBuilder : IStatementBuilder<IUnsafeStatementBuilder>
{
}

public interface IWithUnsafeStatementBuilder<TBuilder>
{
    TBuilder WithUnsafeStatement(Action<IBlockBuilder> blockBlockCallback, Action<IUnsafeStatementBuilder> unsafeStatementCallback);
    TBuilder WithUnsafeStatement(UnsafeStatementSyntax unsafeStatementSyntax);
}

public partial class UnsafeStatementBuilder : IUnsafeStatementBuilder
{
    public UnsafeStatementSyntax Syntax { get; set; }

    public UnsafeStatementBuilder(UnsafeStatementSyntax syntax)
    {
        Syntax = syntax;
    }

    public static UnsafeStatementSyntax CreateSyntax(Action<IBlockBuilder> blockBlockCallback, Action<IUnsafeStatementBuilder> unsafeStatementCallback)
    {
        var unsafeKeywordToken = SyntaxFactory.Token(SyntaxKind.UnsafeKeyword);
        var blockSyntax = BlockBuilder.CreateSyntax(blockBlockCallback);
        var syntax = SyntaxFactory.UnsafeStatement(default(SyntaxList<AttributeListSyntax>), unsafeKeywordToken, blockSyntax);
        var builder = new UnsafeStatementBuilder(syntax);
        unsafeStatementCallback(builder);
        return builder.Syntax;
    }

    public IUnsafeStatementBuilder AddAttribute(Action<INameBuilder> nameCallback, Action<IAttributeBuilder> attributeCallback)
    {
        var attribute = AttributeBuilder.CreateSyntax(nameCallback, attributeCallback);
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public IUnsafeStatementBuilder AddAttribute(AttributeSyntax attribute)
    {
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }
}