using CreateAndFake.AsserterTool;
using CreateAndFake.Design.Exceptions;
using CreateAndFake.FakerTool;
using CreateAndFake.Fluent.AssertCalls;
using CreateAndFake.Fluent.Chaining;
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
                typeof(ArgumentException),
            ],
        };

    [Fact]
    internal static Task AssertComparable_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<AssertComparable>(
            TestContext.Current.CancellationToken,
            config
        );
    }

    [Fact]
    internal static Task AssertComparable_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<AssertComparable>(
            TestContext.Current.CancellationToken,
            config
        );
    }

    [Theory, RandomData]
    internal static async Task AssertComparable_CallsAndChains(Injected<AssertComparable> instance)
    {
        RunResults results = await Tools.Runner.CallMethodsOnAsync(
            instance.Dummy,
            TestContext.Current.CancellationToken
        );
        results
            .RawResults.Where(r => r.Result != null)
            .Where(r => r.Result is not AssertChainer<AssertComparable>)
            .Assert()
            .IsEmpty();
    }
}
