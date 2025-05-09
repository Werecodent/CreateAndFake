using CreateAndFake.AsserterTool.Fluent;
using CreateAndFake.FakerTool;
using CreateAndFake.RunnerTool;

namespace CreateAndFake.Tests.AsserterTool.Fluent;

public static class AssertObjectTests
{
    [Fact]
    internal static Task AssertObject_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<AssertObject>();
    }

    [Fact]
    internal static Task AssertObject_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<AssertObject>();
    }

    [Theory, RandomData]
    internal static async Task AssertObject_CallsAndChains(Injected<AssertObject> instance)
    {
        RunResults results = await Tools.Runner.CallMethodsOn(instance.Dummy);
        results
            .RawResults.Where(r => r.Result != null)
            .Where(r => r.Result is not AssertChainer<AssertObject>)
            .Select(r => r.Method.Name)
            .Assert()
            .IsEmpty();
    }
}
