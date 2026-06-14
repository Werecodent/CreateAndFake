using CreateAndFake.AsserterTool;

namespace CreateAndFake.Tests.Fluent.TaskChainingUnwrapping;

public static class ExceptionChainerExtensionsTests
{
    [Fact]
    internal static Task ExceptionChainerExtensions_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(ExceptionChainerExtensions),
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = [typeof(AssertException)] }
        );
    }
}
