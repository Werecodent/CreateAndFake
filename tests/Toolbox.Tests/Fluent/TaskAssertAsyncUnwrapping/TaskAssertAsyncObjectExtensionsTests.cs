using System.Reflection;
using CreateAndFake.Fluent.AssertAsyncCalls;

namespace CreateAndFake.Tests.Fluent.TaskAssertAsyncUnwrapping;

public static class TaskAssertAsyncObjectExtensionsTests
{
    /*[Fact]
    internal static Task TaskAssertAsyncObjectExtensions_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(TaskAssertAsyncObjectExtensions),
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = [typeof(AssertException)] }
        );
    }*/

    [Fact]
    internal static void TaskAssertAsyncObjectExtensions_MatchesEveryMethod()
    {
        typeof(AssertAsyncObjectBase<>)
            .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
            .OrderBy(m => m.Name)
            .Select(m => m.Name)
            .Assert()
            .Is(
                typeof(TaskAssertAsyncObjectExtensions)
                    .GetMethods(BindingFlags.Static | BindingFlags.Public)
                    .OrderBy(m => m.Name)
                    .Select(m => m.Name)
            );
    }
}
