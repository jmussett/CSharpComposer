using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IGenericNameBuilder : IAddType<IGenericNameBuilder>
{
}

public interface IWithGenericName<TBuilder>
{
    TBuilder WithGenericName(string identifier, Action<IGenericNameBuilder> genericNameCallback);
    TBuilder WithGenericName(GenericNameSyntax genericNameSyntax);
}

public interface IAddGenericName<TBuilder>
{
    TBuilder AddGenericName(string identifier, Action<IGenericNameBuilder> genericNameCallback);
    TBuilder AddGenericName(GenericNameSyntax genericNameSyntax);
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

    public IGenericNameBuilder AddType(Action<ITypeBuilder> argumentCallback)
    {
        var argument = TypeBuilder.CreateSyntax(argumentCallback);
        Syntax = Syntax.AddTypeArgumentListArguments(argument);
        return this;
    }

    public IGenericNameBuilder AddType(TypeSyntax argument)
    {
        Syntax = Syntax.AddTypeArgumentListArguments(argument);
        return this;
    }
}