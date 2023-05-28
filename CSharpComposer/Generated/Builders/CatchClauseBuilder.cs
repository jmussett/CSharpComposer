using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface ICatchClauseBuilder
{
    ICatchClauseBuilder WithDeclaration(Action<ITypeBuilder> typeCallback, Action<ICatchDeclarationBuilder> catchDeclarationCallback);
    ICatchClauseBuilder WithDeclaration(CatchDeclarationSyntax declaration);
    ICatchClauseBuilder WithFilter(Action<IExpressionBuilder> filterExpressionCallback);
    ICatchClauseBuilder WithFilter(CatchFilterClauseSyntax filter);
}

public interface IWithCatchClauseBuilder<TBuilder>
{
    TBuilder WithCatchClause(Action<IBlockBuilder> blockBlockCallback, Action<ICatchClauseBuilder> catchClauseCallback);
    TBuilder WithCatchClause(CatchClauseSyntax catchClauseSyntax);
}

public partial class CatchClauseBuilder : ICatchClauseBuilder
{
    public CatchClauseSyntax Syntax { get; set; }

    public CatchClauseBuilder(CatchClauseSyntax syntax)
    {
        Syntax = syntax;
    }

    public static CatchClauseSyntax CreateSyntax(Action<IBlockBuilder> blockBlockCallback, Action<ICatchClauseBuilder> catchClauseCallback)
    {
        var catchKeywordToken = SyntaxFactory.Token(SyntaxKind.CatchKeyword);
        var blockSyntax = BlockBuilder.CreateSyntax(blockBlockCallback);
        var syntax = SyntaxFactory.CatchClause(catchKeywordToken, default(CatchDeclarationSyntax), default(CatchFilterClauseSyntax), blockSyntax);
        var builder = new CatchClauseBuilder(syntax);
        catchClauseCallback(builder);
        return builder.Syntax;
    }

    public ICatchClauseBuilder WithDeclaration(Action<ITypeBuilder> typeCallback, Action<ICatchDeclarationBuilder> catchDeclarationCallback)
    {
        var declarationSyntax = CatchDeclarationBuilder.CreateSyntax(typeCallback, catchDeclarationCallback);
        Syntax = Syntax.WithDeclaration(declarationSyntax);
        return this;
    }

    public ICatchClauseBuilder WithDeclaration(CatchDeclarationSyntax declaration)
    {
        Syntax = Syntax.WithDeclaration(declaration);
        return this;
    }

    public ICatchClauseBuilder WithFilter(Action<IExpressionBuilder> filterExpressionCallback)
    {
        var filterSyntax = CatchFilterClauseBuilder.CreateSyntax(filterExpressionCallback);
        Syntax = Syntax.WithFilter(filterSyntax);
        return this;
    }

    public ICatchClauseBuilder WithFilter(CatchFilterClauseSyntax filter)
    {
        Syntax = Syntax.WithFilter(filter);
        return this;
    }
}