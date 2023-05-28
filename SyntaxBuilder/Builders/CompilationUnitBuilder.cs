using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SyntaxBuilder.Builders;

public interface ICompilationUnitBuilder
{
    ICompilationUnitBuilder WithUsing(Func<INameBuilder, INameBuilder> nameCallback);
    ICompilationUnitBuilder WithFileScopedNamespace(Func<INameBuilder, INameBuilder> nameCallback, Func<IFileScopedNamespaceDeclarationBuilder, IFileScopedNamespaceDeclarationBuilder> namespaceCallback);
    ICompilationUnitBuilder WithNamespace(Func<INameBuilder, INameBuilder> nameCallback, Func<INamespaceDeclarationBuilder, INamespaceDeclarationBuilder> namespaceCallback);
}

public sealed class CompilationUnitBuilder : ICompilationUnitBuilder
{
    public CompilationUnitSyntax Syntax { get; set; }

    public CompilationUnitBuilder(CompilationUnitSyntax syntax)
    {
        Syntax = syntax;
    }

    public static CompilationUnitSyntax CreateSyntax(Func<ICompilationUnitBuilder, ICompilationUnitBuilder> compilationUnitCallback)
    {
        var syntax = SyntaxFactory.CompilationUnit();

        var builder = new CompilationUnitBuilder(syntax).AssertInvoke(compilationUnitCallback);

        return builder.Syntax.NormalizeWhitespace();
    }

    public static CompilationUnitSyntax ParseCompilationUnit(string compilationUnit)
    {
        return SyntaxFactory.ParseCompilationUnit(compilationUnit).NormalizeWhitespace();
    }

    public ICompilationUnitBuilder WithFileScopedNamespace(Func<INameBuilder, INameBuilder> nameCallback, Func<IFileScopedNamespaceDeclarationBuilder, IFileScopedNamespaceDeclarationBuilder> namespaceCallback)
    {
        var syntax = FileScopedNamespaceDeclarationBuilder.CreateSyntax(nameCallback, namespaceCallback);

        Syntax = Syntax.AddMembers(syntax);

        return this;
    }

    public ICompilationUnitBuilder WithNamespace(Func<INameBuilder, INameBuilder> nameCallback, Func<INamespaceDeclarationBuilder, INamespaceDeclarationBuilder> namespaceCallback)
    {
        var syntax = NamespaceDeclarationBuilder.CreateSyntax(nameCallback, namespaceCallback);

        Syntax = Syntax.AddMembers(syntax);

        return this;
    }

    public ICompilationUnitBuilder WithUsing(Func<INameBuilder, INameBuilder> nameCallback)
    {
        var nameSyntax = NameBuilder.CreateSyntax(nameCallback);
        var syntax = SyntaxFactory.UsingDirective(nameSyntax);

        Syntax = Syntax.AddUsings(syntax);

        return this;
    }
}

public static class CompilationUnitBuilderExtensions
{
    public static ICompilationUnitBuilder WithFileScopedNamespace(this ICompilationUnitBuilder builder, string namespaceName, Func<IFileScopedNamespaceDeclarationBuilder, IFileScopedNamespaceDeclarationBuilder> namespaceCallback)
    {
        return builder.WithFileScopedNamespace(x => x.ParseName(namespaceName), namespaceCallback);
    }

    public static ICompilationUnitBuilder WithNamespace(this ICompilationUnitBuilder builder, string namespaceName, Func<INamespaceDeclarationBuilder, INamespaceDeclarationBuilder> namespaceCallback)
    {
        return builder.WithNamespace(x => x.ParseName(namespaceName), namespaceCallback);
    }

    public static ICompilationUnitBuilder WithUsing(this ICompilationUnitBuilder builder, string namespaceName)
    {
        return builder.WithUsing(x => x.ParseName(namespaceName));
    }
}
