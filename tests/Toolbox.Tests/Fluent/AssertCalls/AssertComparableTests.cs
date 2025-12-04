using CreateAndFake.AsserterTool;
using CreateAndFake.Design.Tooling;
using CreateAndFake.FakerTool;
using CreateAndFake.Fluent.AssertCalls;
using CreateAndFake.RunnerTool;

namespace CreateAndFake.Tests.Fluent.AssertCalls;

public static class AssertComparableTests
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
    internal static Task AssertComparable_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<AssertComparable>(config);
    }

    [Fact]
    internal static Task AssertComparable_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<AssertComparable>(config);
    }

    [Theory, RandomData]
    internal static async Task AssertComparable_CallsAndChains(Injected<AssertComparable> instance)
    {
        RunResults results = await Tools.Runner.CallMethodsOn(instance.Dummy);
        results
            .RawResults.Where(r => r.Result != null)
            .Where(r => r.Result is not AssertChainer<AssertComparable>)
            .Select(r => r.Method.Name)
            .Assert()
            .IsEmpty();
    }
}
