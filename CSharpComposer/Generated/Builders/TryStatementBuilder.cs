using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface ITryStatementBuilder : IWithFinallyClause<ITryStatementBuilder>, IStatementBuilder<ITryStatementBuilder>
{
    ITryStatementBuilder AddCatch(Action<IBlockBuilder> blockBlockCallback, Action<ICatchClauseBuilder> catchClauseCallback);
    ITryStatementBuilder AddCatch(CatchClauseSyntax @catch);
    ITryStatementBuilder WithFinallyClause(Action<IBlockBuilder> blockBlockCallback);
    ITryStatementBuilder WithFinallyClause(FinallyClauseSyntax @finally);
}

public interface IWithTryStatement<TBuilder>
{
    TBuilder WithTryStatement(Action<IBlockBuilder> blockBlockCallback, Action<ITryStatementBuilder> tryStatementCallback);
    TBuilder WithTryStatement(TryStatementSyntax tryStatementSyntax);
}

public partial class TryStatementBuilder : ITryStatementBuilder
{
    public TryStatementSyntax Syntax { get; set; }

    public TryStatementBuilder(TryStatementSyntax syntax)
    {
        Syntax = syntax;
    }

    public static TryStatementSyntax CreateSyntax(Action<IBlockBuilder> blockBlockCallback, Action<ITryStatementBuilder> tryStatementCallback)
    {
        var tryKeywordToken = SyntaxFactory.Token(SyntaxKind.TryKeyword);
        var blockSyntax = BlockBuilder.CreateSyntax(blockBlockCallback);
        var syntax = SyntaxFactory.TryStatement(default(SyntaxList<AttributeListSyntax>), tryKeywordToken, blockSyntax, default(SyntaxList<CatchClauseSyntax>), default(FinallyClauseSyntax));
        var builder = new TryStatementBuilder(syntax);
        tryStatementCallback(builder);
        return builder.Syntax;
    }

    public ITryStatementBuilder AddAttribute(Action<INameBuilder> nameCallback, Action<IAttributeBuilder> attributeCallback)
    {
        var attribute = AttributeBuilder.CreateSyntax(nameCallback, attributeCallback);
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public ITryStatementBuilder AddAttribute(AttributeSyntax attribute)
    {
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public ITryStatementBuilder AddCatch(Action<IBlockBuilder> blockBlockCallback, Action<ICatchClauseBuilder> catchClauseCallback)
    {
        var @catch = CatchClauseBuilder.CreateSyntax(blockBlockCallback, catchClauseCallback);
        Syntax = Syntax.AddCatches(@catch);
        return this;
    }

    public ITryStatementBuilder AddCatch(CatchClauseSyntax @catch)
    {
        Syntax = Syntax.AddCatches(@catch);
        return this;
    }

    public ITryStatementBuilder WithFinallyClause(Action<IBlockBuilder> blockBlockCallback)
    {
        var finallySyntax = FinallyClauseBuilder.CreateSyntax(blockBlockCallback);
        Syntax = Syntax.WithFinally(finallySyntax);
        return this;
    }

    public ITryStatementBuilder WithFinallyClause(FinallyClauseSyntax @finally)
    {
        Syntax = Syntax.WithFinally(@finally);
        return this;
    }
}