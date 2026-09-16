using System.Reflection;
using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.Fluent.AssertAsyncCalls;
using Werecodent.CreateAndFake.Fluent.AssertCalls;
using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Tests.Fluent.TaskAssertAsyncUnwrapping;

public static class TaskAssertAsyncObjectExtensionsTests
{
    [Fact]
    internal static Task TaskAssertAsyncObjectExtensions_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(TaskAssertAsyncObjectExtensions),
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = [typeof(AssertException)] }
        );
    }

    [Fact]
    internal static Task TaskAssertAsyncObjectExtensions_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            typeof(TaskAssertAsyncObjectExtensions),
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = [typeof(AssertException)] }
        );
    }

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
                    .Where(m => m != nameof(AssertObjectBase<>.Inherits))
                    .Where(m => m != nameof(AssertObjectBase<>.InheritedBy))
            );
    }

    [Theory, RandomData]
    internal static async Task IsAsync_Forwarded(
        AsyncDataSample data,
        [Copy] AsyncDataSample clone,
        AsyncDataSample variant
    )
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;
        int modCount = 0;
        AsserterOptions mod(AsserterOptions opt)
        {
            modCount++;
            return opt;
        }

        await Task.FromResult(data.Assert()).IsAsync(clone, canceler);
        await Task.FromResult(data.Assert()).IsAsync(clone, canceler, mod);
        await Task.FromResult(data.Assert())
            .IsAsync(variant, canceler)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(data.Assert())
            .IsAsync(variant, canceler, mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal static async Task IsNotAsync_Forwarded(
        AsyncDataSample data,
        [Copy] AsyncDataSample clone,
        AsyncDataSample variant
    )
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;
        int modCount = 0;
        AsserterOptions mod(AsserterOptions opt)
        {
            modCount++;
            return opt;
        }

        await Task.FromResult(data.Assert()).IsNotAsync(variant, canceler);
        await Task.FromResult(data.Assert()).IsNotAsync(variant, canceler, mod);
        await Task.FromResult(data.Assert())
            .IsNotAsync(clone, canceler)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(data.Assert())
            .IsNotAsync(clone, canceler, mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal static async Task ValuesEqualAsync_Forwarded(
        AsyncDataSample data,
        [Copy] AsyncDataSample clone,
        AsyncDataSample variant
    )
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;
        int modCount = 0;
        AsserterOptions mod(AsserterOptions opt)
        {
            modCount++;
            return opt;
        }

        await Task.FromResult(data.Assert()).ValuesEqualAsync(clone, canceler);
        await Task.FromResult(data.Assert()).ValuesEqualAsync(clone, canceler, mod);
        await Task.FromResult(data.Assert())
            .ValuesEqualAsync(variant, canceler)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(data.Assert())
            .ValuesEqualAsync(variant, canceler, mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal static async Task ValuesNotEqualAsync_Forwarded(
        AsyncDataSample data,
        [Copy] AsyncDataSample clone,
        AsyncDataSample variant
    )
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;
        int modCount = 0;
        AsserterOptions mod(AsserterOptions opt)
        {
            modCount++;
            return opt;
        }

        await Task.FromResult(data.Assert()).ValuesNotEqualAsync(variant, canceler);
        await Task.FromResult(data.Assert()).ValuesNotEqualAsync(variant, canceler, mod);
        await Task.FromResult(data.Assert())
            .ValuesNotEqualAsync(clone, canceler)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(data.Assert())
            .ValuesNotEqualAsync(clone, canceler, mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal static async Task UniqueFromAsync_Forwarded(
        AsyncDataSample data,
        [Copy] AsyncDataSample clone,
        [Unique] AsyncDataSample unique
    )
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;
        int modCount = 0;
        AsserterOptions mod(AsserterOptions opt)
        {
            modCount++;
            return opt;
        }

        await Task.FromResult(data.Assert()).UniqueFromAsync(unique, canceler);
        await Task.FromResult(data.Assert()).UniqueFromAsync(unique, canceler, mod);
        await Task.FromResult(data.Assert())
            .UniqueFromAsync(clone, canceler)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(data.Assert())
            .UniqueFromAsync(clone, canceler, mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        modCount.Assert().Is(2);
    }
}
