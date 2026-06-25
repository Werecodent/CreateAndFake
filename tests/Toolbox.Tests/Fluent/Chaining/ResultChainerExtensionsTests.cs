using CreateAndFake.AsserterTool;

namespace CreateAndFake.Tests.Fluent.Chaining;

public static class ResultChainerExtensionsTests
{
    [Fact]
    internal static Task ResultChainerExtensions_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(ResultChainerExtensions),
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = [typeof(AssertException)] }
        );
    }
}
