using System.Reflection;
using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.FakerTool;
using Werecodent.CreateAndFake.Fluent.AssertCalls;
using Werecodent.CreateAndFake.Fluent.Chaining;
using Werecodent.CreateAndFake.RunnerTool;

namespace Werecodent.CreateAndFake.Tests.Fluent.AssertCalls;

public static class AssertEnumerableTests
{
    private static readonly TesterMod _Config = opt =>
        opt with
        {
            IgnorableExceptions =
            [
                typeof(AssertException),
                typeof(ToolException),
                typeof(InvalidCastException),
                typeof(UnsupportedException),
                typeof(ArgumentException),
                typeof(TargetException),
            ],
        };

    [Fact]
    internal static Task AssertEnumerable_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<AssertEnumerable>(
            TestContext.Current.CancellationToken,
            _Config
        );
    }

    [Fact]
    internal static Task AssertEnumerable_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<AssertEnumerable>(
            TestContext.Current.CancellationToken,
            _Config
        );
    }

    [Theory, RandomData]
    internal static async Task AssertEnumerable_CallsAndChains(Injected<AssertEnumerable> instance)
    {
        RunResults results = await Tools.Runner.CallMethodsOnAsync(
            instance.Dummy,
            TestContext.Current.CancellationToken
        );
        results
            .RawResults.Where(r => r.Result != null)
            .Where(r =>
                r.Result
                    is not AssertChainer<AssertEnumerable>
                        and Task<AssertChainer<AssertEnumerable>>
            )
            .Assert()
            .IsEmpty();
    }
}
