using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IPositionalPatternClauseBuilder
{
    IPositionalPatternClauseBuilder AddSubpattern(Action<IPatternBuilder> patternCallback, Action<ISubpatternBuilder> subpatternCallback);
    IPositionalPatternClauseBuilder AddSubpattern(SubpatternSyntax subpattern);
}

public interface IWithPositionalPatternClauseBuilder<TBuilder>
{
    TBuilder WithPositionalPatternClause(Action<IPositionalPatternClauseBuilder> positionalPatternClauseCallback);
    TBuilder WithPositionalPatternClause(PositionalPatternClauseSyntax positionalPatternClauseSyntax);
}

public partial class PositionalPatternClauseBuilder : IPositionalPatternClauseBuilder
{
    public PositionalPatternClauseSyntax Syntax { get; set; }

    public PositionalPatternClauseBuilder(PositionalPatternClauseSyntax syntax)
    {
        Syntax = syntax;
    }

    public static PositionalPatternClauseSyntax CreateSyntax(Action<IPositionalPatternClauseBuilder> positionalPatternClauseCallback)
    {
        var openParenTokenToken = SyntaxFactory.Token(SyntaxKind.OpenParenToken);
        var closeParenTokenToken = SyntaxFactory.Token(SyntaxKind.CloseParenToken);
        var syntax = SyntaxFactory.PositionalPatternClause(openParenTokenToken, default(SeparatedSyntaxList<SubpatternSyntax>), closeParenTokenToken);
        var builder = new PositionalPatternClauseBuilder(syntax);
        positionalPatternClauseCallback(builder);
        return builder.Syntax;
    }

    public IPositionalPatternClauseBuilder AddSubpattern(Action<IPatternBuilder> patternCallback, Action<ISubpatternBuilder> subpatternCallback)
    {
        var subpattern = SubpatternBuilder.CreateSyntax(patternCallback, subpatternCallback);
        Syntax = Syntax.AddSubpatterns(subpattern);
        return this;
    }

    public IPositionalPatternClauseBuilder AddSubpattern(SubpatternSyntax subpattern)
    {
        Syntax = Syntax.AddSubpatterns(subpattern);
        return this;
    }
}