using System.Reflection;
using CreateAndFake.AsserterTool;
using CreateAndFake.Fluent.AssertCalls;

namespace CreateAndFake.Tests.Fluent.TaskAssertUnwrapping;

public static class TaskAssertFuncExtensionsTests
{
    [Fact]
    internal static Task TaskAssertFuncExtensions_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(TaskAssertFuncExtensions),
            TestContext.Current.CancellationToken,
            opt =>
                opt with
                {
                    IgnorableExceptions = [typeof(AssertException), typeof(ArgumentException)],
                }
        );
    }

    [Fact]
    internal static void TaskAssertFuncExtensions_MatchesEveryMethod()
    {
        typeof(AssertFuncBase<,>)
            .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
            .Select(m => m.Name)
            .Order()
            .Assert()
            .Is(
                typeof(TaskAssertFuncExtensions)
                    .GetMethods(BindingFlags.Static | BindingFlags.Public)
                    .Select(m => m.Name)
                    .Where(n => n != nameof(TaskAssertFuncExtensions.ThrowsException))
                    .Where(n => n != nameof(TaskAssertFuncExtensions.ThrowsNoException))
                    .Order()
            );
    }
}
