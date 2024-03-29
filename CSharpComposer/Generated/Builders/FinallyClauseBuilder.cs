﻿using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
internal partial class FinallyClauseBuilder
{
    public static FinallyClauseSyntax CreateSyntax(Action<IBlockBuilder> blockBlockCallback)
    {
        var finallyKeywordToken = SyntaxFactory.Token(SyntaxKind.FinallyKeyword);
        var blockSyntax = BlockBuilder.CreateSyntax(blockBlockCallback);
        return SyntaxFactory.FinallyClause(finallyKeywordToken, blockSyntax);
    }
}