using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IImplicitElementAccessBuilder : IAddArgument<IImplicitElementAccessBuilder>
{
}

public partial class ImplicitElementAccessBuilder : IImplicitElementAccessBuilder
{
    public ImplicitElementAccessSyntax Syntax { get; set; }

    public ImplicitElementAccessBuilder(ImplicitElementAccessSyntax syntax)
    {
        Syntax = syntax;
    }

    public static ImplicitElementAccessSyntax CreateSyntax(Action<IImplicitElementAccessBuilder>? implicitElementAccessCallback = null)
    {
        var argumentListSyntax = SyntaxFactory.BracketedArgumentList();
        var syntax = SyntaxFactory.ImplicitElementAccess(argumentListSyntax);
        var builder = new ImplicitElementAccessBuilder(syntax);
        implicitElementAccessCallback?.Invoke(builder);
        return builder.Syntax;
    }

    public IImplicitElementAccessBuilder AddArgument(Action<IExpressionBuilder> expressionCallback, Action<IArgumentBuilder>? argumentCallback = null)
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