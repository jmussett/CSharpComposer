using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IPropertyPatternClauseBuilder
{
    IPropertyPatternClauseBuilder AddSubpattern(Action<IPatternBuilder> patternCallback, Action<ISubpatternBuilder> subpatternCallback);
    IPropertyPatternClauseBuilder AddSubpattern(SubpatternSyntax subpattern);
}

public interface IWithPropertyPatternClauseBuilder<TBuilder>
{
    TBuilder WithPropertyPatternClause(Action<IPropertyPatternClauseBuilder> propertyPatternClauseCallback);
    TBuilder WithPropertyPatternClause(PropertyPatternClauseSyntax propertyPatternClauseSyntax);
}

public partial class PropertyPatternClauseBuilder : IPropertyPatternClauseBuilder
{
    public PropertyPatternClauseSyntax Syntax { get; set; }

    public PropertyPatternClauseBuilder(PropertyPatternClauseSyntax syntax)
    {
        Syntax = syntax;
    }

    public static PropertyPatternClauseSyntax CreateSyntax(Action<IPropertyPatternClauseBuilder> propertyPatternClauseCallback)
    {
        var openBraceTokenToken = SyntaxFactory.Token(SyntaxKind.OpenBraceToken);
        var closeBraceTokenToken = SyntaxFactory.Token(SyntaxKind.CloseBraceToken);
        var syntax = SyntaxFactory.PropertyPatternClause(openBraceTokenToken, default(SeparatedSyntaxList<SubpatternSyntax>), closeBraceTokenToken);
        var builder = new PropertyPatternClauseBuilder(syntax);
        propertyPatternClauseCallback(builder);
        return builder.Syntax;
    }

    public IPropertyPatternClauseBuilder AddSubpattern(Action<IPatternBuilder> patternCallback, Action<ISubpatternBuilder> subpatternCallback)
    {
        var subpattern = SubpatternBuilder.CreateSyntax(patternCallback, subpatternCallback);
        Syntax = Syntax.AddSubpatterns(subpattern);
        return this;
    }

    public IPropertyPatternClauseBuilder AddSubpattern(SubpatternSyntax subpattern)
    {
        Syntax = Syntax.AddSubpatterns(subpattern);
        return this;
    }
}