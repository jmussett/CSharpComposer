namespace CSharpComposer.Generator.Models;

public class ContextualKind : KindBase, IEquatable<ContextualKind>
{
    public override bool Equals(object? obj)
        => Equals(obj as ContextualKind);

    public bool Equals(ContextualKind? other)
        => Name == other?.Name;

    public override int GetHashCode()
        => Name == null ? 0 : Name.GetHashCode();
}

