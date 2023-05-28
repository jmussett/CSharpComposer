using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SyntaxBuilder.Builders;

public interface ISimpleNameBuilder : ISimpleNameBuilder<ISimpleNameBuilder>
{

}

public interface ISimpleNameBuilder<TBuilder>
{
    TBuilder AsGeneric(string identifier, Func<IGenericNameBuilder, IGenericNameBuilder> genericCallback);
    TBuilder AsIdentifier(string identifier);
}

public sealed class SimpleNameBuilder : ISimpleNameBuilder
{
    private NameBuilder _nameBuilder = new();

    public SimpleNameSyntax? Syntax
    {
        get => (SimpleNameSyntax?)_nameBuilder.Syntax;
        set => _nameBuilder.Syntax = value;
    }

    public static SimpleNameSyntax CreateSyntax(Func<ISimpleNameBuilder, ISimpleNameBuilder> nameCallback)
    {
        var builder = new SimpleNameBuilder().AssertInvoke(nameCallback);

        if (builder.Syntax is null)
        {
            throw new InvalidOperationException("Simple name syntax has not been specified.");
        }

        return builder.Syntax;
    }

    public ISimpleNameBuilder AsGeneric(string identifier, Func<IGenericNameBuilder, IGenericNameBuilder> genericCallback)
    {
        _nameBuilder = _nameBuilder.AssertInvoke(x => x.AsGeneric(identifier, genericCallback));

        return this;
    }

    public ISimpleNameBuilder AsIdentifier(string identifier)
    {
        _nameBuilder = _nameBuilder.AssertInvoke(x => x.AsIdentifier(identifier));

        return this;
    }
}
