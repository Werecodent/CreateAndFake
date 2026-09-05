using Werecodent.CreateAndFake.Fluent.Chaining;

namespace Werecodent.CreateAndFake.Tests.Fluent.Chaining;

public static class ResultChainerTests
{
    [Fact]
    internal static Task ResultChainer_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(ResultChainer<>),
            TestContext.Current.CancellationToken
        );
    }
}
