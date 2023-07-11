using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IGenericNameBuilder : IAddType<IGenericNameBuilder>
{
}

public partial class GenericNameBuilder : IGenericNameBuilder
{
    public GenericNameSyntax Syntax { get; set; }

    public GenericNameBuilder(GenericNameSyntax syntax)
    {
        Syntax = syntax;
    }

    public static GenericNameSyntax CreateSyntax(string identifier, Action<IGenericNameBuilder>? genericNameCallback = null)
    {
        var identifierToken = SyntaxFactory.Identifier(identifier);
        var typeArgumentListSyntax = SyntaxFactory.TypeArgumentList();
        var syntax = SyntaxFactory.GenericName(identifierToken, typeArgumentListSyntax);
        var builder = new GenericNameBuilder(syntax);
        genericNameCallback?.Invoke(builder);
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