using System.Reflection;
using CreateAndFake.AsserterTool;
using CreateAndFake.Fluent.AssertCalls;

namespace CreateAndFake.Tests.Fluent.TaskUnwrapping;

public static class TaskAssertComparableExtensionsTests
{
    [Fact]
    internal static Task TaskAssertComparableExtensions_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(TaskAssertComparableExtensions),
            TestContext.Current.CancellationToken,
            opt =>
                opt with
                {
                    IgnorableExceptions = [typeof(AssertException), typeof(ArgumentException)],
                }
        );
    }

    [Fact]
    internal static void TaskAssertComparableExtensions_MatchesEveryMethod()
    {
        typeof(AssertComparableBase<>)
            .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
            .Select(m => m.Name)
            .Order()
            .Assert()
            .Is(
                typeof(TaskAssertComparableExtensions)
                    .GetMethods(BindingFlags.Static | BindingFlags.Public)
                    .Select(m => m.Name)
                    .Order()
            );
    }
}
