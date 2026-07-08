using CreateAndFake.Design.Exceptions;

namespace CreateAndFake.Tests.Fluent;

public static class ToolingExtensionsTests
{
    private static readonly TesterMod _Config = opt =>
        opt with
        {
            IgnorableExceptions = [typeof(ToolException)],
        };

    [Fact]
    internal static Task ToolingExtensions_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(ToolingExtensions),
            TestContext.Current.CancellationToken,
            _Config
        );
    }

    [Fact]
    internal static Task ToolingExtensions_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            typeof(ToolingExtensions),
            TestContext.Current.CancellationToken,
            _Config
        );
    }
}
