using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IClassOrStructConstraintBuilder
{
    IClassOrStructConstraintBuilder WithQuestionToken();
}

public interface IWithClassOrStructConstraintBuilder<TBuilder>
{
    TBuilder WithClassOrStructConstraint(ClassOrStructConstraintKind kind, Action<IClassOrStructConstraintBuilder> classOrStructConstraintCallback);
    TBuilder WithClassOrStructConstraint(ClassOrStructConstraintSyntax classOrStructConstraintSyntax);
}

public partial class ClassOrStructConstraintBuilder : IClassOrStructConstraintBuilder
{
    public ClassOrStructConstraintSyntax Syntax { get; set; }

    public ClassOrStructConstraintBuilder(ClassOrStructConstraintSyntax syntax)
    {
        Syntax = syntax;
    }

    public static ClassOrStructConstraintSyntax CreateSyntax(ClassOrStructConstraintKind kind, Action<IClassOrStructConstraintBuilder> classOrStructConstraintCallback)
    {
        var syntaxKind = kind switch
        {
            ClassOrStructConstraintKind.ClassConstraint => SyntaxKind.ClassConstraint,
            ClassOrStructConstraintKind.StructConstraint => SyntaxKind.StructConstraint,
            _ => throw new NotSupportedException()
        };
        var classOrStructKeywordToken = kind switch
        {
            ClassOrStructConstraintKind.ClassConstraint => SyntaxFactory.Token(SyntaxKind.ClassKeyword),
            ClassOrStructConstraintKind.StructConstraint => SyntaxFactory.Token(SyntaxKind.StructKeyword),
            _ => throw new NotSupportedException()
        };
        var syntax = SyntaxFactory.ClassOrStructConstraint(syntaxKind, classOrStructKeywordToken, default(SyntaxToken));
        var builder = new ClassOrStructConstraintBuilder(syntax);
        classOrStructConstraintCallback(builder);
        return builder.Syntax;
    }

    public IClassOrStructConstraintBuilder WithQuestionToken()
    {
        Syntax = Syntax.WithQuestionToken(SyntaxFactory.Token(SyntaxKind.QuestionToken));
        return this;
    }
}