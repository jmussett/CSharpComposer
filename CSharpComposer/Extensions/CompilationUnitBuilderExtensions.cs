using System;

namespace CSharpComposer;

public static class CompilationUnitBuilderExtensions
{
    public static ICompilationUnitBuilder AddClassDeclaration(this ICompilationUnitBuilder builder, string identifier, Action<IClassDeclarationBuilder> classDeclarationCallback)
    {
        return builder.AddMemberDeclaration(x => x.AsClassDeclaration(identifier, classDeclarationCallback));
    }


    public static ICompilationUnitBuilder AddStructDeclaration(this ICompilationUnitBuilder builder, string identifier, Action<IStructDeclarationBuilder> structDeclarationCallback)
    {
        return builder.AddMemberDeclaration(x => x.AsStructDeclaration(identifier, structDeclarationCallback));
    }

    public static ICompilationUnitBuilder AddInterfaceDeclaration(this ICompilationUnitBuilder builder, string identifier, Action<IInterfaceDeclarationBuilder> interfaceDeclarationCallback)
    {
        return builder.AddMemberDeclaration(x => x.AsInterfaceDeclaration(identifier, interfaceDeclarationCallback));
    }

    public static ICompilationUnitBuilder AddEnumDeclaration(this ICompilationUnitBuilder builder, string identifier, Action<IEnumDeclarationBuilder> enumDeclarationCallback)
    {
        return builder.AddMemberDeclaration(x => x.AsEnumDeclaration(identifier, enumDeclarationCallback));
    }

    public static ICompilationUnitBuilder AddDelegateDeclaration(this ICompilationUnitBuilder builder, Action<ITypeBuilder> returnTypeCallback, string identifier, Action<IDelegateDeclarationBuilder> delegateDeclarationCallback)
    {
        return builder.AddMemberDeclaration(x => x.AsDelegateDeclaration(returnTypeCallback, identifier, delegateDeclarationCallback));
    }

    public static ICompilationUnitBuilder AddDelegateDeclaration(this ICompilationUnitBuilder builder, string returnTypeName, string identifier, Action<IDelegateDeclarationBuilder> delegateDeclarationCallback)
    {
        return builder.AddMemberDeclaration(x => x.AsDelegateDeclaration(x => x.ParseTypeName(returnTypeName), identifier, delegateDeclarationCallback));
    }

    public static ICompilationUnitBuilder AddNamespaceDeclaration(this ICompilationUnitBuilder builder, Action<INameBuilder> nameCallback, Action<INamespaceDeclarationBuilder> delegateDeclarationCallback)
    {
        return builder.AddMemberDeclaration(x => x.AsNamespaceDeclaration(nameCallback, delegateDeclarationCallback));
    }

    public static ICompilationUnitBuilder AddNamespaceDeclaration(this ICompilationUnitBuilder builder, string name, Action<INamespaceDeclarationBuilder> delegateDeclarationCallback)
    {
        return builder.AddMemberDeclaration(x => x.AsNamespaceDeclaration(x => x.ParseName(name), delegateDeclarationCallback));
    }

    public static ICompilationUnitBuilder AddFileScopedNamespaceDeclaration(this ICompilationUnitBuilder builder, Action<INameBuilder> nameCallback, Action<IFileScopedNamespaceDeclarationBuilder> delegateDeclarationCallback)
    {
        return builder.AddMemberDeclaration(x => x.AsFileScopedNamespaceDeclaration(nameCallback, delegateDeclarationCallback));
    }

    public static ICompilationUnitBuilder AddFileScopedNamespaceDeclaration(this ICompilationUnitBuilder builder, string name, Action<IFileScopedNamespaceDeclarationBuilder> delegateDeclarationCallback)
    {
        return builder.AddMemberDeclaration(x => x.AsFileScopedNamespaceDeclaration(x => x.ParseName(name), delegateDeclarationCallback));
    }

    public static ICompilationUnitBuilder AddUsingDirective(this ICompilationUnitBuilder builder, string name, Action<IUsingDirectiveBuilder>? usingDirectiveCallback = null)
    {
        return builder.AddUsingDirective(x => x.ParseName(name), usingDirectiveCallback);
    }

    public static ICompilationUnitBuilder AddMemberDeclaration(this ICompilationUnitBuilder builder, string member)
    {
        return builder.AddMemberDeclaration(x => x.ParseMemberDeclaration(member));
    }
}
