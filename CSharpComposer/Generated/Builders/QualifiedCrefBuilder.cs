using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
internal partial class QualifiedCrefBuilder
{
    public static QualifiedCrefSyntax CreateSyntax(Action<ITypeBuilder> containerCallback, Action<IMemberCrefBuilder> memberCallback)
    {
        var containerSyntax = TypeBuilder.CreateSyntax(containerCallback);
        var dotTokenToken = SyntaxFactory.Token(SyntaxKind.DotToken);
        var memberSyntax = MemberCrefBuilder.CreateSyntax(memberCallback);
        return SyntaxFactory.QualifiedCref(containerSyntax, dotTokenToken, memberSyntax);
    }
}