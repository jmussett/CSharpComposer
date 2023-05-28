using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SyntaxBuilder.Builders;

public interface IBaseNamespaceDeclarationBuilder : IBaseNamespaceDeclarationBuilder<IBaseNamespaceDeclarationBuilder>
{

}

public interface IBaseNamespaceDeclarationBuilder<TNamespaceBuilder>
{
    TNamespaceBuilder WithClass(string identifier, Func<IClassDeclarationBuilder, IClassDeclarationBuilder> classCallback);
    TNamespaceBuilder WithDelegate(string identifier, Func<ITypeBuilder, ITypeBuilder> typeCallback, Func<IDelegateDeclarationBuilder, IDelegateDeclarationBuilder> delegateCallback);
    TNamespaceBuilder WithEnum(string identifier, Func<IEnumDeclarationBuilder, IEnumDeclarationBuilder> enumCallback, Func<ITypeBuilder, ITypeBuilder>? typeCallback = null);
    TNamespaceBuilder WithInterface(string identifier, Func<IInterfaceDeclarationBuilder, IInterfaceDeclarationBuilder> interfaceCallback);
    TNamespaceBuilder WithStruct(string identifier, Func<IStructDeclarationBuilder, IStructDeclarationBuilder> structCallback);
}

public sealed class BaseNamespaceDeclarationBuilder : IBaseNamespaceDeclarationBuilder
{
    public BaseNamespaceDeclarationSyntax Syntax { get; set; }

    public BaseNamespaceDeclarationBuilder(BaseNamespaceDeclarationSyntax syntax)
    {
        Syntax = syntax;
    }

    public IBaseNamespaceDeclarationBuilder WithClass(string identifier, Func<IClassDeclarationBuilder, IClassDeclarationBuilder> classCallback)
    {
        var classSyntax = ClassDeclarationBuilder.CreateSyntax(identifier, classCallback);

        Syntax = Syntax.AddMembers(classSyntax);

        return this;
    }

    public IBaseNamespaceDeclarationBuilder WithEnum(string identifier, Func<IEnumDeclarationBuilder, IEnumDeclarationBuilder> enumCallback, Func<ITypeBuilder, ITypeBuilder>? typeCallback = null)
    {
        var enumSyntax = EnumDeclarationBuilder.CreateSyntax(identifier, enumCallback, typeCallback);

        Syntax = Syntax.AddMembers(enumSyntax);

        return this;
    }

    public IBaseNamespaceDeclarationBuilder WithInterface(string identifier, Func<IInterfaceDeclarationBuilder, IInterfaceDeclarationBuilder> interfaceCallback)
    {
        var interfaceSyntax = InterfaceDeclarationBuilder.CreateSyntax(identifier, interfaceCallback);

        Syntax = Syntax.AddMembers(interfaceSyntax);

        return this;
    }

    public IBaseNamespaceDeclarationBuilder WithStruct(string identifier, Func<IStructDeclarationBuilder, IStructDeclarationBuilder> structCallback)
    {
        var structSyntax = StructDeclarationBuilder.CreateSyntax(identifier, structCallback);

        Syntax = Syntax.AddMembers(structSyntax);

        return this;
    }

    public IBaseNamespaceDeclarationBuilder WithDelegate(
        string identifier,
        Func<ITypeBuilder, ITypeBuilder> typeCallback,
        Func<IDelegateDeclarationBuilder, IDelegateDeclarationBuilder> delegateCallback
    )
    {
        var delegateSyntax = DelegateDeclarationBuilder.CreateSyntax(identifier, typeCallback, delegateCallback);

        Syntax = Syntax.AddMembers(delegateSyntax);

        return this;
    }
}
