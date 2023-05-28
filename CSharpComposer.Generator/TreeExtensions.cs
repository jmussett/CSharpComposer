using CSharpComposer.Generator.Models;
using System.Diagnostics.CodeAnalysis;

namespace CSharpComposer.Generator;

public static class NodeExtensions
{
    public static bool AnyValidFieldMethod(this Tree tree, TreeType type, TreeTypeChild child, bool isImplementation)
    {
        if (child is Choice choice)
        {
            return choice.Children.Any(x => tree.AnyValidFieldMethod(type, x, isImplementation));
        }

        if (child is Sequence sequence)
        {
            return sequence.Children.Any(x => tree.AnyValidFieldMethod(type, x, isImplementation));
        }

        if (child is Field field)
        {
            return tree.IsValidFieldMethod(type, field, isImplementation, false);
        }

        return false;
    }

    public static bool IsValidFieldMethod(this Tree tree, TreeType type, Field field, bool isImplementation, bool isChoice)
    {
        // TODO: Also ignore if optional and parent field is part of a choice?
        if (!isChoice && !field.IsOptional && !NodeValidator.IsAnyList(field.Type))
        {
            return false;
        }

        if (!isImplementation && type is AbstractNode)
        {
            var derivedFields = tree.GetDerivedFields(type, field);

            if (derivedFields.Any(x => !x.IsOptional))
            {
                return false;
            }
        }

        return true;
    }

    public static IEnumerable<Field> GetDerivedFields(this Tree tree, TreeType type, Field field)
    {
        var baseTypes = tree.Types
            .Where(x => x.Base == type.Name);

        foreach (var baseType in baseTypes)
        {
            var derivedBaseFields = tree.GetDerivedFields(baseType, field);

            var children = baseType.Children.GetNestedChildren();

            foreach (var baseField in children.Concat(derivedBaseFields))
            {
                if (baseField.Name == field.Name)
                {
                    yield return baseField;
                }
            }
        }
    }

    public static IEnumerable<Field> GetNestedChildren(this List<TreeTypeChild> children)
    {
        var fields = children.OfType<Field>();
        var sequences = children.OfType<Sequence>();
        var choices = children.OfType<Choice>();

        foreach (var sequence in sequences)
        {
            fields = fields.Concat(GetNestedChildren(sequence.Children));
        }

        foreach (var choice in choices)
        {
            fields = fields.Concat(GetNestedChildren(choice.Children));
        }

        return fields;
    }

    public static bool TryGetBaseField(this Tree tree, TreeType type, Field field, [NotNullWhen(true)] out TreeType? baseType, [NotNullWhen(true)] out Field? baseField)
    {
        baseField = null;
        baseType = tree.Types.FirstOrDefault(x => x.Name == type.Base);

        if (baseType is not null)
        {
            var children = baseType.Children.GetNestedChildren();
            baseField = children.FirstOrDefault(x => x.Name == field.Name);

            if (baseField is null || baseField.IsOverride)
            {
                return baseType is not null && tree.TryGetBaseField(baseType, field, out baseType, out baseField);
            }
        }

        return false;
    }

    public static Field? GetReferenceListType(this Tree tree, string type)
    {
        var referenceTypeNode = tree.Types.FirstOrDefault(x => x.Name == type);

        if (referenceTypeNode is not null)
        {
            foreach (var referencedNodeField in referenceTypeNode.Children.OfType<Field>())
            {
                if (NodeValidator.IsAnyNodeList(referencedNodeField.Type))
                {
                    return referencedNodeField;
                }
            }
        }

        return null;
    }
}
