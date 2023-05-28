using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IConstructorInitializerBuilder
{
    IConstructorInitializerBuilder AddArgument(Action<IExpressionBuilder> expressionCallback, Action<IArgumentBuilder> argumentCallback);
    IConstructorInitializerBuilder AddArgument(ArgumentSyntax argument);
}

public interface IWithConstructorInitializerBuilder<TBuilder>
{
    TBuilder WithConstructorInitializer(ConstructorInitializerKind kind, Action<IConstructorInitializerBuilder> constructorInitializerCallback);
    TBuilder WithConstructorInitializer(ConstructorInitializerSyntax constructorInitializerSyntax);
}

public partial class ConstructorInitializerBuilder : IConstructorInitializerBuilder
{
    public ConstructorInitializerSyntax Syntax { get; set; }

    public ConstructorInitializerBuilder(ConstructorInitializerSyntax syntax)
    {
        Syntax = syntax;
    }

    public static ConstructorInitializerSyntax CreateSyntax(ConstructorInitializerKind kind, Action<IConstructorInitializerBuilder> constructorInitializerCallback)
    {
        var syntaxKind = kind switch
        {
            ConstructorInitializerKind.BaseConstructorInitializer => SyntaxKind.BaseConstructorInitializer,
            ConstructorInitializerKind.ThisConstructorInitializer => SyntaxKind.ThisConstructorInitializer,
            _ => throw new NotSupportedException()
        };
        var colonTokenToken = SyntaxFactory.Token(SyntaxKind.ColonToken);
        var thisOrBaseKeywordToken = kind switch
        {
            ConstructorInitializerKind.BaseConstructorInitializer => SyntaxFactory.Token(SyntaxKind.BaseKeyword),
            ConstructorInitializerKind.ThisConstructorInitializer => SyntaxFactory.Token(SyntaxKind.ThisKeyword),
            _ => throw new NotSupportedException()
        };
        var argumentListSyntax = SyntaxFactory.ArgumentList();
        var syntax = SyntaxFactory.ConstructorInitializer(syntaxKind, colonTokenToken, thisOrBaseKeywordToken, argumentListSyntax);
        var builder = new ConstructorInitializerBuilder(syntax);
        constructorInitializerCallback(builder);
        return builder.Syntax;
    }

    public IConstructorInitializerBuilder AddArgument(Action<IExpressionBuilder> expressionCallback, Action<IArgumentBuilder> argumentCallback)
    {
        var argument = ArgumentBuilder.CreateSyntax(expressionCallback, argumentCallback);
        Syntax = Syntax.AddArgumentListArguments(argument);
        return this;
    }

    public IConstructorInitializerBuilder AddArgument(ArgumentSyntax argument)
    {
        Syntax = Syntax.AddArgumentListArguments(argument);
        return this;
    }
}