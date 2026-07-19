using CreateAndFake.AsserterTool;
using CreateAndFake.Design.Exceptions;

namespace CreateAndFake.Tests.Fluent.TaskAssertAsyncUnwrapping;

public static class TaskAssertGenericValueTaskExtensionsTests
{
    [Fact]
    internal static Task TaskAssertGenericValueTaskExtensions_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(TaskAssertGenericValueTaskExtensions),
            TestContext.Current.CancellationToken,
            opt =>
                opt with
                {
                    IgnorableExceptions =
                    [
                        typeof(AssertException),
                        typeof(ValueTaskRepeatedAccessException),
                    ],
                }
        );
    }
}
