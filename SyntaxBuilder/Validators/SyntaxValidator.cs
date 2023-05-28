using Microsoft.CodeAnalysis.CSharp;

namespace SyntaxBuilder.Validators;

internal class SyntaxValidator
{
    public static void ValidateIdentifier(string identifier)
    {
        if (!SyntaxFacts.IsValidIdentifier(identifier))
        {
            //throw new InvalidOperationException($"'{identifier}' is not a valid identifier");
        }
    }

    public static void ValidateName(string name)
    {
        string[] parts = name.Split('.', StringSplitOptions.RemoveEmptyEntries);

        foreach (string part in parts)
        {
            if (!SyntaxFacts.IsValidIdentifier(part))
            {
                throw new InvalidOperationException($"The identifier part '{part}' of '{name}' is not valid.");
            }
        }
    }

    // TODO: Validate namespaces
}
