using CreateAndFake.FakerTool.Engine;

namespace CreateAndFake.Tests.FakerTool.Engine;

public static class FakerChainerTests
{
    [Fact]
    internal static Task FakerChainer_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<FakerChainer>(
            TestContext.Current.CancellationToken,
            opt =>
                opt with
                {
                    IgnorableExceptions =
                    [
                        typeof(ArgumentException),
                        typeof(InvalidOperationException),
                        typeof(ArgumentOutOfRangeException),
                    ],
                }
        );
    }

    [Fact]
    internal static Task FakerChainer_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<FakerChainer>(
            TestContext.Current.CancellationToken,
            opt =>
                opt with
                {
                    IgnorableExceptions =
                    [
                        typeof(ArgumentException),
                        typeof(InvalidOperationException),
                        typeof(ArgumentOutOfRangeException),
                    ],
                }
        );
    }
}
