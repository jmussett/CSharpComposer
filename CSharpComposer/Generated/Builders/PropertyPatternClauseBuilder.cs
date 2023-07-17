using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IPropertyPatternClauseBuilder : IAddSubpattern<IPropertyPatternClauseBuilder>
{
}

public interface IWithPropertyPatternClause<TBuilder>
{
    TBuilder WithPropertyPatternClause(PropertyPatternClauseSyntax propertyPatternClauseSyntax);
    TBuilder WithPropertyPatternClause(Action<IPropertyPatternClauseBuilder>? propertyPatternClauseCallback = null);
}

internal partial class PropertyPatternClauseBuilder : IPropertyPatternClauseBuilder
{
    public PropertyPatternClauseSyntax Syntax { get; set; }

    public PropertyPatternClauseBuilder(PropertyPatternClauseSyntax syntax)
    {
        Syntax = syntax;
    }

    public static PropertyPatternClauseSyntax CreateSyntax(Action<IPropertyPatternClauseBuilder>? propertyPatternClauseCallback = null)
    {
        var openBraceTokenToken = SyntaxFactory.Token(SyntaxKind.OpenBraceToken);
        var closeBraceTokenToken = SyntaxFactory.Token(SyntaxKind.CloseBraceToken);
        var syntax = SyntaxFactory.PropertyPatternClause(openBraceTokenToken, default(SeparatedSyntaxList<SubpatternSyntax>), closeBraceTokenToken);
        var builder = new PropertyPatternClauseBuilder(syntax);
        propertyPatternClauseCallback?.Invoke(builder);
        return builder.Syntax;
    }

    public IPropertyPatternClauseBuilder AddSubpattern(Action<IPatternBuilder> patternCallback, Action<ISubpatternBuilder>? subpatternCallback = null)
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