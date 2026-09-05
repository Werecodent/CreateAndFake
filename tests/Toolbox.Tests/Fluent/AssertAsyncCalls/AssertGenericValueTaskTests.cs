using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.Fluent.AssertAsyncCalls;

namespace Werecodent.CreateAndFake.Tests.Fluent.AssertAsyncCalls;

public static class AssertGenericValueTaskTests
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
    internal static Task AssertGenericValueTask_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<AssertGenericValueTask<string>>(
            TestContext.Current.CancellationToken,
            _Config
        );
    }

    [Fact]
    internal static Task AssertGenericValueTask_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<AssertGenericValueTask<string>>(
            TestContext.Current.CancellationToken,
            _Config
        );
    }
}
