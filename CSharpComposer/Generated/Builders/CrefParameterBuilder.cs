using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface ICrefParameterBuilder
{
    ICrefParameterBuilder WithRefKindKeyword(RefKindKeyword refKindKeyword);
}

public partial class CrefParameterBuilder : ICrefParameterBuilder
{
    public CrefParameterSyntax Syntax { get; set; }

    public CrefParameterBuilder(CrefParameterSyntax syntax)
    {
        Syntax = syntax;
    }

    public static CrefParameterSyntax CreateSyntax(Action<ITypeBuilder> typeCallback, Action<ICrefParameterBuilder>? crefParameterCallback = null)
    {
        var typeSyntax = TypeBuilder.CreateSyntax(typeCallback);
        var syntax = SyntaxFactory.CrefParameter(default(SyntaxToken), typeSyntax);
        var builder = new CrefParameterBuilder(syntax);
        crefParameterCallback?.Invoke(builder);
        return builder.Syntax;
    }

    public ICrefParameterBuilder WithRefKindKeyword(RefKindKeyword refKindKeyword)
    {
        Syntax = Syntax.WithRefKindKeyword(SyntaxFactory.Token(refKindKeyword switch
        {
            RefKindKeyword.RefKeyword => SyntaxKind.RefKeyword,
            RefKindKeyword.OutKeyword => SyntaxKind.OutKeyword,
            RefKindKeyword.InKeyword => SyntaxKind.InKeyword,
            _ => throw new NotSupportedException()
        }));
        return this;
    }
}