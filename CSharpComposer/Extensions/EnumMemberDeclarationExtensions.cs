namespace CSharpComposer;

public static class EnumMemberDeclarationExtensions
{
    public static IEnumMemberDeclarationBuilder WithEqualsValue(this IEnumMemberDeclarationBuilder builder, string value)
    {
        return builder.WithEqualsValue(x => x.ParseExpression(value));
    }
}
