using CreateAndFake.FakerTool.Hints;

namespace CreateAndFake.Tests.FakerTool.Hints;

public static class AsyncDisposableFakeHintTests
{
    [Fact]
    internal static Task AsyncDisposableFakeHint_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<AsyncDisposableFakeHint>(
            TestContext.Current.CancellationToken,
            opt =>
                opt with
                {
                    IgnorableExceptions =
                    [
                        typeof(ArgumentException),
                        typeof(NotSupportedException),
                    ],
                }
        );
    }

    [Fact]
    internal static Task AsyncDisposableFakeHint_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<AsyncDisposableFakeHint>(
            TestContext.Current.CancellationToken,
            opt =>
                opt with
                {
                    IgnorableExceptions =
                    [
                        typeof(ArgumentException),
                        typeof(NotSupportedException),
                    ],
                }
        );
    }
}
