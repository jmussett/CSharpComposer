using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IBranchingDirectiveTriviaBuilder
{
    void AsIfDirectiveTrivia(Action<IExpressionBuilder> conditionCallback, bool isActive, bool branchTaken, bool conditionValue);
    void AsElifDirectiveTrivia(Action<IExpressionBuilder> conditionCallback, bool isActive, bool branchTaken, bool conditionValue);
    void AsElseDirectiveTrivia(bool isActive, bool branchTaken);
}

public interface IWithBranchingDirectiveTriviaBuilder<TBuilder>
{
    TBuilder WithBranchingDirectiveTrivia(Action<IBranchingDirectiveTriviaBuilder> branchingDirectiveTriviaCallback);
    TBuilder WithBranchingDirectiveTrivia(BranchingDirectiveTriviaSyntax branchingDirectiveTriviaSyntax);
}

public partial class BranchingDirectiveTriviaBuilder : IBranchingDirectiveTriviaBuilder
{
    public BranchingDirectiveTriviaSyntax? Syntax { get; set; }

    public static BranchingDirectiveTriviaSyntax CreateSyntax(Action<IBranchingDirectiveTriviaBuilder> callback)
    {
        var builder = new BranchingDirectiveTriviaBuilder();
        callback(builder);
        if (builder.Syntax is null)
        {
            throw new InvalidOperationException("BranchingDirectiveTriviaSyntax has not been specified");
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

    public void AsElseDirectiveTrivia(bool isActive, bool branchTaken)
    {
        Syntax = ElseDirectiveTriviaBuilder.CreateSyntax(isActive, branchTaken);
    }
}