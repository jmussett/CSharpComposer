using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IUsingDirectiveBuilder
{
    IUsingDirectiveBuilder WithStaticKeyword(StaticKeyword staticKeyword);
    IUsingDirectiveBuilder WithAlias(string nameIdentifier);
    IUsingDirectiveBuilder WithAlias(NameEqualsSyntax alias);
    IUsingDirectiveBuilder WithGlobalKeyword();
}

public interface IWithUsingDirectiveBuilder<TBuilder>
{
    TBuilder WithUsingDirective(Action<INameBuilder> nameCallback, Action<IUsingDirectiveBuilder> usingDirectiveCallback);
    TBuilder WithUsingDirective(UsingDirectiveSyntax usingDirectiveSyntax);
}

public partial class UsingDirectiveBuilder : IUsingDirectiveBuilder
{
    public UsingDirectiveSyntax Syntax { get; set; }

    public UsingDirectiveBuilder(UsingDirectiveSyntax syntax)
    {
        Syntax = syntax;
    }

    public static UsingDirectiveSyntax CreateSyntax(Action<INameBuilder> nameCallback, Action<IUsingDirectiveBuilder> usingDirectiveCallback)
    {
        var usingKeywordToken = SyntaxFactory.Token(SyntaxKind.UsingKeyword);
        var nameSyntax = NameBuilder.CreateSyntax(nameCallback);
        var semicolonTokenToken = SyntaxFactory.Token(SyntaxKind.SemicolonToken);
        var syntax = SyntaxFactory.UsingDirective(default(SyntaxToken), usingKeywordToken, default(SyntaxToken), null, nameSyntax, semicolonTokenToken);
        var builder = new UsingDirectiveBuilder(syntax);
        usingDirectiveCallback(builder);
        return builder.Syntax;
    }

    public IUsingDirectiveBuilder WithStaticKeyword(StaticKeyword staticKeyword)
    {
        Syntax = Syntax.WithStaticKeyword(SyntaxFactory.Token(staticKeyword switch
        {
            _ => throw new NotSupportedException()
        }));
        return this;
    }

    public IUsingDirectiveBuilder WithAlias(string nameIdentifier)
    {
        var aliasSyntax = NameEqualsBuilder.CreateSyntax(nameIdentifier);
        Syntax = Syntax.WithAlias(aliasSyntax);
        return this;
    }

    public IUsingDirectiveBuilder WithAlias(NameEqualsSyntax alias)
    {
        Syntax = Syntax.WithAlias(alias);
        return this;
    }

    public IUsingDirectiveBuilder WithGlobalKeyword()
    {
        Syntax = Syntax.WithGlobalKeyword(SyntaxFactory.Token(SyntaxKind.GlobalKeyword));
        return this;
    }
}