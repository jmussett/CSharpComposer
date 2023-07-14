# CSharpComposer • [![NuGet](https://img.shields.io/nuget/vpre/csharpcomposer.svg)](https://www.nuget.org/packages/csharpcomposer)

A fluent C# code generation library, constructed to ease and facilitate the task of creating C# code programmatically using Roslyn. Used for design-time code generation, source generators, and code fixes for static analyzers.

Automatically generated from the [Syntax Rules](https://github.com/dotnet/roslyn/blob/main/src/Compilers/CSharp/Portable/Syntax/Syntax.xml) defined by the Roslyn Compiler.

## Installation

You should install CSharpComposer with [NuGet](https://www.nuget.org/packages/CSharpComposer/):

    Install-Package CSharpComposer

Or via the .NET Core command line interface:

    dotnet add package CSharpComposer

Either commands, from Package Manager Console or .NET Core CLI, will download and install MediatR and all required dependencies.

## Usage

The entry point to CSharpComposer is the **CSharpFactory** class, which can be used for constructing any kind of syntax node. 

For demonstration, here's an example of creating a Hello World application:

```c#
using CSharpComposer;

var compilationUnit = CSharpFactory.CompilationUnit(cu => cu
    .AddUsingDirective(t => t.AsIdentifierName("MyNamespace"))
    .AddFileScopedNamespaceDeclaration(
        t => t.AsIdentifierName("MyNamespace"), 
        ns => ns
            .AddClassDeclaration("Program", cs => cs
                .AddMethodDeclaration(
                    x => x.AsPredefinedType(PredefinedTypeKeyword.VoidKeyword),
                    "Main",
                    md => md
                        .AddModifierToken(SyntaxKind.StaticKeyword)
                        .AddParameter("args", p => p
                            .WithType(t => t
                                .AsArrayType(at => at
                                    .AsPredefinedType(PredefinedTypeKeyword.StringKeyword)
                                )
                            )
                        )
                        .WithBody(b => b
                            .AddExpressionStatement(es => es
                                .AsInvocationExpression(
                                    e => e.AsMemberAccessExpression(
                                        MemberAccessExpressionKind.SimpleMemberAccessExpression, 
                                        e => e.AsIdentifierName("Console"), 
                                        e => e.AsIdentifierName("WriteLine")
                                    ),
                                    ie => ie.AddArgument(a => a
                                        .AsLiteralExpression(le => le
                                            .AsStringLiteral("Hello, World!")
                                        )
                                    )
                                )
                             )
                        )
                )
            )
    )
);

Console.WriteLine(compilationUnit.NormalizeWhitespace());
```

This outputs the following code:

```c#
using System;

namespace MyNamespace;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
    }
}
```

## Parsing API

To improve on the verbose nature of writing C# syntax nodes by hand, **CSharpComposer** has been extended to support Roslyn's parsing API

This allows you to parse the following syntax types, with examples of supporting extension methods to improve quality of life:

- NameSyntax
  - ```n => n.ParseName("{name}");```
  - ```n => n.AddAttribute("{name}");```
- TypeSyntax
  - ```t => t.ParseTypeName("{type}");```
  - ```t => t.WithType("{type}");```
- ExpressionSyntax
  - ```e => e.ParseExpression("{expression}");```
  - ```t => t.WithExpressionBody("{type}");```
- StatementSyntax
  - ```s => s.ParseStatement("{statement}");```
  - ```s => s.AddStatement("{statement}");```
- MemberDeclarationSyntax
  - ```md => md.ParseMemberDeclaration("{memberDeclaration}");```
  - ```md => md.AddMemberDeclaration("{memberDeclaration}");```
- CompilationUnitSyntax
  - ```cu => cu.ParseCompilationUnit("{compilationUnit}");```


