using CreateAndFake.AsserterTool.Fluent;
using CreateAndFake.FakerTool;
using CreateAndFake.RunnerTool;

namespace CreateAndFake.Tests.AsserterTool.Fluent;

public static class AssertComparableTests
{
    [Fact]
    internal static void AssertComparable_GuardsNulls()
    {
        Tools.Tester.PreventsNullRefException<AssertComparable>();
    }

    [Fact]
    internal static void AssertComparable_NoParameterMutation()
    {
        Tools.Tester.PreventsParameterMutation<AssertComparable>();
    }

    [Theory, RandomData]
    internal static void AssertComparable_CallsAndChains(Injected<AssertComparable> instance)
    {
        RunResults results = Tools.Runner.CallMethodsOn(instance.Dummy);
        results.RawResults
            .Where(r => r.Result != null)
            .Where(r => r.Result is not AssertChainer<AssertComparable>)
            .Select(r => r.Method.Name)
            .Assert()
            .IsEmpty();
    }
}

