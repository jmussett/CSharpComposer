using CSharpComposer.Generator.Models;

namespace CSharpComposer.Generator;

internal class EnumStore
{
    public Dictionary<string, Field> FieldEnums { get; } = new();
    public Dictionary<string, Node> KindEnums { get; } = new();

    public bool TryAddEnum(string enumName, Field field)
    {
        return FieldEnums.TryAdd(enumName, field);
    }

    public bool TryAddEnum(string enumName, Node node)
    {
        return KindEnums.TryAdd(enumName, node);
    }
}
