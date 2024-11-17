using CreateAndFake.Toolbox.FakerTool;
using CreateAndFake.Toolbox.FakerTool.Proxy;
using CreateAndFakeTests.TestSamples;

namespace CreateAndFakeTests.Toolbox.FakerTool.Proxy;

public static class CallDataTests
{
    [Fact]
    internal static void CallData_GuardsNulls()
    {
        Tools.Tester.PreventsNullRefException<CallData>();
    }

    [Fact]
    internal static void CallData_NoParameterMutation()
    {
        Tools.Tester.PreventsParameterMutation<CallData>();
    }

    [Theory, RandomData]
    internal static void MatchesCall_MethodNameMismatch(DataHolderSample[] data, Type[] generics, string name)
    {
        new CallData(name, generics, data, Tools.Faker.Options)
            .MatchesCall(new CallData(name.CreateVariant(), generics, data, null))
            .Assert().Is(false);
    }

    [Theory, RandomData]
    internal static void MatchesCall_GenericsMismatch(DataHolderSample[] data, string name)
    {
        Type[] generics1 = Tools.Randomizer.Create<Type[]>().Except([typeof(AnyGeneric)]).ToArray();
        Type[] generics2 = Tools.Mutator.Variant(generics1);

        new CallData(name, generics1, data, Tools.Faker.Options)
            .MatchesCall(new CallData(name, generics2, data, null))
            .Assert().Is(false);
    }

    [Theory, RandomData]
    internal static void MatchesCall_AnyGenericMatchesAll(DataHolderSample[] data, string name, Type[] generics1)
    {
        Type[] generics2 = generics1.Select(t => typeof(AnyGeneric)).ToArray();

        new CallData(name, generics2, data, Tools.Faker.Options)
            .MatchesCall(new CallData(name, generics1, data, null))
            .Assert().Is(true);
    }

    [Theory, RandomData]
    internal static void MatchesCall_DataMatchBehavior(string name, Type[] generics, DataHolderSample[] data1)
    {
        DataHolderSample[] data2 = data1.Select(d => d.CreateDeepClone()).ToArray();

        new CallData(name, generics, data1, Tools.Faker.Options)
            .MatchesCall(new CallData(name, generics, data2, null))
            .Assert().Is(true);

        new CallData(name, generics, data1, null)
            .MatchesCall(new CallData(name, generics, data2, null))
            .Assert().Is(false);
    }
}
