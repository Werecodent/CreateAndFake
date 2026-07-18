using CreateAndFake.AsserterTool;
using CreateAndFake.Design.Exceptions;
using CreateAndFake.Fluent.AssertAsyncCalls;

namespace CreateAndFake.Tests.Fluent.AssertAsyncCalls;

public static class AssertTaskTests
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
                typeof(ValueTaskRepeatedAccessException),
            ],
            DisableNullRefExceptionTests = true,
            DisableParameterMutationTests = true,
        };

    [Fact]
    internal static Task AssertTask_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<AssertTask>(
            TestContext.Current.CancellationToken,
            _Config
        );
    }

    [Fact]
    internal static Task AssertTask_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<AssertTask>(
            TestContext.Current.CancellationToken,
            _Config
        );
    }
}
