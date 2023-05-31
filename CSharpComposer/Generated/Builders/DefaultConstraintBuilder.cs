using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithDefaultConstraint<TBuilder>
{
    TBuilder WithDefaultConstraint();
    TBuilder WithDefaultConstraint(DefaultConstraintSyntax defaultConstraintSyntax);
}

public interface IAddDefaultConstraint<TBuilder>
{
    TBuilder AddDefaultConstraint();
    TBuilder AddDefaultConstraint(DefaultConstraintSyntax defaultConstraintSyntax);
}

public partial class DefaultConstraintBuilder
{
    public static DefaultConstraintSyntax CreateSyntax()
    {
        var defaultKeywordToken = SyntaxFactory.Token(SyntaxKind.DefaultKeyword);
        return SyntaxFactory.DefaultConstraint(defaultKeywordToken);
    }
}