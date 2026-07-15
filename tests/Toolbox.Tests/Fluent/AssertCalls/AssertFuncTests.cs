using CreateAndFake.AsserterTool;
using CreateAndFake.Design.Exceptions;
using CreateAndFake.Design.Types;
using CreateAndFake.FakerTool;
using CreateAndFake.Fluent.AssertCalls;
using CreateAndFake.Fluent.Chaining;
using CreateAndFake.RunnerTool;

namespace CreateAndFake.Tests.Fluent.AssertCalls;

public static class AssertFuncTests
{
    private static readonly TesterMod _Config = opt =>
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
    internal static Task AssertFunc_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<AssertFunc<string>>(
            TestContext.Current.CancellationToken,
            _Config
        );
    }

    [Fact]
    internal static Task AssertFunc_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<AssertFunc<string>>(
            TestContext.Current.CancellationToken,
            _Config
        );
    }

    [Theory, RandomData]
    internal static async Task AssertFunc_CallsAndChains(Injected<AssertFunc<string>> instance)
    {
        RunResults results = await Tools.Runner.CallMethodsOnAsync(
            instance.Dummy,
            TestContext.Current.CancellationToken,
            opt => opt with { IncludeBaseObjectMethods = false }
        );
        results
            .RawResults.Where(r => r.Result != null)
            .Where(r =>
                r.Result is not AssertChainer<AssertFunc<string>>
                && !TypeDescriber.For(r.Result?.GetType()).Inherits(typeof(ResultChainer<>))
                && !TypeDescriber.For(r.Result?.GetType()).Inherits(typeof(ExceptionChainer<>))
                && r.Result is not AlsoChainer
            )
            .Assert()
            .IsEmpty();
    }
}
