using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithTypePatternBuilder<TBuilder>
{
    TBuilder WithTypePattern(Action<ITypeBuilder> typeCallback);
    TBuilder WithTypePattern(TypePatternSyntax typePatternSyntax);
}

public partial class TypePatternBuilder
{
    public static TypePatternSyntax CreateSyntax(Action<ITypeBuilder> typeCallback)
    {
        var typeSyntax = TypeBuilder.CreateSyntax(typeCallback);
        return SyntaxFactory.TypePattern(typeSyntax);
    }
}