using CreateAndFake.AsserterTool;
using CreateAndFake.Design.Tooling;
using CreateAndFake.FakerTool;
using CreateAndFake.Fluent.AssertCalls;
using CreateAndFake.RunnerTool;

namespace CreateAndFake.Tests.Fluent.AssertCalls;

public static class AssertStringTests
{
    private static readonly TesterMod config = opt =>
        opt with
        {
            IgnorableExceptions =
            [
                typeof(AssertException),
                typeof(ToolException),
                typeof(InvalidCastException),
            ],
        };

    [Fact]
    internal static Task AssertString_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<AssertString>(config);
    }

    [Fact]
    internal static Task AssertString_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<AssertString>(config);
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
