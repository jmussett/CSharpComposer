﻿using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
internal partial class QueryContinuationBuilder
{
    public static QueryContinuationSyntax CreateSyntax(string identifier, Action<ISelectOrGroupClauseBuilder> bodySelectOrGroupCallback, Action<IQueryBodyBuilder> bodyQueryBodyCallback)
    {
        var intoKeywordToken = SyntaxFactory.Token(SyntaxKind.IntoKeyword);
        var identifierToken = SyntaxFactory.Identifier(identifier);
        var bodySyntax = QueryBodyBuilder.CreateSyntax(bodySelectOrGroupCallback, bodyQueryBodyCallback);
        return SyntaxFactory.QueryContinuation(intoKeywordToken, identifierToken, bodySyntax);
    }
}