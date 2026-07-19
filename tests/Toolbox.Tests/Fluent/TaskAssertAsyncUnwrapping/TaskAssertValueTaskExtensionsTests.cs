using CreateAndFake.AsserterTool;

namespace CreateAndFake.Tests.Fluent.TaskAssertAsyncUnwrapping;

public static class TaskAssertValueTaskExtensionsTests
{
    [Fact]
    internal static Task TaskAssertValueTaskExtensions_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(TaskAssertValueTaskExtensions),
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = [typeof(AssertException)] }
        );
    }
}
