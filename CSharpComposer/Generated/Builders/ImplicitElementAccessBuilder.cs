using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IImplicitElementAccessBuilder
{
    IImplicitElementAccessBuilder AddArgument(Action<IExpressionBuilder> expressionCallback, Action<IArgumentBuilder> argumentCallback);
    IImplicitElementAccessBuilder AddArgument(ArgumentSyntax argument);
}

public interface IWithImplicitElementAccessBuilder<TBuilder>
{
    TBuilder WithImplicitElementAccess(Action<IImplicitElementAccessBuilder> implicitElementAccessCallback);
    TBuilder WithImplicitElementAccess(ImplicitElementAccessSyntax implicitElementAccessSyntax);
}

public partial class ImplicitElementAccessBuilder : IImplicitElementAccessBuilder
{
    public ImplicitElementAccessSyntax Syntax { get; set; }

    public ImplicitElementAccessBuilder(ImplicitElementAccessSyntax syntax)
    {
        Syntax = syntax;
    }

    public static ImplicitElementAccessSyntax CreateSyntax(Action<IImplicitElementAccessBuilder> implicitElementAccessCallback)
    {
        var argumentListSyntax = SyntaxFactory.BracketedArgumentList();
        var syntax = SyntaxFactory.ImplicitElementAccess(argumentListSyntax);
        var builder = new ImplicitElementAccessBuilder(syntax);
        implicitElementAccessCallback(builder);
        return builder.Syntax;
    }

    public IImplicitElementAccessBuilder AddArgument(Action<IExpressionBuilder> expressionCallback, Action<IArgumentBuilder> argumentCallback)
    {
        var argument = ArgumentBuilder.CreateSyntax(expressionCallback, argumentCallback);
        Syntax = Syntax.AddArgumentListArguments(argument);
        return this;
    }

    public IImplicitElementAccessBuilder AddArgument(ArgumentSyntax argument)
    {
        Syntax = Syntax.AddArgumentListArguments(argument);
        return this;
    }
}