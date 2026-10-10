using Werecodent.CreateAndFake.FakerTool.Hints;

namespace Werecodent.CreateAndFake.Tests.FakerTool.Hints;

public static class ObjectFakeHintTests
{
    [Fact]
    internal static Task ObjectFakeHint_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<ObjectFakeHint>(
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
    internal static Task ObjectFakeHint_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<ObjectFakeHint>(
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
