using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IIndexerMemberCrefBuilder
{
    IIndexerMemberCrefBuilder AddParameter(Action<ITypeBuilder> typeCallback, Action<ICrefParameterBuilder> crefParameterCallback);
    IIndexerMemberCrefBuilder AddParameter(CrefParameterSyntax parameter);
}

public interface IWithIndexerMemberCrefBuilder<TBuilder>
{
    TBuilder WithIndexerMemberCref(Action<IIndexerMemberCrefBuilder> indexerMemberCrefCallback);
    TBuilder WithIndexerMemberCref(IndexerMemberCrefSyntax indexerMemberCrefSyntax);
}

public partial class IndexerMemberCrefBuilder : IIndexerMemberCrefBuilder
{
    public IndexerMemberCrefSyntax Syntax { get; set; }

    public IndexerMemberCrefBuilder(IndexerMemberCrefSyntax syntax)
    {
        Syntax = syntax;
    }

    public static IndexerMemberCrefSyntax CreateSyntax(Action<IIndexerMemberCrefBuilder> indexerMemberCrefCallback)
    {
        var thisKeywordToken = SyntaxFactory.Token(SyntaxKind.ThisKeyword);
        var syntax = SyntaxFactory.IndexerMemberCref(thisKeywordToken, default(CrefBracketedParameterListSyntax));
        var builder = new IndexerMemberCrefBuilder(syntax);
        indexerMemberCrefCallback(builder);
        return builder.Syntax;
    }

    public IIndexerMemberCrefBuilder AddParameter(Action<ITypeBuilder> typeCallback, Action<ICrefParameterBuilder> crefParameterCallback)
    {
        var parameter = CrefParameterBuilder.CreateSyntax(typeCallback, crefParameterCallback);
        Syntax = Syntax.AddParametersParameters(parameter);
        return this;
    }

    public IIndexerMemberCrefBuilder AddParameter(CrefParameterSyntax parameter)
    {
        Syntax = Syntax.AddParametersParameters(parameter);
        return this;
    }
}