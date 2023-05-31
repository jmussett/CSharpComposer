using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithExternAliasDirective<TBuilder>
{
    TBuilder WithExternAliasDirective(string identifier);
    TBuilder WithExternAliasDirective(ExternAliasDirectiveSyntax externAliasDirectiveSyntax);
}

public interface IAddExternAliasDirective<TBuilder>
{
    TBuilder AddExternAliasDirective(string identifier);
    TBuilder AddExternAliasDirective(ExternAliasDirectiveSyntax externAliasDirectiveSyntax);
}

public partial class ExternAliasDirectiveBuilder
{
    public static ExternAliasDirectiveSyntax CreateSyntax(string identifier)
    {
        var externKeywordToken = SyntaxFactory.Token(SyntaxKind.ExternKeyword);
        var aliasKeywordToken = SyntaxFactory.Token(SyntaxKind.AliasKeyword);
        var identifierToken = SyntaxFactory.Identifier(identifier);
        var semicolonTokenToken = SyntaxFactory.Token(SyntaxKind.SemicolonToken);
        return SyntaxFactory.ExternAliasDirective(externKeywordToken, aliasKeywordToken, identifierToken, semicolonTokenToken);
    }
}