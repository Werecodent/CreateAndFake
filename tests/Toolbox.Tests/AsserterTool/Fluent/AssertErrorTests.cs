using CreateAndFake.AsserterTool.Fluent;
using CreateAndFake.FakerTool;
using CreateAndFake.RunnerTool;

namespace CreateAndFake.Tests.AsserterTool.Fluent;

public static class AssertErrorTests
{
    [Fact]
    internal static Task AssertError_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<AssertError>();
    }

    [Fact]
    internal static Task AssertError_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<AssertError>();
    }

    [Theory, RandomData]
    internal static async Task AssertError_CallsAndChains(Injected<AssertError> instance)
    {
        RunResults results = await Tools.Runner.CallMethodsOn(instance.Dummy);
        results
            .RawResults.Where(r => r.Result != null)
            .Where(r => r.Result is not AssertChainer<AssertError>)
            .Select(r => r.Method.Name)
            .Assert()
            .IsEmpty();
    }
}
