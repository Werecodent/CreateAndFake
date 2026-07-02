using CreateAndFake.AsserterTool;
using CreateAndFake.Design.Exceptions;
using CreateAndFake.Design.Types;
using CreateAndFake.FakerTool;
using CreateAndFake.Fluent.AssertCalls;
using CreateAndFake.Fluent.Chaining;
using CreateAndFake.RunnerTool;

namespace CreateAndFake.Tests.Fluent.AssertCalls;

public static class AssertActionTests
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
    internal static Task AssertAction_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<AssertAction>(
            TestContext.Current.CancellationToken,
            config
        );
    }

    [Fact]
    internal static Task AssertAction_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<AssertAction>(
            TestContext.Current.CancellationToken,
            config
        );
    }

    [Theory, RandomData]
    internal static async Task AssertAction_CallsAndChains(Injected<AssertAction> instance)
    {
        RunResults results = await Tools.Runner.CallMethodsOnAsync(
            instance.Dummy,
            TestContext.Current.CancellationToken
        );
        results
            .RawResults.Where(r => r.Result != null)
            .Where(r =>
                r.Result is not AssertChainer<AssertAction>
                && !TypeDescriber.For(r.Result?.GetType()).Inherits(typeof(ExceptionChainer<>))
                && r.Result is not AlsoChainer
            )
            .Assert()
            .IsEmpty();
    }
}
