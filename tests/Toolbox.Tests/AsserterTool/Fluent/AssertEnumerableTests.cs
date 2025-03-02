using CreateAndFake.AsserterTool.Fluent;
using CreateAndFake.FakerTool;
using CreateAndFake.RunnerTool;

namespace CreateAndFake.Tests.AsserterTool.Fluent;

public static class AssertEnumerableTests
{
    [Fact]
    internal static void AssertEnumerable_GuardsNulls()
    {
        Tools.Tester.PreventsNullRefException<AssertEnumerable>();
    }

    [Fact]
    internal static void AssertEnumerable_NoParameterMutation()
    {
        Tools.Tester.PreventsParameterMutation<AssertEnumerable>();
    }

    [Theory, RandomData]
    internal static void AssertEnumerable_CallsAndChains(Injected<AssertEnumerable> instance)
    {
        RunResults results = Tools.Runner.CallMethodsOn(instance.Dummy);
        results.RawResults
            .Where(r => r.Result != null)
            .Where(r => r.Result is not AssertChainer<AssertEnumerable>)
            .Select(r => r.Method.Name)
            .Assert()
            .IsEmpty();
    }
}
