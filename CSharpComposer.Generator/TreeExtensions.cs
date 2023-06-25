using CSharpComposer.Generator.Models;
using System.Diagnostics.CodeAnalysis;
using System.Security.AccessControl;

namespace CSharpComposer.Generator;

public static class NodeExtensions
{
    public static bool AnyValidFieldMethod(this Tree tree, TreeType type, TreeTypeChild child, bool isImplementation, bool isChoice = false)
    {
        if (child is Choice choice)
        {
            return choice.Children.Any(x => tree.AnyValidFieldMethod(type, x, isImplementation, true));
        }

        if (child is Sequence sequence)
        {
            return sequence.Children.Any(x => tree.AnyValidFieldMethod(type, x, isImplementation, isChoice));
        }

        if (child is Field field)
        {
            return tree.IsValidFieldMethod(type, field, isImplementation, isChoice);
        }

        return false;
    }

    public static bool IsValidFieldMethod(this Tree tree, TreeType type, Field field, bool isImplementation, bool isChoice)
    {
        if (tree.TryGetBaseField(type, field, out var baseType, out var baseField, out var isBaseFieldChoice, out var isBaseFieldSequence))
        {
            if (field.IsOptional && isBaseFieldChoice)
            {
                return false;
            }
        }

        if (!isChoice && !field.IsOptional && !NodeValidator.IsAnyList(field.Type))
        {
            return false;
        }

        if (!isImplementation && type is AbstractNode)
        {
            var derivedFields = tree.GetDerivedFields(type, field);

            if (derivedFields.Any(x => !x.IsOptional && !NodeValidator.IsAnyList(x.Type)))
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

            foreach (var baseField in children.Select(x => x.Field).Concat(derivedBaseFields))
            {
                if (baseField.Name == field.Name)
                {
                    yield return baseField;
                }
            }
        }
    }

    public static IEnumerable<(Field Field, bool IsSequence, bool IsChoice)> GetNestedChildren(this List<TreeTypeChild> children)
    {
        var fields = children.OfType<Field>().Select(f => (f, false, false));
        var sequences = children.OfType<Sequence>();
        var choices = children.OfType<Choice>();

        foreach (var sequence in sequences)
        {
            fields = fields.Concat(GetNestedChildren(sequence.Children).Select(f => (f.Field, true, f.IsChoice)));
        }

        foreach (var choice in choices)
        {
            fields = fields.Concat(GetNestedChildren(choice.Children).Select(f => (f.Field, f.IsSequence, true)));
        }

        return fields;
    }

    public static bool TryGetBaseField(
        this Tree tree, 
        TreeType type, 
        Field field, 
        [NotNullWhen(true)] out TreeType? baseType, 
        [NotNullWhen(true)] out Field? baseField,
        out bool isChoice,
        out bool isSequence)
    {
        baseField = null;
        isChoice = false;
        isSequence = false;
        baseType = tree.Types.FirstOrDefault(x => x.Name == type.Base);

        if (baseType is null)
        {
            return false;
        }

        var children = baseType.Children.GetNestedChildren();
        var matchingBaseField = children.FirstOrDefault(x => x.Field.Name == field.Name);

        if (matchingBaseField.Field is not null && !matchingBaseField.Field.IsOverride)
        {
            baseField = matchingBaseField.Field;
            isChoice = matchingBaseField.IsChoice;
            isSequence = matchingBaseField.IsSequence;

            return true;
        }

        return tree.TryGetBaseField(baseType, field, out baseType, out baseField, out isChoice, out isSequence);
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

    public static bool HasOptionalChildren(this Tree tree, string typeName)
    {
        var type = tree.Types.FirstOrDefault(x => x.Name == typeName)
            ?? throw new InvalidOperationException($"Unable to find type '{typeName}'");

        var hasOptionalChildren = type.Children.Any(child =>
            child is Choice ||
            (
                child is Field field &&
                (field.IsOptional || NodeValidator.IsAnyList(field.Type))
            )
        );

        if (!hasOptionalChildren && type.Base is not null && NodeValidator.IsSyntaxNode(type.Base))
        {
            return HasOptionalChildren(tree, type.Base);
        }

        return hasOptionalChildren;
    }
}
