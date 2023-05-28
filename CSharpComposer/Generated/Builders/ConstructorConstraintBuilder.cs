using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithConstructorConstraintBuilder<TBuilder>
{
    TBuilder WithConstructorConstraint();
    TBuilder WithConstructorConstraint(ConstructorConstraintSyntax constructorConstraintSyntax);
}

public partial class ConstructorConstraintBuilder
{
    public static ConstructorConstraintSyntax CreateSyntax()
    {
        var newKeywordToken = SyntaxFactory.Token(SyntaxKind.NewKeyword);
        var openParenTokenToken = SyntaxFactory.Token(SyntaxKind.OpenParenToken);
        var closeParenTokenToken = SyntaxFactory.Token(SyntaxKind.CloseParenToken);
        return SyntaxFactory.ConstructorConstraint(newKeywordToken, openParenTokenToken, closeParenTokenToken);
    }
}