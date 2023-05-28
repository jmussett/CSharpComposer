using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithLineDirectivePositionBuilder<TBuilder>
{
    TBuilder WithLineDirectivePosition(int line, int character);
    TBuilder WithLineDirectivePosition(LineDirectivePositionSyntax lineDirectivePositionSyntax);
}

public partial class LineDirectivePositionBuilder
{
    public static LineDirectivePositionSyntax CreateSyntax(int line, int character)
    {
        var openParenTokenToken = SyntaxFactory.Token(SyntaxKind.OpenParenToken);
        var lineToken = SyntaxFactory.Literal(line);
        var commaTokenToken = SyntaxFactory.Token(SyntaxKind.CommaToken);
        var characterToken = SyntaxFactory.Literal(character);
        var closeParenTokenToken = SyntaxFactory.Token(SyntaxKind.CloseParenToken);
        return SyntaxFactory.LineDirectivePosition(openParenTokenToken, lineToken, commaTokenToken, characterToken, closeParenTokenToken);
    }
}