using CreateAndFake.AsserterTool;

namespace CreateAndFake.Tests.Fluent.TaskChainingUnwrapping;

public static class TaskAssertChainerExtensionsTests
{
    [Fact]
    internal static Task TaskAssertChainerExtensions_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(TaskAssertChainerExtensions),
            TestContext.Current.CancellationToken,
            opt =>
                opt with
                {
                    IgnorableExceptions = [typeof(AssertException)],
                    DisableNullRefExceptionTests = true,
                }
        );
    }
}
