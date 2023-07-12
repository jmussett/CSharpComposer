namespace CSharpComposer;

public static class NamespaceDeclarationBuilderExtensions
{
    public static TBuilder AddClassDeclaration<TBuilder>(this TBuilder builder, string identifier, Action<IClassDeclarationBuilder> classDeclarationCallback)
        where TBuilder : IBaseNamespaceDeclarationBuilder<TBuilder>
    {
        return builder.AddMemberDeclaration(x => x.AsClassDeclaration(identifier, classDeclarationCallback));
    }

    public static TBuilder AddStructDeclaration<TBuilder>(this TBuilder builder, string identifier, Action<IStructDeclarationBuilder> structDeclarationCallback)
        where TBuilder : IBaseNamespaceDeclarationBuilder<TBuilder>
    {
        return builder.AddMemberDeclaration(x => x.AsStructDeclaration(identifier, structDeclarationCallback));
    }

    public static TBuilder AddInterfaceDeclaration<TBuilder>(this TBuilder builder, string identifier, Action<IInterfaceDeclarationBuilder> interfaceDeclarationCallback)
        where TBuilder : IBaseNamespaceDeclarationBuilder<TBuilder>
    {
        return builder.AddMemberDeclaration(x => x.AsInterfaceDeclaration(identifier, interfaceDeclarationCallback));
    }

    public static TBuilder AddEnumDeclaration<TBuilder>(this TBuilder builder, string identifier, Action<IEnumDeclarationBuilder> enumDeclarationCallback)
        where TBuilder : IBaseNamespaceDeclarationBuilder<TBuilder>
    {
        return builder.AddMemberDeclaration(x => x.AsEnumDeclaration(identifier, enumDeclarationCallback));
    }

    public static TBuilder AddDelegateDeclaration<TBuilder>(this TBuilder builder, Action<ITypeBuilder> returnTypeCallback, string identifier, Action<IDelegateDeclarationBuilder> delegateDeclarationCallback)
        where TBuilder : IBaseNamespaceDeclarationBuilder<TBuilder>
    {
        return builder.AddMemberDeclaration(x => x.AsDelegateDeclaration(returnTypeCallback, identifier, delegateDeclarationCallback));
    }

    public static TBuilder AddDelegateDeclaration<TBuilder>(this TBuilder builder, string returnTypeName, string identifier, Action<IDelegateDeclarationBuilder> delegateDeclarationCallback)
        where TBuilder : IBaseNamespaceDeclarationBuilder<TBuilder>
    {
        return builder.AddMemberDeclaration(x => x.AsDelegateDeclaration(x => x.ParseTypeName(returnTypeName), identifier, delegateDeclarationCallback));
    }

    public static INamespaceDeclarationBuilder AddNamespaceDeclaration(this INamespaceDeclarationBuilder builder, Action<INameBuilder> nameCallback, Action<INamespaceDeclarationBuilder> delegateDeclarationCallback)
    {
        return builder.AddMemberDeclaration(x => x.AsNamespaceDeclaration(nameCallback, delegateDeclarationCallback));
    }

    public static INamespaceDeclarationBuilder AddNamespaceDeclaration(this INamespaceDeclarationBuilder builder, string name, Action<INamespaceDeclarationBuilder> delegateDeclarationCallback)
    {
        return builder.AddMemberDeclaration(x => x.AsNamespaceDeclaration(x => x.ParseName(name), delegateDeclarationCallback));
    }
}
