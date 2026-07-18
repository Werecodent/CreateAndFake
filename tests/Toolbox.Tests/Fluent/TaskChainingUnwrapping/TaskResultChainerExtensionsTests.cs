using CreateAndFake.AsserterTool;

namespace CreateAndFake.Tests.Fluent.TaskChainingUnwrapping;

public static class TaskResultChainerExtensionsTests
{
    [Fact]
    internal static Task TaskResultChainerExtensions_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(TaskResultChainerExtensions),
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
