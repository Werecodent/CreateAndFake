using CreateAndFake.FakerTool.Hints;

namespace CreateAndFake.Tests.FakerTool.Hints;

public static class DisposableFakeHintTests
{
    [Fact]
    internal static Task DisposableFakeHint_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<DisposableFakeHint>(
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
    internal static Task DisposableFakeHint_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<DisposableFakeHint>(
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
