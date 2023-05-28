using System.Xml.Serialization;

namespace CSharpComposer.Generator.Models;

public class Kind : KindBase, IEquatable<Kind>
{
    public override bool Equals(object? obj)
        => Equals(obj as Kind);

    public bool Equals(Kind? other)
        => Name == other?.Name;

    public override int GetHashCode()
        => Name == null ? 0 : Name.GetHashCode();
}
