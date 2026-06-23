using CreateAndFake.AsserterTool;
using CreateAndFake.Design.Exceptions;
using CreateAndFake.Fluent.AssertAsyncCalls;

namespace CreateAndFake.Tests.Fluent.AssertAsyncCalls;

public static class AssertGenericValueTaskTests
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
                typeof(ValueTaskRepeatedAccessException),
            ],
        };

    [Fact]
    internal static Task AssertGenericValueTask_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<AssertGenericValueTask<string>>(
            TestContext.Current.CancellationToken,
            config
        );
    }

    [Fact]
    internal static Task AssertGenericValueTask_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<AssertGenericValueTask<string>>(
            TestContext.Current.CancellationToken,
            config
        );
    }
}
