using CreateAndFake.Design.Content;

namespace CreateAndFake.Design.Tests.Content;

public static class SingleCallValueTaskSource_T_Tests
{
    [Fact]
    internal static Task SingleCallValueTaskSource_T_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(SingleCallValueTaskSource<string>),
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = [typeof(NotSupportedException)] }
        );
    }

    [Fact]
    internal static Task SingleCallValueTaskSource_T_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            typeof(SingleCallValueTaskSource<>),
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = [typeof(NotSupportedException)] }
        );
    }
}
