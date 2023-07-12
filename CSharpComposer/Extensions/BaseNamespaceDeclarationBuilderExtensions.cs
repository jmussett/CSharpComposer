namespace CSharpComposer;

public static class BaseNamespaceDeclarationBuilderExtensions
{
    public static TBuilder AddMemberDeclaration<TBuilder>(this TBuilder builder, string member)
        where TBuilder : IBaseNamespaceDeclarationBuilder<TBuilder>
    {
        return builder.AddMemberDeclaration(x => x.ParseMemberDeclaration(member));
    }

    public static TBuilder AddUsingDirective<TBuilder>(this TBuilder builder, string name, Action<IUsingDirectiveBuilder>? usingDirectiveCallback = null)
        where TBuilder : IBaseNamespaceDeclarationBuilder<TBuilder>
    {
        return builder.AddUsingDirective(x => x.ParseName(name), usingDirectiveCallback);
    }
}
