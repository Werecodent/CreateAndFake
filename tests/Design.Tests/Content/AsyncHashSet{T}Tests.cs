using Werecodent.CreateAndFake.Design.Comparisons;
using Werecodent.CreateAndFake.Design.Content;
using Werecodent.CreateAndFake.FakerTool;
using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Design.Tests.Content;

public static class AsyncHashSet_T_Tests
{
    [Fact]
    internal static void Debug_AsyncHashSet_T_ToString()
    {
        Tools.Randomizer.Create(typeof(AsyncHashSet<>)).Assert().Debug();
    }

    [Theory, RandomData]
    internal static void Debug_AsyncHashSet_T_CompletedToString(
        AsyncHashSet<AsyncDataSample> sample
    )
    {
        sample.ToString().Assert().Debug();
    }

    [Fact]
    internal static void Debug_AsyncHashSet_T_EmptyToString()
    {
        new AsyncHashSet<AsyncDataSample>(Tools.Valuer.ToAsyncComparer<AsyncDataSample>())
            .ToString()
            .Assert()
            .Debug();
    }

    [Theory, RandomData]
    internal static void Debug_AsyncHashSet_T_UncompletedToString(IList<AsyncDataSample> sample)
    {
        async IAsyncEnumerable<AsyncDataSample> slowlyIterate()
        {
            foreach (AsyncDataSample item in sample)
            {
                await Task.Delay(3000, TestContext.Current.CancellationToken);
                yield return item;
            }
        }

        using CancellationTokenSource source = new();

        AsyncHashSet
            .CreateFromAsync(
                slowlyIterate(),
                Tools.Valuer.ToAsyncComparer<AsyncDataSample>(),
                Tools.Valuer.Options.IterationLimit,
                TestContext.Current.CancellationToken
            )
            .ToString()
            .Assert()
            .Debug();
    }

    [Theory, RandomData]
    internal static Task Debug_AsyncHashSet_T_EnumerateString(AsyncHashSet<AsyncDataSample> sample)
    {
        return sample.Assert().DebugAsync(TestContext.Current.CancellationToken);
    }

