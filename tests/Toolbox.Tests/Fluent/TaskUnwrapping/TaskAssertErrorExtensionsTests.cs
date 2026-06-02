using System.Reflection;
using CreateAndFake.AsserterTool;
using CreateAndFake.Fluent.AssertCalls;

namespace CreateAndFake.Tests.Fluent.TaskUnwrapping;

public static class TaskAssertEnumerableExtensionsTests
{
    [Fact]
    internal static Task TaskAssertEnumerableExtensions_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(TaskAssertEnumerableExtensions),
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = [typeof(AssertException)] }
        );
    }

    [Fact]
    internal static void TaskAssertEnumerableExtensions_MatchesEveryMethod()
    {
        typeof(AssertEnumerableBase<>)
            .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
            .Select(m => m.Name)
            .Order()
            .Assert()
            .Is(
                typeof(TaskAssertEnumerableExtensions)
                    .GetMethods(BindingFlags.Static | BindingFlags.Public)
                    .Select(m => m.Name)
                    .Order()
            );
    }
}
