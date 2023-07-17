using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IClassOrStructConstraintBuilder
{
    IClassOrStructConstraintBuilder WithQuestionToken();
}

internal partial class ClassOrStructConstraintBuilder : IClassOrStructConstraintBuilder
{
    public ClassOrStructConstraintSyntax Syntax { get; set; }

    public ClassOrStructConstraintBuilder(ClassOrStructConstraintSyntax syntax)
    {
        Syntax = syntax;
    }

    public static ClassOrStructConstraintSyntax CreateSyntax(ClassOrStructConstraintKind kind, Action<IClassOrStructConstraintBuilder>? classOrStructConstraintCallback = null)
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
        classOrStructConstraintCallback?.Invoke(builder);
        return builder.Syntax;
    }

    public IClassOrStructConstraintBuilder WithQuestionToken()
    {
        Syntax = Syntax.WithQuestionToken(SyntaxFactory.Token(SyntaxKind.QuestionToken));
        return this;
    }
}