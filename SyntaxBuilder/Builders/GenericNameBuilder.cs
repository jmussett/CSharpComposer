using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SyntaxBuilder.Validators;

namespace SyntaxBuilder.Builders;

public interface IGenericNameBuilder
{
    IGenericNameBuilder WithTypeArgument(Func<ITypeBuilder, ITypeBuilder> typeCallback);
    IGenericNameBuilder WithTypeArgument(Func<INameBuilder, INameBuilder> nameCallback);
    IGenericNameBuilder WithTypeArgument(Type type);
}

public sealed class GenericNameBuilder : IGenericNameBuilder
{
    public GenericNameSyntax Syntax { get; set; }

    public GenericNameBuilder(GenericNameSyntax syntax)
    {
        Syntax = syntax;
    }

    public static GenericNameSyntax CreateSyntax(string identifier, Func<IGenericNameBuilder, IGenericNameBuilder> genericNameCallback)
    {
        SyntaxValidator.ValidateIdentifier(identifier);

        var syntax = SyntaxFactory.GenericName(identifier);

        var builder = new GenericNameBuilder(syntax).AssertInvoke(genericNameCallback);

        return builder.Syntax;
    }

    public IGenericNameBuilder WithTypeArgument(Type type)
    {
        var typeSyntax = TypeBuilder.CreateSyntax(type);

        Syntax = Syntax.AddTypeArgumentListArguments(typeSyntax);

        return this;
    }

    // TODO: create name extension
    public IGenericNameBuilder WithTypeArgument(Func<INameBuilder, INameBuilder> nameBuilder)
    {
        var typeSyntax = NameBuilder.CreateSyntax(nameBuilder);

        Syntax = Syntax.AddTypeArgumentListArguments(typeSyntax);

        return this;
    }

    public IGenericNameBuilder WithTypeArgument(Func<ITypeBuilder, ITypeBuilder> typeCallback)
    {
        var typeSyntax = TypeBuilder.CreateSyntax(typeCallback);

        Syntax = Syntax.AddTypeArgumentListArguments(typeSyntax);

        return this;
    }
}

public static class GenericNameBuilderExtensions
{
    public static IGenericNameBuilder WithTypeArgument<T>(this IGenericNameBuilder builder)
    {
        return builder.WithTypeArgument(typeof(T));
    }
}
