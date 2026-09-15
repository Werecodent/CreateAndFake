using System.Reflection;
using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.Fluent.AssertAsyncCalls;
using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Tests.Fluent.TaskAssertAsyncUnwrapping;

public static class TaskAssertGenericTaskExtensionsTests
{
    [Fact]
    internal static Task TaskAssertGenericTaskExtensions_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(TaskAssertGenericTaskExtensions),
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = [typeof(AssertException)] }
        );
    }

    [Fact]
    internal static Task TaskAssertGenericTaskExtensions_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            typeof(TaskAssertGenericTaskExtensions),
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = [typeof(AssertException)] }
        );
    }

    [Fact]
    internal static void TaskAssertGenericTaskExtensions_MatchesEveryMethod()
    {
        typeof(AssertGenericTaskBase<,>)
            .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
            .OrderBy(m => m.Name)
            .Select(m => m.Name)
            .Assert()
            .Is(
                typeof(TaskAssertGenericTaskExtensions)
                    .GetMethods(BindingFlags.Static | BindingFlags.Public)
                    .OrderBy(m => m.Name)
                    .Select(m => m.Name)
            );
    }

    [Theory, RandomData]
    internal static async Task HasResultAsync_WithMatchForwarded(string value, string variant)
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;
        int modCount = 0;
        AsserterOptions mod(AsserterOptions opt)
        {
            modCount++;
            return opt;
        }
        async Task<string> getValue()
        {
            await Task.Delay(0, canceler).ConfigureAwait(false);
            return value;
        }

        await Task.FromResult(getValue().Assert()).HasResultAsync(value, canceler);
        await Task.FromResult(getValue().Assert()).HasResultAsync(value, canceler, mod);
        await Task.FromResult(getValue().Assert())
            .HasResultAsync(variant, canceler)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(getValue().Assert())
            .HasResultAsync(variant, canceler, mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal static async Task HasResultAsync_Forwarded(
        Task<string> dataA,
        Task<int> dataB,
        Exception error
    )
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;
        int modCount = 0;
        AsserterOptions mod(AsserterOptions opt)
        {
            modCount++;
            return opt;
        }
        async Task<DataSample> thrower()
        {
            await Task.Delay(0, canceler).ConfigureAwait(false);
            throw error;
        }

        await Task.FromResult(dataA.Assert()).HasResultAsync(canceler);
        await Task.FromResult(dataB.Assert()).HasResultAsync(canceler, mod);
        await Task.FromResult(thrower().Assert())
            .HasResultAsync(canceler)
            .Assert()
            .ThrowsAsync<Exception>(canceler)
            .That()
            .Is(error);
        await Task.FromResult(thrower().Assert())
            .HasResultAsync(canceler, mod)
            .Assert()
            .ThrowsAsync<Exception>(canceler)
            .That()
            .Is(error);

        modCount.Assert().Is(2);
    }
}
