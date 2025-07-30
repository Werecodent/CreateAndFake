using CreateAndFake.FakerTool;
using CreateAndFake.Fluent.AssertCalls;
using CreateAndFake.RunnerTool;

namespace CreateAndFake.Tests.Fluent.AssertCalls;

public static class AssertStringTests
{
    [Fact]
    internal static Task AssertString_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<AssertString>();
    }

    [Fact]
    internal static Task AssertString_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<AssertString>();
    }

    [Theory, RandomData]
    internal static async Task AssertString_CallsAndChains(Injected<AssertString> instance)
    {
        RunResults results = await Tools.Runner.CallMethodsOn(instance.Dummy);
        results
            .RawResults.Where(r => r.Result != null)
            .Where(r => r.Result is not AssertChainer<AssertString>)
            .Select(r => r.Method.Name)
            .Assert()
            .IsEmpty();
    }
}
