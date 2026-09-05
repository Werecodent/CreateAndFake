using System.Reflection;
using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.Fluent.AssertCalls;

namespace Werecodent.CreateAndFake.Tests.Fluent.TaskAssertUnwrapping;

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
            .OrderBy(m => m.Name)
            .Select(m => m.Name)
            .Assert()
            .Is(
                typeof(TaskAssertFuncExtensions)
                    .GetMethods(BindingFlags.Static | BindingFlags.Public)
                    .OrderBy(m => m.Name)
                    .Select(m => m.Name)
                    .Where(n => n != nameof(TaskAssertFuncExtensions.ThrowsException))
                    .Where(n => n != nameof(TaskAssertFuncExtensions.ThrowsNoException))
            );
    }
}
