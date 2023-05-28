using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;

namespace SyntaxBuilder.Builders;

public interface IFileScopedNamespaceDeclarationBuilder : IBaseNamespaceDeclarationBuilder<IFileScopedNamespaceDeclarationBuilder>
{

}

public class FileScopedNamespaceDeclarationBuilder : IFileScopedNamespaceDeclarationBuilder
{
    private BaseNamespaceDeclarationBuilder _baseNamespaceBuilder;

    public FileScopedNamespaceDeclarationSyntax Syntax
    {
        get => (FileScopedNamespaceDeclarationSyntax)_baseNamespaceBuilder.Syntax;
        set => _baseNamespaceBuilder.Syntax = value;
    }

    public FileScopedNamespaceDeclarationBuilder(FileScopedNamespaceDeclarationSyntax syntax)
    {
        _baseNamespaceBuilder = new(syntax);
    }

    public static FileScopedNamespaceDeclarationSyntax CreateSyntax(Func<INameBuilder, INameBuilder> nameCallback, Func<IFileScopedNamespaceDeclarationBuilder, IFileScopedNamespaceDeclarationBuilder> namespaceCallback)
    {
        var nameSyntax = NameBuilder.CreateSyntax(nameCallback);

        var syntax = SyntaxFactory.FileScopedNamespaceDeclaration(nameSyntax);

        var builder = new FileScopedNamespaceDeclarationBuilder(syntax).AssertInvoke(namespaceCallback);

        return builder.Syntax;
    }

    public IFileScopedNamespaceDeclarationBuilder WithClass(string identifier, Func<IClassDeclarationBuilder, IClassDeclarationBuilder> classCallback)
    {
        _baseNamespaceBuilder = _baseNamespaceBuilder.AssertInvoke(x => x.WithClass(identifier, classCallback));
        return this;
    }

    public IFileScopedNamespaceDeclarationBuilder WithDelegate(string identifier, Func<ITypeBuilder, ITypeBuilder> typeCallback, Func<IDelegateDeclarationBuilder, IDelegateDeclarationBuilder> delegateCallback)
    {
        _baseNamespaceBuilder = _baseNamespaceBuilder.AssertInvoke(x => x.WithDelegate(identifier, typeCallback, delegateCallback));
        return this;
    }

    public IFileScopedNamespaceDeclarationBuilder WithEnum(string identifier, Func<IEnumDeclarationBuilder, IEnumDeclarationBuilder> enumCallback, Func<ITypeBuilder, ITypeBuilder>? typeCallback = null)
    {
        _baseNamespaceBuilder = _baseNamespaceBuilder.AssertInvoke(x => x.WithEnum(identifier, enumCallback, typeCallback));
        return this;
    }

    public IFileScopedNamespaceDeclarationBuilder WithInterface(string identifier, Func<IInterfaceDeclarationBuilder, IInterfaceDeclarationBuilder> interfaceCallback)
    {
        _baseNamespaceBuilder = _baseNamespaceBuilder.AssertInvoke(x => x.WithInterface(identifier, interfaceCallback));
        return this;
    }

    public IFileScopedNamespaceDeclarationBuilder WithStruct(string identifier, Func<IStructDeclarationBuilder, IStructDeclarationBuilder> structCallback)
    {
        _baseNamespaceBuilder = _baseNamespaceBuilder.AssertInvoke(x => x.WithStruct(identifier, structCallback));
        return this;
    }
}
