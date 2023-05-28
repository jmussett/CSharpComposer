using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SyntaxBuilder.Builders;

public interface INamespaceDeclarationBuilder : IBaseNamespaceDeclarationBuilder<INamespaceDeclarationBuilder>
{
    INamespaceDeclarationBuilder WithNamespace(Func<INameBuilder, INameBuilder> nameBuilder, Func<INamespaceDeclarationBuilder, INamespaceDeclarationBuilder> namespaceCallback);
}

public class NamespaceDeclarationBuilder : INamespaceDeclarationBuilder
{
    private BaseNamespaceDeclarationBuilder _baseNamespaceBuilder;

    public NamespaceDeclarationSyntax Syntax
    {
        get => (NamespaceDeclarationSyntax)_baseNamespaceBuilder.Syntax;
        set => _baseNamespaceBuilder.Syntax = value;
    }

    public NamespaceDeclarationBuilder(NamespaceDeclarationSyntax syntax)
    {
        _baseNamespaceBuilder = new(syntax);
    }

    public static NamespaceDeclarationSyntax CreateSyntax(Func<INameBuilder, INameBuilder> nameCallback, Func<INamespaceDeclarationBuilder, INamespaceDeclarationBuilder> namespaceCallback)
    {
        var nameSyntax = NameBuilder.CreateSyntax(nameCallback);

        var syntax = SyntaxFactory.NamespaceDeclaration(nameSyntax);

        var builder = new NamespaceDeclarationBuilder(syntax).AssertInvoke(namespaceCallback);

        return builder.Syntax;
    }

    public INamespaceDeclarationBuilder WithNamespace(Func<INameBuilder, INameBuilder> nameBuilder, Func<INamespaceDeclarationBuilder, INamespaceDeclarationBuilder> namespaceCallback)
    {
        var syntax = CreateSyntax(nameBuilder, namespaceCallback);

        Syntax = Syntax.AddMembers(syntax);

        return this;
    }

    public INamespaceDeclarationBuilder WithClass(string identifier, Func<IClassDeclarationBuilder, IClassDeclarationBuilder> classCallback)
    {
        _baseNamespaceBuilder = _baseNamespaceBuilder.AssertInvoke(x => x.WithClass(identifier, classCallback));
        return this;
    }

    public INamespaceDeclarationBuilder WithDelegate(string identifier, Func<ITypeBuilder, ITypeBuilder> typeCallback, Func<IDelegateDeclarationBuilder, IDelegateDeclarationBuilder> delegateCallback)
    {
        _baseNamespaceBuilder = _baseNamespaceBuilder.AssertInvoke(x => x.WithDelegate(identifier, typeCallback, delegateCallback));
        return this;
    }

    public INamespaceDeclarationBuilder WithEnum(string identifier, Func<IEnumDeclarationBuilder, IEnumDeclarationBuilder> enumCallback, Func<ITypeBuilder, ITypeBuilder>? typeCallbackck = null)
    {
        _baseNamespaceBuilder = _baseNamespaceBuilder.AssertInvoke(x => x.WithEnum(identifier, enumCallback, typeCallbackck));
        return this;
    }

    public INamespaceDeclarationBuilder WithInterface(string identifier, Func<IInterfaceDeclarationBuilder, IInterfaceDeclarationBuilder> interfaceCallback)
    {
        _baseNamespaceBuilder = _baseNamespaceBuilder.AssertInvoke(x => x.WithInterface(identifier, interfaceCallback));
        return this;
    }

    public INamespaceDeclarationBuilder WithStruct(string identifier, Func<IStructDeclarationBuilder, IStructDeclarationBuilder> structCallback)
    {
        _baseNamespaceBuilder = _baseNamespaceBuilder.AssertInvoke(x => x.WithStruct(identifier, structCallback));
        return this;
    }
}

public static class NamespaceBuilderExtensions
{
    public static INamespaceDeclarationBuilder WithDelegate<T>(
        this INamespaceDeclarationBuilder builder,
        string identifier,
        Func<IDelegateDeclarationBuilder, IDelegateDeclarationBuilder> delegateCallback
    )
    {
        return builder.WithDelegate(identifier, x => x.AsType<T>(), delegateCallback);
    }

    public static INamespaceDeclarationBuilder WithDelegate(
        this INamespaceDeclarationBuilder builder,
        string identifier,
        Func<IDelegateDeclarationBuilder, IDelegateDeclarationBuilder> delegateCallback
    )
    {
        return builder.WithDelegate(identifier, x => x.AsVoid(), delegateCallback);
    }

}