    [Fact]
    internal static Task AsyncHashSet_T_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(AsyncHashSet<>).Tools().CreateRandomInstance(),
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task AsyncHashSet_T_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            typeof(AsyncHashSet<>).Tools().CreateRandomInstance(),
            TestContext.Current.CancellationToken,
            opt => opt with { MethodsToIgnore = [nameof(AsyncHashSet<>.AddToAsync)] }
        );
    }

    [Theory, RandomData]
    internal static async Task ContainsAsync_UsesObjectByValue(
        AsyncDataSample original,
        [Copy] AsyncDataSample clone,
        AsyncDataSample variant
    )
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;

        AsyncHashSet<AsyncDataSample> set = AsyncHashSet.CreateFromAsync(
            [original],
            Tools.Valuer.ToAsyncComparer<AsyncDataSample>(),
            Tools.Valuer.Options.IterationLimit,
            canceler
        );

        await set.ContainsKeyAsync(await Tools.Valuer.GetHashCodeAsync(clone, canceler), canceler)
            .Assert()
            .HasResultAsync(true, canceler);

        await set.ContainsAsync(clone, canceler).Assert().HasResultAsync(true, canceler);

        await set.ContainsAsync(variant, canceler).Assert().HasResultAsync(false, canceler);
    }

    [Theory, RandomData]
    internal static async Task ContainsAsync_SameValueHashOkay(
        [Stub] IAsyncEqualityComparer<AsyncDataSample> comparer,
        AsyncDataSample original,
        [Copy] AsyncDataSample clone,
        AsyncDataSample variant,
        int valueHash,
        int otherHash
    )
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;

        comparer
            .GetHashCodeAsync(Arg.Any<AsyncDataSample>(), Arg.Any<CancellationToken>())
            .SetupReturn(Task.FromResult(valueHash));
        comparer
            .EqualsAsync(
                Arg.Any<AsyncDataSample>(),
                Arg.Any<AsyncDataSample>(),
                Arg.Any<CancellationToken>()
            )
            .SetupReturn(
                Behavior.Call(
                    (AsyncDataSample x, AsyncDataSample y, CancellationToken t) =>
                        Tools.Valuer.EqualsAsync(x, y, t)
                )
            );

        AsyncHashSet<AsyncDataSample> set = new(comparer);

        await set.ContainsAsync(original, canceler).Assert().HasResultAsync(false, canceler);
        await set.AddAsync(original, canceler).Assert().HasResultAsync(true, canceler);
        await set.ContainsAsync(original, canceler).Assert().HasResultAsync(true, canceler);

        await set.ContainsAsync(clone, canceler).Assert().HasResultAsync(true, canceler);
        await set.AddAsync(clone, canceler).Assert().HasResultAsync(false, canceler);

        await set.ContainsAsync(
                new KeyValuePair<int, AsyncDataSample>(valueHash, variant),
                canceler
            )
            .Assert()
            .HasResultAsync(false, canceler);

        await set.ContainsAsync(variant, canceler).Assert().HasResultAsync(false, canceler);
        await set.AddAsync(variant, canceler).Assert().HasResultAsync(true, canceler);
        await set.ContainsAsync(variant, canceler).Assert().HasResultAsync(true, canceler);

        await set.ContainsAsync(
                new KeyValuePair<int, AsyncDataSample>(otherHash, original),
                canceler
            )
            .Assert()
            .HasResultAsync(false, canceler);
        await set.ContainsAsync(
                new KeyValuePair<int, AsyncDataSample>(valueHash, original),
                canceler
            )
            .Assert()
            .HasResultAsync(true, canceler);
        await set.ContainsAsync(original, canceler).Assert().HasResultAsync(true, canceler);
    }

    [Theory, RandomData]
    public static Task FindMatchesInAsync_FindsMatches(
        AsyncDataSample sampleA,
        AsyncDataSample sampleB,
        AsyncDataSample sampleC,
        AsyncDataSample sampleD,
        AsyncDataSample sampleE,
        AsyncDataSample sampleF,
        int hashABC,
        int hashD,
        int hashE,
        int hashF
    )
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;

        List<AsyncDataSample> shared = [sampleA, sampleD];

        AsyncHashSet<AsyncDataSample> set1 = AsyncHashSet.CreateFromAsync(
            AsyncSeriesHelper.CreateFromAsync(
                [
                    new KeyValuePair<int, AsyncDataSample>(hashABC, sampleA),
                    new KeyValuePair<int, AsyncDataSample>(hashABC, sampleB),
                    new KeyValuePair<int, AsyncDataSample>(hashD, sampleD),
                    new KeyValuePair<int, AsyncDataSample>(hashE, sampleE),
                ],
                5,
                canceler
            ),
            Tools.Valuer.ToAsyncComparer<AsyncDataSample>(),
            5,
            canceler
        );

        AsyncHashSet<AsyncDataSample> set2 = AsyncHashSet.CreateFromAsync(
            AsyncSeriesHelper.CreateFromAsync(
                [
                    new KeyValuePair<int, AsyncDataSample>(hashABC, sampleA),
                    new KeyValuePair<int, AsyncDataSample>(hashABC, sampleC),
                    new KeyValuePair<int, AsyncDataSample>(hashD, sampleD),
                    new KeyValuePair<int, AsyncDataSample>(hashF, sampleF),
                ],
                5,
                canceler
            ),
            Tools.Valuer.ToAsyncComparer<AsyncDataSample>(),
            5,
            canceler
        );

        return set1.FindMatchesInAsync(set2, canceler).Assert().IsAsync(shared, canceler);
    }

    [Theory, RandomData]
    public static Task FindMissingFromAsync_FindsMissing(
        AsyncDataSample sampleA,
        AsyncDataSample sampleB,
        AsyncDataSample sampleC,
        AsyncDataSample sampleD,
        AsyncDataSample sampleE,
        AsyncDataSample sampleF,
        int hashABC,
        int hashD,
        int hashE,
        int hashF
    )
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;

        List<AsyncDataSample> missing1 = [sampleC, sampleF];
        List<AsyncDataSample> missing2 = [sampleB, sampleE];

        AsyncHashSet<AsyncDataSample> set1 = AsyncHashSet.CreateFromAsync(
            AsyncSeriesHelper.CreateFromAsync(
                [
                    new KeyValuePair<int, AsyncDataSample>(hashABC, sampleA),
                    new KeyValuePair<int, AsyncDataSample>(hashABC, sampleB),
                    new KeyValuePair<int, AsyncDataSample>(hashD, sampleD),
                    new KeyValuePair<int, AsyncDataSample>(hashE, sampleE),
                ],
                5,
                canceler
            ),
            Tools.Valuer.ToAsyncComparer<AsyncDataSample>(),
            5,
            canceler
        );

        AsyncHashSet<AsyncDataSample> set2 = AsyncHashSet.CreateFromAsync(
            AsyncSeriesHelper.CreateFromAsync(
                [
                    new KeyValuePair<int, AsyncDataSample>(hashABC, sampleA),
                    new KeyValuePair<int, AsyncDataSample>(hashABC, sampleC),
                    new KeyValuePair<int, AsyncDataSample>(hashD, sampleD),
                    new KeyValuePair<int, AsyncDataSample>(hashF, sampleF),
                ],
                5,
                canceler
            ),
            Tools.Valuer.ToAsyncComparer<AsyncDataSample>(),
            5,
            canceler
        );

        return set1.FindMissingFromAsync(set2, canceler)
            .Assert()
            .IsAsync(missing1, canceler)
            .Also(() => set2.FindMissingFromAsync(set1, canceler))
            .IsAsync(missing2, canceler);
    }

    [Fact]
    internal static Task GetAsyncEnumerator_EmptyWorks()
    {
        return new AsyncHashSet<AsyncDataSample>(Tools.Valuer.ToAsyncComparer<AsyncDataSample>())
            .Assert()
            .HasCountAsync(0, TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal static Task GetAsyncEnumerator_Cancelable([Size(1)] List<AsyncDataSample> items)
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;

        return AsyncHashSet
            .CreateFromAsync(items, Tools.Valuer.ToAsyncComparer<AsyncDataSample>(), 1, canceler)
            .GetAsyncEnumerator(new CancellationToken(true))
            .MoveNextAsync()
            .Assert()
            .ThrowsAsync<OperationCanceledException>(canceler);
    }

    [Fact]
    internal static Task IterateAsync_EmptyWorks()
    {
        return new AsyncHashSet<AsyncDataSample>(Tools.Valuer.ToAsyncComparer<AsyncDataSample>())
            .IterateAsync(TestContext.Current.CancellationToken)
            .Assert()
            .HasCountAsync(0, TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal static Task IterateAsync_Cancelable([Size(1)] List<AsyncDataSample> items)
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;

        return AsyncHashSet
            .CreateFromAsync(items, Tools.Valuer.ToAsyncComparer<AsyncDataSample>(), 1, canceler)
            .IterateAsync(new CancellationToken(true))
            .Assert()
            .ThrowsAsync<OperationCanceledException>(canceler);
    }
}
