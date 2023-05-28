using Microsoft.CodeAnalysis;
using CSharpComposer.Generator.Models;
using System.Diagnostics.CodeAnalysis;

namespace CSharpComposer.Generator;

internal static class NodeValidator
{

    public static bool IsAnyList([NotNullWhen(true)] string? typeName)
    {
        return IsAnyNodeList(typeName) || typeName == "SyntaxNodeOrTokenList" || IsConcreteList(typeName);
    }

    public static bool IsConcreteList([NotNullWhen(true)] string? typeName)
    {
        return typeName?.EndsWith("ListSyntax") ?? false;
    }

    public static bool IsAnyNodeList([NotNullWhen(true)] string? typeName)
    {
        return IsNodeList(typeName) || IsSeparatedNodeList(typeName);
    }

    public static bool IsNodeList([NotNullWhen(true)] string? typeName)
    {
        return typeName?.StartsWith("SyntaxList<", StringComparison.Ordinal) ?? false;
    }

    public static bool IsSeparatedNodeList([NotNullWhen(true)] string? typeName)
    {
        return typeName?.StartsWith("SeparatedSyntaxList<", StringComparison.Ordinal) ?? false;
    }

    public static bool IsSyntaxNode([NotNullWhen(true)] string? typeName)
    {
        return typeName?.EndsWith("Syntax") ?? false;
    }

    public static bool IsSyntaxToken([NotNullWhen(true)] string? typeName)
    {
        return typeName == "SyntaxToken";
    }

    public static bool IsValidNode([NotNullWhen(true)] string? nodeName)
    {
        return nodeName is not null
            && nodeName != "SyntaxNode"
            && nodeName != "CSharpSyntaxNode"
            && nodeName != "SyntaxToken"
            && nodeName != "StructuredTriviaSyntax"
            && !nodeName.EndsWith("ListSyntax");
    }

    public static bool IsValidLiteral(KindBase kind)
    {
        return kind.Name == "NumericLiteralToken"
            || kind.Name == "StringLiteralToken"
            || kind.Name == "CharacterLiteralToken"
            || kind.Name == "TrueKeyword"
            || kind.Name == "FalseKeyword"
            || kind.Name == "NullKeyword"
            || kind.Name == "DefaultKeyword";
    }

    public static bool IsRoot(Node n)
    {
        return n.Root != null && string.Compare(n.Root, "true", true) == 0;
    }

    public static bool IsTrue([NotNullWhen(true)] string? val)
        => val != null && string.Compare(val, "true", true) == 0;

    public static bool IsOptional(Field f)
        => IsTrue(f.Optional);

    public static bool IsOverride(Field f)
        => IsTrue(f.Override);

    public static bool IsNew(Field f)
        => IsTrue(f.New);

    public static bool HasOptionalChildren(TreeType type)
    {
        return type.Children.Any(child =>
            child is Choice ||
            (
                child is Field field &&
                (field.IsOptional || IsAnyList(field.Type))
            )
        );
    }

    public static bool HasMandatoryChildren(TreeType type)
    {
        return type.Children.Any(child =>
            child is Field field &&
            !field.IsOptional &&
            !IsAnyList(field.Type)
        );
    }

    // If we have multiple kinds with a coresponding "Token" field, it's likely that the node can be tokenized and will therefor require dedicated casting methods.
    // i.e: LiteralExpressionSyntax.
    public static bool IsTokenized(TreeType type)
    {
        return type is Node node && 
            node.Kinds.Count > 1 && 
            node.Children.OfType<Field>()
                .Any(x => x.Name == "Token");
    }
}