using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SyntaxBuilder.Builders;
using SyntaxBuilder.Validators;

namespace SyntaxBuilder;


public interface INameBuilder : ISimpleNameBuilder<INameBuilder>
{
    INameBuilder ParseName(string name);
    INameBuilder AsQualified(Func<INameBuilder, INameBuilder> nameCallback, Func<ISimpleNameBuilder, ISimpleNameBuilder> simpleNameCallback);
}

// TODO: Do we need to split this up into 3 seperate builders?
public sealed class NameBuilder : INameBuilder
{
    public NameSyntax? Syntax { get; set; }

    public static NameSyntax CreateSyntax(Func<INameBuilder, INameBuilder> nameCallback)
    {
        var builder = new NameBuilder().AssertInvoke(nameCallback);

        if (builder.Syntax is null)
        {
            throw new InvalidOperationException("Name syntax has not been specified.");
        }

        return builder.Syntax;
    }

    public INameBuilder AsIdentifier(string identifier)
    {
        SyntaxValidator.ValidateIdentifier(identifier);

        Syntax = SyntaxFactory.IdentifierName(identifier);

        return this;
    }

    public INameBuilder AsQualified(Func<INameBuilder, INameBuilder> nameCallback, Func<ISimpleNameBuilder, ISimpleNameBuilder> simpleNameCallback)
    {
        var nameSyntax = CreateSyntax(nameCallback);
        var simpleNameSyntax = SimpleNameBuilder.CreateSyntax(simpleNameCallback);

        Syntax = SyntaxFactory.QualifiedName(nameSyntax, simpleNameSyntax);

        return this;
    }

    public INameBuilder ParseName(string name)
    {
        SyntaxValidator.ValidateName(name);

        Syntax = SyntaxFactory.ParseName(name);

        return this;
    }

    public INameBuilder AsGeneric(string identifier, Func<IGenericNameBuilder, IGenericNameBuilder> genericCallback)
    {
        Syntax = GenericNameBuilder.CreateSyntax(identifier, genericCallback);

        return this;
    }
}
