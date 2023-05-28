using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithQualifiedNameBuilder<TBuilder>
{
    TBuilder WithQualifiedName(Action<INameBuilder> leftCallback, Action<ISimpleNameBuilder> rightCallback);
    TBuilder WithQualifiedName(QualifiedNameSyntax qualifiedNameSyntax);
}

public partial class QualifiedNameBuilder
{
    public static QualifiedNameSyntax CreateSyntax(Action<INameBuilder> leftCallback, Action<ISimpleNameBuilder> rightCallback)
    {
        var leftSyntax = NameBuilder.CreateSyntax(leftCallback);
        var dotTokenToken = SyntaxFactory.Token(SyntaxKind.DotToken);
        var rightSyntax = SimpleNameBuilder.CreateSyntax(rightCallback);
        return SyntaxFactory.QualifiedName(leftSyntax, dotTokenToken, rightSyntax);
    }
}