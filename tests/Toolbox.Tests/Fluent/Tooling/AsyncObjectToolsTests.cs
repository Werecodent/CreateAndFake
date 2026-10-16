using Werecodent.CreateAndFake.FakerTool.Proxy;
using Werecodent.CreateAndFake.Fluent.Tooling;

namespace Werecodent.CreateAndFake.Tests.Fluent.Tooling;

public static class AsyncObjectToolsTests
{
    [Fact]
    internal static Task AsyncObjectTools_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(AsyncObjectTools<IFaked>),
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task AsyncObjectTools_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            typeof(AsyncObjectTools<IFaked>),
            TestContext.Current.CancellationToken
        );
    }
}
