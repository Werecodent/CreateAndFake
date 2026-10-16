using Werecodent.CreateAndFake.ValuerTool.Engine;

namespace Werecodent.CreateAndFake.Tests.ValuerTool.Engine;

public static class ByValuerAsyncComparerTests
{
    [Fact]
    internal static Task ByValuerAsyncComparer_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(ByValuerAsyncComparer<>),
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task ByValuerAsyncComparer_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            typeof(ByValuerAsyncComparer<>),
            TestContext.Current.CancellationToken
        );
    }
}
