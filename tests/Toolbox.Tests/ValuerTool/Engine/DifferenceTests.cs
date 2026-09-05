using Werecodent.CreateAndFake.ValuerTool;

namespace Werecodent.CreateAndFake.Tests.ValuerTool.Engine;

public static class DifferenceTests
{
    [Fact]
    internal static Task Difference_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<Difference>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task Difference_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<Difference>(
            TestContext.Current.CancellationToken
        );
    }
}
