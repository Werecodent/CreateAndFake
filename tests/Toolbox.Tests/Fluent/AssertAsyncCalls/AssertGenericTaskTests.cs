using CreateAndFake.AsserterTool;
using CreateAndFake.Design.Exceptions;
using CreateAndFake.Fluent.AssertAsyncCalls;

namespace CreateAndFake.Tests.Fluent.AssertAsyncCalls;

public static class AssertGenericTaskTests
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
        };

    [Fact]
    internal static Task AssertGenericTask_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<AssertGenericTask<string>>(
            TestContext.Current.CancellationToken,
            _Config
        );
    }

    [Fact]
    internal static Task AssertGenericTask_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<AssertGenericTask<string>>(
            TestContext.Current.CancellationToken,
            _Config
        );
    }
}
