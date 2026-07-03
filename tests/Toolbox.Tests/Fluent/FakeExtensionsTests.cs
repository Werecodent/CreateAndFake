namespace CreateAndFake.Tests.Fluent;

public static class FakeExtensionsTests
{
    private static readonly TesterMod config = opt =>
        opt with
        {
            IgnorableExceptions = [typeof(InvalidOperationException), typeof(InvalidCastException)],
        };

    [Fact]
    internal static Task FakeExtensions_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(FakeExtensions),
            TestContext.Current.CancellationToken,
            config
        );
    }

    [Fact]
    internal static Task FakeExtensions_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            typeof(FakeExtensions),
            TestContext.Current.CancellationToken,
            config
        );
    }
}
