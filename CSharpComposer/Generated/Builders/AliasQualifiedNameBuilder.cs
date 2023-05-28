using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithAliasQualifiedNameBuilder<TBuilder>
{
    TBuilder WithAliasQualifiedName(string aliasIdentifier, Action<ISimpleNameBuilder> nameCallback);
    TBuilder WithAliasQualifiedName(AliasQualifiedNameSyntax aliasQualifiedNameSyntax);
}

public partial class AliasQualifiedNameBuilder
{
    public static AliasQualifiedNameSyntax CreateSyntax(string aliasIdentifier, Action<ISimpleNameBuilder> nameCallback)
    {
        var aliasSyntax = IdentifierNameBuilder.CreateSyntax(aliasIdentifier);
        var colonColonTokenToken = SyntaxFactory.Token(SyntaxKind.ColonColonToken);
        var nameSyntax = SimpleNameBuilder.CreateSyntax(nameCallback);
        return SyntaxFactory.AliasQualifiedName(aliasSyntax, colonColonTokenToken, nameSyntax);
    }
}