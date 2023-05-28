using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithPredefinedTypeBuilder<TBuilder>
{
    TBuilder WithPredefinedType(PredefinedTypeKeyword predefinedTypeKeyword);
    TBuilder WithPredefinedType(PredefinedTypeSyntax predefinedTypeSyntax);
}

public partial class PredefinedTypeBuilder
{
    public static PredefinedTypeSyntax CreateSyntax(PredefinedTypeKeyword predefinedTypeKeyword)
    {
        var keywordToken = predefinedTypeKeyword switch
        {
            PredefinedTypeKeyword.BoolKeyword => SyntaxFactory.Token(SyntaxKind.BoolKeyword),
            PredefinedTypeKeyword.ByteKeyword => SyntaxFactory.Token(SyntaxKind.ByteKeyword),
            PredefinedTypeKeyword.SByteKeyword => SyntaxFactory.Token(SyntaxKind.SByteKeyword),
            PredefinedTypeKeyword.IntKeyword => SyntaxFactory.Token(SyntaxKind.IntKeyword),
            PredefinedTypeKeyword.UIntKeyword => SyntaxFactory.Token(SyntaxKind.UIntKeyword),
            PredefinedTypeKeyword.ShortKeyword => SyntaxFactory.Token(SyntaxKind.ShortKeyword),
            PredefinedTypeKeyword.UShortKeyword => SyntaxFactory.Token(SyntaxKind.UShortKeyword),
            PredefinedTypeKeyword.LongKeyword => SyntaxFactory.Token(SyntaxKind.LongKeyword),
            PredefinedTypeKeyword.ULongKeyword => SyntaxFactory.Token(SyntaxKind.ULongKeyword),
            PredefinedTypeKeyword.FloatKeyword => SyntaxFactory.Token(SyntaxKind.FloatKeyword),
            PredefinedTypeKeyword.DoubleKeyword => SyntaxFactory.Token(SyntaxKind.DoubleKeyword),
            PredefinedTypeKeyword.DecimalKeyword => SyntaxFactory.Token(SyntaxKind.DecimalKeyword),
            PredefinedTypeKeyword.StringKeyword => SyntaxFactory.Token(SyntaxKind.StringKeyword),
            PredefinedTypeKeyword.CharKeyword => SyntaxFactory.Token(SyntaxKind.CharKeyword),
            PredefinedTypeKeyword.ObjectKeyword => SyntaxFactory.Token(SyntaxKind.ObjectKeyword),
            PredefinedTypeKeyword.VoidKeyword => SyntaxFactory.Token(SyntaxKind.VoidKeyword),
            _ => throw new NotSupportedException()
        };
        return SyntaxFactory.PredefinedType(keywordToken);
    }
}