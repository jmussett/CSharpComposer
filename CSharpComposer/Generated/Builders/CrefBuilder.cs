using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface ICrefBuilder
{
    void AsTypeCref(Action<ITypeBuilder> typeCallback);
    void AsQualifiedCref(Action<ITypeBuilder> containerCallback, Action<IMemberCrefBuilder> memberCallback);
    void AsNameMemberCref(Action<ITypeBuilder> nameCallback, Action<INameMemberCrefBuilder> nameMemberCrefCallback);
    void AsIndexerMemberCref(Action<IIndexerMemberCrefBuilder> indexerMemberCrefCallback);
    void AsOperatorMemberCref(OperatorMemberCrefOperatorToken operatorMemberCrefOperatorToken, Action<IOperatorMemberCrefBuilder> operatorMemberCrefCallback);
    void AsConversionOperatorMemberCref(ConversionOperatorMemberCrefImplicitOrExplicitKeyword conversionOperatorMemberCrefImplicitOrExplicitKeyword, Action<ITypeBuilder> typeCallback, Action<IConversionOperatorMemberCrefBuilder> conversionOperatorMemberCrefCallback);
}

public interface IWithCrefBuilder<TBuilder>
{
    TBuilder WithCref(Action<ICrefBuilder> crefCallback);
    TBuilder WithCref(CrefSyntax crefSyntax);
}

public partial class CrefBuilder : ICrefBuilder
{
    public CrefSyntax? Syntax { get; set; }

    public static CrefSyntax CreateSyntax(Action<ICrefBuilder> callback)
    {
        var builder = new CrefBuilder();
        callback(builder);
        if (builder.Syntax is null)
        {
            throw new InvalidOperationException("CrefSyntax has not been specified");
        }

        return builder.Syntax;
    }

    public void AsTypeCref(Action<ITypeBuilder> typeCallback)
    {
        Syntax = TypeCrefBuilder.CreateSyntax(typeCallback);
    }

    public void AsQualifiedCref(Action<ITypeBuilder> containerCallback, Action<IMemberCrefBuilder> memberCallback)
    {
        Syntax = QualifiedCrefBuilder.CreateSyntax(containerCallback, memberCallback);
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