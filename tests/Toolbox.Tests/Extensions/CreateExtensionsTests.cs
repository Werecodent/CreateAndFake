using CreateAndFake.Design.Exceptions;

namespace CreateAndFake.Tests.Extensions;

public static class CreateExtensionsTests
{
    private static readonly TesterMod config = opt =>
        opt with
        {
            IgnorableExceptions = [typeof(ToolException)],
        };

    [Fact]
    internal static Task CreateExtensions_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(CreateExtensions),
            TestContext.Current.CancellationToken,
            config
        );
    }

    [Fact]
    internal static Task CreateExtensions_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            typeof(CreateExtensions),
            TestContext.Current.CancellationToken,
            config
        );
    }
}
