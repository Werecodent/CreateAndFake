using Werecodent.CreateAndFake.AsserterTool;

namespace Werecodent.CreateAndFake.Tests.Fluent.TaskChainingUnwrapping;

public static class TaskExceptionChainerExtensionsTests
{
    [Fact]
    internal static Task TaskExceptionChainerExtensions_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(TaskExceptionChainerExtensions),
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = [typeof(AssertException)] }
        );
    }
}
