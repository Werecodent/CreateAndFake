using CreateAndFake.ValuerTool.Engine;

namespace CreateAndFake.Tests.ValuerTool.Engine;

public static class ByValuerComparerTests
{
    [Fact]
    internal static Task ByValuerComparer_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(ByValuerComparer<string>),
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task ByValuerComparer_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            typeof(ByValuerComparer<string>),
            TestContext.Current.CancellationToken
        );
    }
}
