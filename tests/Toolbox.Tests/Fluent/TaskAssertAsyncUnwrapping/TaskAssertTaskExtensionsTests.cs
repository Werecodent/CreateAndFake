using System.Reflection;
using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.Fluent.AssertAsyncCalls;

namespace Werecodent.CreateAndFake.Tests.Fluent.TaskAssertAsyncUnwrapping;

public static class TaskAssertTaskExtensionsTests
{
    [Fact]
    internal static Task TaskAssertTaskExtensions_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(TaskAssertTaskExtensions),
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = [typeof(AssertException)] }
        );
    }

    [Fact]
    internal static Task TaskAssertTaskExtensions_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            typeof(TaskAssertTaskExtensions),
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = [typeof(AssertException)] }
        );
    }

    [Fact]
    internal static void TaskAssertTaskExtensions_MatchesEveryMethod()
    {
        typeof(AssertTaskBase<>)
            .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
            .OrderBy(m => m.Name)
            .Select(m => m.Name)
            .Where(m => m != nameof(AssertTaskBase<>.ThrowsAsync))
            .Where(m => m != nameof(AssertTaskBase<>.ThrowsNoAsync))
            .Assert()
            .Is(
                typeof(TaskAssertTaskExtensions)
                    .GetMethods(BindingFlags.Static | BindingFlags.Public)
                    .OrderBy(m => m.Name)
                    .Select(m => m.Name)
                    .Where(m => m != nameof(TaskAssertTaskExtensions.ThrowsExceptionAsync))
                    .Where(m => m != nameof(TaskAssertTaskExtensions.ThrowsNoExceptionAsync))
            );
    }

    [Theory, RandomData]
    internal static async Task ThrowsExceptionAsync_Forwarded(Task data, Exception error)
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;
        int modCount = 0;
        AsserterOptions mod(AsserterOptions opt)
        {
            modCount++;
            return opt;
        }
        async Task thrower()
        {
            await Task.Delay(0, canceler).ConfigureAwait(false);
            throw error;
        }

        await Task.FromResult(thrower().Assert()).ThrowsExceptionAsync(canceler);
        await Task.FromResult(thrower().Assert()).ThrowsExceptionAsync(canceler, mod);
        await Task.FromResult(data.Assert())
            .ThrowsExceptionAsync(canceler)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(data.Assert())
            .ThrowsExceptionAsync(canceler, mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal static async Task ThrowsNoExceptionAsync_Forwarded(Task data, Exception error)
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;
        int modCount = 0;
        AsserterOptions mod(AsserterOptions opt)
        {
            modCount++;
            return opt;
        }
        async Task thrower()
        {
            await Task.Delay(0, canceler).ConfigureAwait(false);
            throw error;
        }

        await Task.FromResult(data.Assert()).ThrowsNoExceptionAsync(canceler);
        await Task.FromResult(data.Assert()).ThrowsNoExceptionAsync(canceler, mod);
        await Task.FromResult(thrower().Assert())
            .ThrowsNoExceptionAsync(canceler)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(thrower().Assert())
            .ThrowsNoExceptionAsync(canceler, mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        modCount.Assert().Is(2);
    }
}
