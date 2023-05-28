namespace SyntaxBuilder.Validators;

internal class TypeValidator
{
    public static void ValidateAttributeForTarget(Type type, AttributeTargets target)
    {
        if (!type.IsAssignableTo(typeof(Attribute)))
        {
            throw new InvalidOperationException($"Type '{type.Name}' is not an Attribute.");
        }

        // Get the attribute usage for the attribute type T, or assume default usage if not specified
        var attributeUsage = Attribute.GetCustomAttribute(type, typeof(AttributeUsageAttribute))
            as AttributeUsageAttribute ?? new AttributeUsageAttribute(AttributeTargets.All);

        // Check if the attribute can be used on the desired target
        if (!attributeUsage.ValidOn.HasFlag(target))
        {
            throw new InvalidOperationException($"Attribute of type '{type.Name}' cannot be used on target '{target}'");
        }
    }
}
