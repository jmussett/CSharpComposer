using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IGenericNameBuilder
{
    IGenericNameBuilder AddTypeArgument(Action<ITypeBuilder> argumentCallback);
    IGenericNameBuilder AddTypeArgument(TypeSyntax argument);
}

public interface IWithGenericNameBuilder<TBuilder>
{
    TBuilder WithGenericName(string identifier, Action<IGenericNameBuilder> genericNameCallback);
    TBuilder WithGenericName(GenericNameSyntax genericNameSyntax);
}

public partial class GenericNameBuilder : IGenericNameBuilder
{
    public GenericNameSyntax Syntax { get; set; }

    public GenericNameBuilder(GenericNameSyntax syntax)
    {
        Syntax = syntax;
    }

    public static GenericNameSyntax CreateSyntax(string identifier, Action<IGenericNameBuilder> genericNameCallback)
    {
        var identifierToken = SyntaxFactory.Identifier(identifier);
        var typeArgumentListSyntax = SyntaxFactory.TypeArgumentList();
        var syntax = SyntaxFactory.GenericName(identifierToken, typeArgumentListSyntax);
        var builder = new GenericNameBuilder(syntax);
        genericNameCallback(builder);
        return builder.Syntax;
    }

    public IGenericNameBuilder AddTypeArgument(Action<ITypeBuilder> argumentCallback)
    {
        var argument = TypeBuilder.CreateSyntax(argumentCallback);
        Syntax = Syntax.AddTypeArgumentListArguments(argument);
        return this;
    }

    public IGenericNameBuilder AddTypeArgument(TypeSyntax argument)
    {
        Syntax = Syntax.AddTypeArgumentListArguments(argument);
        return this;
    }
}