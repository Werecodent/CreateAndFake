using CreateAndFake.Design.Content;

namespace CreateAndFake.Design.Tests.Content;

public static class SingleCallValueTaskSourceTests
{
    [Fact]
    internal static Task SingleCallValueTaskSource_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(SingleCallValueTaskSource),
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = [typeof(NotSupportedException)] }
        );
    }

    [Fact]
    internal static Task SingleCallValueTaskSource_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            typeof(SingleCallValueTaskSource),
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = [typeof(NotSupportedException)] }
        );
    }
}
