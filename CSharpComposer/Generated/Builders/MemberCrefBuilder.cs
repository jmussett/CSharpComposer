using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IMemberCrefBuilder
{
    void AsNameMemberCref(Action<ITypeBuilder> nameCallback, Action<INameMemberCrefBuilder>? nameMemberCrefCallback = null);
    void AsIndexerMemberCref(Action<IIndexerMemberCrefBuilder>? indexerMemberCrefCallback = null);
    void AsOperatorMemberCref(OperatorMemberCrefOperatorToken operatorMemberCrefOperatorToken, Action<IOperatorMemberCrefBuilder>? operatorMemberCrefCallback = null);
    void AsConversionOperatorMemberCref(ConversionOperatorMemberCrefImplicitOrExplicitKeyword conversionOperatorMemberCrefImplicitOrExplicitKeyword, Action<ITypeBuilder> typeCallback, Action<IConversionOperatorMemberCrefBuilder>? conversionOperatorMemberCrefCallback = null);
}

internal partial class MemberCrefBuilder : IMemberCrefBuilder
{
    public MemberCrefSyntax? Syntax { get; set; }

    public static MemberCrefSyntax CreateSyntax(Action<IMemberCrefBuilder> callback)
    {
        var builder = new MemberCrefBuilder();
        callback(builder);
        if (builder.Syntax is null)
        {
            throw new InvalidOperationException("MemberCrefSyntax has not been specified");
        }

        return builder.Syntax;
    }

    public void AsNameMemberCref(Action<ITypeBuilder> nameCallback, Action<INameMemberCrefBuilder>? nameMemberCrefCallback = null)
    {
        Syntax = NameMemberCrefBuilder.CreateSyntax(nameCallback, nameMemberCrefCallback);
    }

    public void AsIndexerMemberCref(Action<IIndexerMemberCrefBuilder>? indexerMemberCrefCallback = null)
    {
        Syntax = IndexerMemberCrefBuilder.CreateSyntax(indexerMemberCrefCallback);
    }

    public void AsOperatorMemberCref(OperatorMemberCrefOperatorToken operatorMemberCrefOperatorToken, Action<IOperatorMemberCrefBuilder>? operatorMemberCrefCallback = null)
    {
        Syntax = OperatorMemberCrefBuilder.CreateSyntax(operatorMemberCrefOperatorToken, operatorMemberCrefCallback);
    }

    public void AsConversionOperatorMemberCref(ConversionOperatorMemberCrefImplicitOrExplicitKeyword conversionOperatorMemberCrefImplicitOrExplicitKeyword, Action<ITypeBuilder> typeCallback, Action<IConversionOperatorMemberCrefBuilder>? conversionOperatorMemberCrefCallback = null)
    {
        Syntax = ConversionOperatorMemberCrefBuilder.CreateSyntax(conversionOperatorMemberCrefImplicitOrExplicitKeyword, typeCallback, conversionOperatorMemberCrefCallback);
    }
}