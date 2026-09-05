using Werecodent.CreateAndFake.FakerTool;
using Werecodent.CreateAndFake.FakerTool.Proxy;
using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Tests.FakerTool.Proxy;

public static class CallDataTests
{
    [Fact]
    internal static Task CallData_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<CallData>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task CallData_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<CallData>(
            TestContext.Current.CancellationToken
        );
    }

    [Theory, RandomData]
    internal static void MatchesCall_MethodNameMismatch(
        DataHolderSample[] data,
        Type[] generics,
        string name
    )
    {
        new CallData(name, generics, data, Tools.Faker.Options)
            .MatchesCall(new CallData(name.Tools().Variant(), generics, data, null))
            .Assert()
            .Is(false);
    }

    [Theory, RandomData]
    internal static void MatchesCall_GenericsMismatch(DataHolderSample[] data, string name)
    {
        Type[] generics1 = [.. Tools.Randomizer.Create<Type[]>().Except([typeof(AnyGeneric)])];
        Type[] generics2 = Tools.Mutator.Variant(generics1);

        new CallData(name, generics1, data, Tools.Faker.Options)
            .MatchesCall(new CallData(name, generics2, data, null))
            .Assert()
            .Is(false);
    }

    [Theory, RandomData]
    internal static void MatchesCall_AnyGenericMatchesAll(
        DataHolderSample[] data,
        string name,
        Type[] generics1
    )
    {
        Type[] generics2 = [.. generics1.Select(_ => typeof(AnyGeneric))];

        new CallData(name, generics2, data, Tools.Faker.Options)
            .MatchesCall(new CallData(name, generics1, data, null))
            .Assert()
            .Is(true);
    }

    [Theory, RandomData]
    internal static void MatchesCall_DataMatchBehavior(
        string name,
        Type[] generics,
        DataHolderSample[] data1
    )
    {
        DataHolderSample[] data2 = [.. data1.Select(d => d.Tools().Copy())];

        new CallData(name, generics, data1, Tools.Faker.Options)
            .MatchesCall(new CallData(name, generics, data2, null))
            .Assert()
            .Is(true);

        new CallData(name, generics, data1, null)
            .MatchesCall(new CallData(name, generics, data2, null))
            .Assert()
            .Is(false);
    }
}
