using System.Reflection;
using CreateAndFake.AsserterTool;
using CreateAndFake.Fluent.AssertCalls;

namespace CreateAndFake.Tests.Fluent.TaskAssertUnwrapping;

public static class TaskAssertStringExtensionsTests
{
    [Fact]
    internal static Task TaskAssertStringExtensions_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(TaskAssertStringExtensions),
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = [typeof(AssertException)] }
        );
    }

    [Fact]
    internal static void TaskAssertStringExtensions_MatchesEveryMethod()
    {
        typeof(AssertStringBase<>)
            .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
            .Select(m => m.Name)
            .Order()
            .Assert()
            .Is(
                typeof(TaskAssertStringExtensions)
                    .GetMethods(BindingFlags.Static | BindingFlags.Public)
                    .Select(m => m.Name)
                    .Order()
            );
    }
}
