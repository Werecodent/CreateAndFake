using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.Design.Extensions;
using Werecodent.CreateAndFake.FakerTool;
using Werecodent.CreateAndFake.Fluent.AssertCalls;
using Werecodent.CreateAndFake.Fluent.Chaining;
using Werecodent.CreateAndFake.RunnerTool;

namespace Werecodent.CreateAndFake.Tests.Fluent.AssertCalls;

public static class AssertErrorTests
{
    private static readonly TesterMod _Config = opt =>
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
    internal static Task AssertError_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<AssertError>(
            TestContext.Current.CancellationToken,
            _Config
        );
    }

    [Fact]
    internal static Task AssertError_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<AssertError>(
            TestContext.Current.CancellationToken,
            _Config
        );
    }

    [Theory, RandomData]
    internal static async Task AssertError_CallsAndChains(Injected<AssertError> instance)
    {
        RunResults results = await Tools.Runner.CallMethodsOnAsync(
            instance.Dummy,
            TestContext.Current.CancellationToken,
            opt => opt with { IncludeBaseObjectMethods = false }
        );
        results
            .RawResults.Where(r => r.Result != null)
            .Where(r => r.Result is not AssertChainer<AssertError>)
            .Where(r => !r.Result?.GetType().Inherits(typeof(ExceptionChainer<>)) ?? false)
            .Assert()
            .IsEmpty();
    }
}
