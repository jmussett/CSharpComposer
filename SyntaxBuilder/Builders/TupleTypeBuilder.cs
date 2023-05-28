using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SyntaxBuilder.Validators;

namespace SyntaxBuilder.Builders;

public interface ITupleTypeBuilder
{
    ITupleTypeBuilder WithTypeArgument(Func<ITypeBuilder, ITypeBuilder> typeCallback);
    ITupleTypeBuilder WithTypeArgument(string identifier, Func<ITypeBuilder, ITypeBuilder> typeCallback);
}

public sealed class TupleTypeBuilder : ITupleTypeBuilder
{
    public TupleTypeSyntax Syntax { get; set; }

    public TupleTypeBuilder(TupleTypeSyntax syntax)
    {
        Syntax = syntax;
    }

    public static TupleTypeSyntax CreateSyntax(Func<ITupleTypeBuilder, ITupleTypeBuilder> tupleCallback)
    {
        var syntax = SyntaxFactory.TupleType();

        var builder = new TupleTypeBuilder(syntax).AssertInvoke(tupleCallback);

        return builder.Syntax;
    }

    public ITupleTypeBuilder WithTypeArgument(Func<ITypeBuilder, ITypeBuilder> typeCallback)
    {
        var typeSyntax = TypeBuilder.CreateSyntax(typeCallback);

        var tupleElementSyntax = SyntaxFactory.TupleElement(typeSyntax);

        Syntax = Syntax.AddElements(tupleElementSyntax);

        return this;
    }

    public ITupleTypeBuilder WithTypeArgument(string identifier, Func<ITypeBuilder, ITypeBuilder> typeCallback)
    {
        SyntaxValidator.ValidateIdentifier(identifier);

        var typeSyntax = TypeBuilder.CreateSyntax(typeCallback);

        var tupleElementSyntax = SyntaxFactory.TupleElement(
            typeSyntax,
            SyntaxFactory.Identifier(identifier)
        );

        Syntax = Syntax.AddElements(tupleElementSyntax);

        return this;
    }
}

public static class TupleTypeBuilderExtensions
{

    public static ITupleTypeBuilder WithTypeArgument<T>(this ITupleTypeBuilder builder)
    {
        return builder.WithTypeArgument(x => x.AsType<T>());
    }

    public static ITupleTypeBuilder WithTypeArgument<T>(this ITupleTypeBuilder builder, string identifier)
    {
        return builder.WithTypeArgument(identifier, x => x.AsType<T>());
    }

    public static ITupleTypeBuilder WithTypeArgument(this ITupleTypeBuilder builder, Type type)
    {
        return builder.WithTypeArgument(x => x.AsType(type));
    }

    public static ITupleTypeBuilder WithTypeArgument(this ITupleTypeBuilder builder, string identifier, Type type)
    {
        return builder.WithTypeArgument(identifier, x => x.AsType(type));
    }

    public static ITupleTypeBuilder WithTypeArgument(this ITupleTypeBuilder builder, string typeName)
    {
        return builder.WithTypeArgument(x => x.AsType(typeName));
    }

    public static ITupleTypeBuilder WithTypeArgument(this ITupleTypeBuilder builder, string identifier, string typeName)
    {
        return builder.WithTypeArgument(identifier, x => x.AsType(typeName));
    }
}