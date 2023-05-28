using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IMemberCrefBuilder
{
    void AsNameMemberCref(Action<ITypeBuilder> nameCallback, Action<INameMemberCrefBuilder> nameMemberCrefCallback);
    void AsIndexerMemberCref(Action<IIndexerMemberCrefBuilder> indexerMemberCrefCallback);
    void AsOperatorMemberCref(OperatorMemberCrefOperatorToken operatorMemberCrefOperatorToken, Action<IOperatorMemberCrefBuilder> operatorMemberCrefCallback);
    void AsConversionOperatorMemberCref(ConversionOperatorMemberCrefImplicitOrExplicitKeyword conversionOperatorMemberCrefImplicitOrExplicitKeyword, Action<ITypeBuilder> typeCallback, Action<IConversionOperatorMemberCrefBuilder> conversionOperatorMemberCrefCallback);
}

public interface IWithMemberCrefBuilder<TBuilder>
{
    TBuilder WithMemberCref(Action<IMemberCrefBuilder> memberCrefCallback);
    TBuilder WithMemberCref(MemberCrefSyntax memberCrefSyntax);
}

public partial class MemberCrefBuilder : IMemberCrefBuilder
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

    public void AsNameMemberCref(Action<ITypeBuilder> nameCallback, Action<INameMemberCrefBuilder> nameMemberCrefCallback)
    {
        Syntax = NameMemberCrefBuilder.CreateSyntax(nameCallback, nameMemberCrefCallback);
    }

    public void AsIndexerMemberCref(Action<IIndexerMemberCrefBuilder> indexerMemberCrefCallback)
    {
        Syntax = IndexerMemberCrefBuilder.CreateSyntax(indexerMemberCrefCallback);
    }

    public void AsOperatorMemberCref(OperatorMemberCrefOperatorToken operatorMemberCrefOperatorToken, Action<IOperatorMemberCrefBuilder> operatorMemberCrefCallback)
    {
        Syntax = OperatorMemberCrefBuilder.CreateSyntax(operatorMemberCrefOperatorToken, operatorMemberCrefCallback);
    }

    public void AsConversionOperatorMemberCref(ConversionOperatorMemberCrefImplicitOrExplicitKeyword conversionOperatorMemberCrefImplicitOrExplicitKeyword, Action<ITypeBuilder> typeCallback, Action<IConversionOperatorMemberCrefBuilder> conversionOperatorMemberCrefCallback)
    {
        Syntax = ConversionOperatorMemberCrefBuilder.CreateSyntax(conversionOperatorMemberCrefImplicitOrExplicitKeyword, typeCallback, conversionOperatorMemberCrefCallback);
    }
}