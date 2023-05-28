using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IConditionalDirectiveTriviaBuilder
{
    void AsIfDirectiveTrivia(Action<IExpressionBuilder> conditionCallback, bool isActive, bool branchTaken, bool conditionValue);
    void AsElifDirectiveTrivia(Action<IExpressionBuilder> conditionCallback, bool isActive, bool branchTaken, bool conditionValue);
}

public interface IWithConditionalDirectiveTriviaBuilder<TBuilder>
{
    TBuilder WithConditionalDirectiveTrivia(Action<IConditionalDirectiveTriviaBuilder> conditionalDirectiveTriviaCallback);
    TBuilder WithConditionalDirectiveTrivia(ConditionalDirectiveTriviaSyntax conditionalDirectiveTriviaSyntax);
}

public partial class ConditionalDirectiveTriviaBuilder : IConditionalDirectiveTriviaBuilder
{
    public ConditionalDirectiveTriviaSyntax? Syntax { get; set; }

    public static ConditionalDirectiveTriviaSyntax CreateSyntax(Action<IConditionalDirectiveTriviaBuilder> callback)
    {
        var builder = new ConditionalDirectiveTriviaBuilder();
        callback(builder);
        if (builder.Syntax is null)
        {
            throw new InvalidOperationException("ConditionalDirectiveTriviaSyntax has not been specified");
        }

        return builder.Syntax;
    }

    public void AsIfDirectiveTrivia(Action<IExpressionBuilder> conditionCallback, bool isActive, bool branchTaken, bool conditionValue)
    {
        Syntax = IfDirectiveTriviaBuilder.CreateSyntax(conditionCallback, isActive, branchTaken, conditionValue);
    }

    public void AsElifDirectiveTrivia(Action<IExpressionBuilder> conditionCallback, bool isActive, bool branchTaken, bool conditionValue)
    {
        Syntax = ElifDirectiveTriviaBuilder.CreateSyntax(conditionCallback, isActive, branchTaken, conditionValue);
    }
}