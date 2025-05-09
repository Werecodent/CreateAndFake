using CreateAndFake.AsserterTool.Fluent;
using CreateAndFake.FakerTool;
using CreateAndFake.RunnerTool;

namespace CreateAndFake.Tests.AsserterTool.Fluent;

public static class AssertTypeTests
{
    [Fact]
    internal static Task AssertType_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<AssertType>();
    }

    [Fact]
    internal static Task AssertType_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<AssertType>();
    }

    [Theory, RandomData]
    internal static async Task AssertType_CallsAndChains(Injected<AssertType> instance)
    {
        RunResults results = await Tools.Runner.CallMethodsOn(instance.Dummy);
        results
            .RawResults.Where(r => r.Result != null)
            .Where(r => r.Result is not Exception)
            .Where(r => r.Result is not AssertChainer<AssertType>)
            .Select(r => r.Method.Name)
            .Assert()
            .IsEmpty();
    }
}
