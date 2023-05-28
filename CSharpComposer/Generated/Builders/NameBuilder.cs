using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface INameBuilder
{
    void AsIdentifierName(string identifier);
    void AsGenericName(string identifier, Action<IGenericNameBuilder> genericNameCallback);
    void AsQualifiedName(Action<INameBuilder> leftCallback, Action<ISimpleNameBuilder> rightCallback);
    void AsAliasQualifiedName(string aliasIdentifier, Action<ISimpleNameBuilder> nameCallback);
}

public interface IWithNameBuilder<TBuilder>
{
    TBuilder WithName(Action<INameBuilder> nameCallback);
    TBuilder WithName(NameSyntax nameSyntax);
}

public partial class NameBuilder : INameBuilder
{
    public NameSyntax? Syntax { get; set; }

    public static NameSyntax CreateSyntax(Action<INameBuilder> callback)
    {
        var builder = new NameBuilder();
        callback(builder);
        if (builder.Syntax is null)
        {
            throw new InvalidOperationException("NameSyntax has not been specified");
        }

        return builder.Syntax;
    }

    public void AsIdentifierName(string identifier)
    {
        Syntax = IdentifierNameBuilder.CreateSyntax(identifier);
    }

    public void AsGenericName(string identifier, Action<IGenericNameBuilder> genericNameCallback)
    {
        Syntax = GenericNameBuilder.CreateSyntax(identifier, genericNameCallback);
    }

    public void AsQualifiedName(Action<INameBuilder> leftCallback, Action<ISimpleNameBuilder> rightCallback)
    {
        Syntax = QualifiedNameBuilder.CreateSyntax(leftCallback, rightCallback);
    }

    public void AsAliasQualifiedName(string aliasIdentifier, Action<ISimpleNameBuilder> nameCallback)
    {
        Syntax = AliasQualifiedNameBuilder.CreateSyntax(aliasIdentifier, nameCallback);
    }
}