namespace Werecodent.CreateAndFake.Tests.Fluent;

public static class FakeExtensionsTests
{
    private static readonly TesterMod _Config = opt =>
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
            _Config
        );
    }

    [Fact]
    internal static Task FakeExtensions_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            typeof(FakeExtensions),
            TestContext.Current.CancellationToken,
            _Config
        );
    }
}
