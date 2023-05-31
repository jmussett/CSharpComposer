using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithOmittedTypeArgument<TBuilder>
{
    TBuilder WithOmittedTypeArgument();
    TBuilder WithOmittedTypeArgument(OmittedTypeArgumentSyntax omittedTypeArgumentSyntax);
}

public interface IAddOmittedTypeArgument<TBuilder>
{
    TBuilder AddOmittedTypeArgument();
    TBuilder AddOmittedTypeArgument(OmittedTypeArgumentSyntax omittedTypeArgumentSyntax);
}

public partial class OmittedTypeArgumentBuilder
{
    public static OmittedTypeArgumentSyntax CreateSyntax()
    {
        var omittedTypeArgumentTokenToken = SyntaxFactory.Token(SyntaxKind.OmittedTypeArgumentToken);
        return SyntaxFactory.OmittedTypeArgument(omittedTypeArgumentTokenToken);
    }
}