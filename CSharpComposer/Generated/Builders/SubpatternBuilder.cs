using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface ISubpatternBuilder
{
    ISubpatternBuilder WithExpressionColon(Action<IBaseExpressionColonBuilder> expressionColonCallback);
    ISubpatternBuilder WithExpressionColon(BaseExpressionColonSyntax expressionColon);
}

public interface IAddSubpattern<TBuilder>
{
    TBuilder AddSubpattern(SubpatternSyntax subpatternSyntax);
    TBuilder AddSubpattern(Action<IPatternBuilder> patternCallback, Action<ISubpatternBuilder>? subpatternCallback = null);
}

internal partial class SubpatternBuilder : ISubpatternBuilder
{
    public SubpatternSyntax Syntax { get; set; }

    public SubpatternBuilder(SubpatternSyntax syntax)
    {
        Syntax = syntax;
    }

    public static SubpatternSyntax CreateSyntax(Action<IPatternBuilder> patternCallback, Action<ISubpatternBuilder>? subpatternCallback = null)
    {
        var patternSyntax = PatternBuilder.CreateSyntax(patternCallback);
        var syntax = SyntaxFactory.Subpattern(default(BaseExpressionColonSyntax), patternSyntax);
        var builder = new SubpatternBuilder(syntax);
        subpatternCallback?.Invoke(builder);
        return builder.Syntax;
    }

    public ISubpatternBuilder WithExpressionColon(Action<IBaseExpressionColonBuilder> expressionColonCallback)
    {
        var expressionColonSyntax = BaseExpressionColonBuilder.CreateSyntax(expressionColonCallback);
        Syntax = Syntax.WithExpressionColon(expressionColonSyntax);
        return this;
    }

    public ISubpatternBuilder WithExpressionColon(BaseExpressionColonSyntax expressionColon)
    {
        Syntax = Syntax.WithExpressionColon(expressionColon);
        return this;
    }
}