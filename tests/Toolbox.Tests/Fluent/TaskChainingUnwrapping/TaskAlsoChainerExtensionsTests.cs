using CreateAndFake.AsserterTool;

namespace CreateAndFake.Tests.Fluent.TaskChainingUnwrapping;

public static class TaskAlsoChainerExtensionsTests
{
    [Fact]
    internal static Task TaskAlsoChainerExtensions_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(TaskAlsoChainerExtensions),
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = [typeof(AssertException)] }
        );
    }
}
