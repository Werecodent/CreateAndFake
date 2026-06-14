using System.Reflection;
using CreateAndFake.AsserterTool;
using CreateAndFake.Fluent.AssertCalls;

namespace CreateAndFake.Tests.Fluent.TaskAssertUnwrapping;

public static class TaskAssertDelegateExtensionsTests
{
    [Fact]
    internal static Task TaskAssertDelegateExtensions_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(TaskAssertDelegateExtensions),
            TestContext.Current.CancellationToken,
            opt =>
                opt with
                {
                    IgnorableExceptions = [typeof(AssertException), typeof(ArgumentException)],
                }
        );
    }

    [Fact]
    internal static void TaskAssertDelegateExtensions_MatchesEveryMethod()
    {
        typeof(AssertDelegateBase<>)
            .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
            .Select(m => m.Name)
            .Order()
            .Assert()
            .Is(
                typeof(TaskAssertDelegateExtensions)
                    .GetMethods(BindingFlags.Static | BindingFlags.Public)
                    .Select(m => m.Name)
                    .Order()
            );
    }
}
