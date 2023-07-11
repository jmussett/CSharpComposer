using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IIndexerMemberCrefBuilder
{
    IIndexerMemberCrefBuilder AddCrefParameter(Action<ITypeBuilder> typeCallback, Action<ICrefParameterBuilder>? crefParameterCallback = null);
    IIndexerMemberCrefBuilder AddCrefParameter(CrefParameterSyntax parameter);
}

public partial class IndexerMemberCrefBuilder : IIndexerMemberCrefBuilder
{
    public IndexerMemberCrefSyntax Syntax { get; set; }

    public IndexerMemberCrefBuilder(IndexerMemberCrefSyntax syntax)
    {
        Syntax = syntax;
    }

    public static IndexerMemberCrefSyntax CreateSyntax(Action<IIndexerMemberCrefBuilder>? indexerMemberCrefCallback = null)
    {
        var thisKeywordToken = SyntaxFactory.Token(SyntaxKind.ThisKeyword);
        var syntax = SyntaxFactory.IndexerMemberCref(thisKeywordToken, default(CrefBracketedParameterListSyntax));
        var builder = new IndexerMemberCrefBuilder(syntax);
        indexerMemberCrefCallback?.Invoke(builder);
        return builder.Syntax;
    }

    public IIndexerMemberCrefBuilder AddCrefParameter(Action<ITypeBuilder> typeCallback, Action<ICrefParameterBuilder>? crefParameterCallback = null)
    {
        var parameter = CrefParameterBuilder.CreateSyntax(typeCallback, crefParameterCallback);
        Syntax = Syntax.AddParametersParameters(parameter);
        return this;
    }

    public IIndexerMemberCrefBuilder AddCrefParameter(CrefParameterSyntax parameter)
    {
        Syntax = Syntax.AddParametersParameters(parameter);
        return this;
    }
}