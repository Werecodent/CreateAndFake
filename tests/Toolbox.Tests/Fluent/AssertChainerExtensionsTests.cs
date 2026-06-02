using CreateAndFake.AsserterTool;

namespace CreateAndFake.Tests.Fluent;

public static class AssertChainerExtensionsTests
{
    [Fact]
    internal static Task AssertChainerExtensions_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(TaskAssertTypeExtensions),
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = [typeof(AssertException)] }
        );
    }
}
