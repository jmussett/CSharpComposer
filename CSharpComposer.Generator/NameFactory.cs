using Humanizer;
using CSharpComposer.Generator.Models;
using System.IO;
using System.Text.RegularExpressions;

namespace CSharpComposer.Generator;

internal static partial class NameFactory
{
    private const int SyntaxLength = 6;

    private static HashSet<string> csharpKeywords = new HashSet<string>
    {
        "abstract", "as", "base", "bool", "break", "byte", "case", "catch", "char", "checked",
        "class", "const", "continue", "decimal", "default", "delegate", "do", "double", "else",
        "enum", "event", "explicit", "extern", "false", "finally", "fixed", "float", "for",
        "foreach", "goto", "if", "implicit", "in", "int", "interface", "internal", "is",
        "lock", "long", "namespace", "new", "null", "object", "operator", "out", "override",
        "params", "private", "protected", "public", "readonly", "ref", "return", "sbyte",
        "sealed", "short", "sizeof", "stackalloc", "static", "string", "struct", "switch",
        "this", "throw", "true", "try", "typeof", "uint", "ulong", "unchecked", "unsafe",
        "ushort", "using", "virtual", "void", "volatile", "while"
    };

    public static string CreateBuilderName(string nodeName)
    {
        return $"{CreateTypeName(nodeName)}Builder";
    }

    public static string CreateTypeName(string nodeName)
    {
        return nodeName.Replace("Syntax", string.Empty);
    }

    [GeneratedRegex("(?<!^)(?=[A-Z])")]
    private static partial Regex WordSplitRegex();

    public static string CreateNameWithoutSuffix(string name)
    {
        // Split the string into words
        var words = WordSplitRegex().Split(name).ToList();

        // If there are no words (i.e., the string was empty or all lowercase), just return the original string
        if (words.Count == 0)
        {
            return name;
        }

        // Remove the last word
        words.RemoveAt(words.Count - 1);

        // Join the remaining words back together
        var newWord = string.Concat(words);

        return newWord;
    }

    public static string CreateSafeIdentifier(string identifier)
    {
        return csharpKeywords.Contains(identifier) ? $"@{identifier}" : identifier;
    }

    public static string CreateReferenceAddMethodName(Field parentField, Field referencedField)
    {
        var parentFieldName = CreateSingularName(parentField);
        var referencedFieldName = CreateSingularName(referencedField);

        string suffix = parentFieldName;

        if (!parentFieldName.EndsWith(referencedFieldName))
        {
            suffix += referencedFieldName;
        }

        if(referencedFieldName.EndsWith(parentFieldName))
        {
            suffix = referencedFieldName;
        }

        return "Add" + suffix;
    }

    public static string CreateSingularName(Field field)
    {
        var fieldName = field.Name.Singularize();

        var suffix = "List";

        if (string.IsNullOrEmpty(fieldName) || string.IsNullOrEmpty(suffix))
        {
            return fieldName;
        }

        if (fieldName.EndsWith(suffix))
        {
            return fieldName[..^suffix.Length];
        }

        return fieldName;
    }

    [GeneratedRegex("<(\\w+)>")]
    private static partial Regex ListTypeRegex();

    [GeneratedRegex("(\\w+)<\\w+>")]
    private static partial Regex ParentTypeRegex();

    public static string? ExtractSyntaxTypeFromListType(string? listTypeName)
    {
        if (string.IsNullOrEmpty(listTypeName))
            return null;

        var match = ListTypeRegex().Match(listTypeName);

        return match.Success ? match.Groups[1].Value : null;
    }

    public static string? ExtractParentTypeFromListType(string? listTypeName)
    {
        if (string.IsNullOrEmpty(listTypeName))
            return null;

        var match = ParentTypeRegex().Match(listTypeName);

        return match.Success ? match.Groups[1].Value : null;
    }


}
