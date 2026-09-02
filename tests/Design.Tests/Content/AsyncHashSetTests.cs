using Werecodent.CreateAndFake.Design.Comparisons;
using Werecodent.CreateAndFake.Design.Content;
using Werecodent.CreateAndFake.FakerTool;
using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Design.Tests.Content;

public static class AsyncHashSetTests
{
    [Fact]
    internal static void Debug_AsyncHashSet_ToString()
    {
        Tools.Randomizer.Create(typeof(AsyncHashSet<>)).Assert().Debug();
    }

    [Theory, RandomData]
    internal static void Debug_AsyncHashSet_CompletedToString(AsyncHashSet<AsyncDataSample> sample)
    {
        sample.ToString().Assert().Debug();
    }

    [Fact]
    internal static void Debug_AsyncHashSet_EmptyToString()
    {
        new AsyncHashSet<AsyncDataSample>(Tools.Valuer.ToAsyncComparer<AsyncDataSample>())
            .ToString()
            .Assert()
            .Debug();
    }

    [Theory, RandomData]
    internal static void Debug_AsyncHashSet_UncompletedToString(IList<AsyncDataSample> sample)
    {
        using CancellationTokenSource source = new();
        AsyncHashSet<AsyncDataSample>
            .CreateFromAsync(
                SlowlyIterate(sample),
                Tools.Valuer.ToAsyncComparer<AsyncDataSample>(),
                Tools.Valuer.Options.IterationLimit,
                TestContext.Current.CancellationToken
            )
            .ToString()
            .Assert()
            .Debug();
    }

    [Theory, RandomData]
    internal static Task Debug_AsyncHashSet_EnumerateString(AsyncHashSet<AsyncDataSample> sample)
    {
        return sample.Assert().DebugAsync(TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal static Task AsyncHashSet_GuardsNulls([Cap(6, 9)] int iterationLimit)
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(AsyncHashSet<>),
            TestContext.Current.CancellationToken,
            opt => opt with { InjectionValues = [iterationLimit] }
        );
    }

    [Theory, RandomData]
    internal static Task AsyncHashSet_NoParameterMutation([Cap(7, 9)] int iterationLimit)
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            typeof(AsyncHashSet<>),
            TestContext.Current.CancellationToken,
            opt => opt with { InjectionValues = [iterationLimit] }
        );
    }

    [Theory, RandomData]
    internal static Task CreateFromAsync_SetsInitialSyncContent(IEnumerable<AsyncDataSample> list)
    {
        AsyncHashSet<AsyncDataSample> set = AsyncHashSet<AsyncDataSample>.CreateFromAsync(
            list,
            Tools.Valuer.ToAsyncComparer<AsyncDataSample>(),
            Tools.Valuer.Options.IterationLimit,
            TestContext.Current.CancellationToken
        );

        return set.Assert().IsAsync(list, TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal static Task CreateFromAsync_SetsInitialAsyncContent(
        IAsyncEnumerable<AsyncDataSample> list
    )
    {
        AsyncHashSet<AsyncDataSample> set = AsyncHashSet<AsyncDataSample>.CreateFromAsync(
            list,
            Tools.Valuer.ToAsyncComparer<AsyncDataSample>(),
            Tools.Valuer.Options.IterationLimit,
            TestContext.Current.CancellationToken
        );

        return set.Assert().IsAsync(list, TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal static Task CreateFromAsync_SameHashAdded(KeyValuePair<int, AsyncDataSample> pair)
    {
        AsyncHashSet<AsyncDataSample> set = AsyncHashSet<AsyncDataSample>.CreateFromAsync(
            AsyncSeriesHelper.CreateFromAsync(
                [pair, pair],
                2,
                TestContext.Current.CancellationToken
            ),
            Tools.Valuer.ToAsyncComparer<AsyncDataSample>(),
            Tools.Valuer.Options.IterationLimit,
            TestContext.Current.CancellationToken
        );

        return set.Assert().HasCountAsync(2, TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal static async Task ContainsAsync_UsesObjectByValue(
        AsyncDataSample original,
        [Copy] AsyncDataSample clone,
        AsyncDataSample variant
    )
    {
        AsyncHashSet<AsyncDataSample> set = AsyncHashSet<AsyncDataSample>.CreateFromAsync(
            [original],
            Tools.Valuer.ToAsyncComparer<AsyncDataSample>(),
            Tools.Valuer.Options.IterationLimit,
            TestContext.Current.CancellationToken
        );

        await set.ContainsKeyAsync(
                await Tools.Valuer.GetHashCodeAsync(clone, TestContext.Current.CancellationToken),
                TestContext.Current.CancellationToken
            )
            .Assert()
            .HasResultAsync(true, TestContext.Current.CancellationToken);

        await set.ContainsAsync(clone, TestContext.Current.CancellationToken)
            .Assert()
            .HasResultAsync(true, TestContext.Current.CancellationToken);

        await set.ContainsAsync(variant, TestContext.Current.CancellationToken)
            .Assert()
            .HasResultAsync(false, TestContext.Current.CancellationToken);
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
        CancellationToken ct = TestContext.Current.CancellationToken;

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

        await set.ContainsAsync(original, ct).Assert().HasResultAsync(false, ct);
        await set.AddAsync(original, ct).Assert().HasResultAsync(true, ct);
        await set.ContainsAsync(original, ct).Assert().HasResultAsync(true, ct);

        await set.ContainsAsync(clone, ct).Assert().HasResultAsync(true, ct);
        await set.AddAsync(clone, ct).Assert().HasResultAsync(false, ct);

        await set.ContainsAsync(new KeyValuePair<int, AsyncDataSample>(valueHash, variant), ct)
            .Assert()
            .HasResultAsync(false, ct);

        await set.ContainsAsync(variant, ct).Assert().HasResultAsync(false, ct);
        await set.AddAsync(variant, ct).Assert().HasResultAsync(true, ct);
        await set.ContainsAsync(variant, ct).Assert().HasResultAsync(true, ct);

        await set.ContainsAsync(new KeyValuePair<int, AsyncDataSample>(otherHash, original), ct)
            .Assert()
            .HasResultAsync(false, ct);
        await set.ContainsAsync(new KeyValuePair<int, AsyncDataSample>(valueHash, original), ct)
            .Assert()
            .HasResultAsync(true, ct);
        await set.ContainsAsync(original, ct).Assert().HasResultAsync(true, ct);
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
        List<AsyncDataSample> shared = [sampleA, sampleD];

        AsyncHashSet<AsyncDataSample> set1 = AsyncHashSet<AsyncDataSample>.CreateFromAsync(
            AsyncSeriesHelper.CreateFromAsync(
                [
                    new KeyValuePair<int, AsyncDataSample>(hashABC, sampleA),
                    new KeyValuePair<int, AsyncDataSample>(hashABC, sampleB),
                    new KeyValuePair<int, AsyncDataSample>(hashD, sampleD),
                    new KeyValuePair<int, AsyncDataSample>(hashE, sampleE),
                ],
                5,
                TestContext.Current.CancellationToken
            ),
            Tools.Valuer.ToAsyncComparer<AsyncDataSample>(),
            5,
            TestContext.Current.CancellationToken
        );

        AsyncHashSet<AsyncDataSample> set2 = AsyncHashSet<AsyncDataSample>.CreateFromAsync(
            AsyncSeriesHelper.CreateFromAsync(
                [
                    new KeyValuePair<int, AsyncDataSample>(hashABC, sampleA),
                    new KeyValuePair<int, AsyncDataSample>(hashABC, sampleC),
                    new KeyValuePair<int, AsyncDataSample>(hashD, sampleD),
                    new KeyValuePair<int, AsyncDataSample>(hashF, sampleF),
                ],
                5,
                TestContext.Current.CancellationToken
            ),
            Tools.Valuer.ToAsyncComparer<AsyncDataSample>(),
            5,
            TestContext.Current.CancellationToken
        );

        return set1.FindMatchesInAsync(set2, TestContext.Current.CancellationToken)
            .Assert()
            .IsAsync(shared, TestContext.Current.CancellationToken);
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
        List<AsyncDataSample> missing1 = [sampleC, sampleF];
        List<AsyncDataSample> missing2 = [sampleB, sampleE];

        AsyncHashSet<AsyncDataSample> set1 = AsyncHashSet<AsyncDataSample>.CreateFromAsync(
            AsyncSeriesHelper.CreateFromAsync(
                [
                    new KeyValuePair<int, AsyncDataSample>(hashABC, sampleA),
                    new KeyValuePair<int, AsyncDataSample>(hashABC, sampleB),
                    new KeyValuePair<int, AsyncDataSample>(hashD, sampleD),
                    new KeyValuePair<int, AsyncDataSample>(hashE, sampleE),
                ],
                5,
                TestContext.Current.CancellationToken
            ),
            Tools.Valuer.ToAsyncComparer<AsyncDataSample>(),
            5,
            TestContext.Current.CancellationToken
        );

        AsyncHashSet<AsyncDataSample> set2 = AsyncHashSet<AsyncDataSample>.CreateFromAsync(
            AsyncSeriesHelper.CreateFromAsync(
                [
                    new KeyValuePair<int, AsyncDataSample>(hashABC, sampleA),
                    new KeyValuePair<int, AsyncDataSample>(hashABC, sampleC),
                    new KeyValuePair<int, AsyncDataSample>(hashD, sampleD),
                    new KeyValuePair<int, AsyncDataSample>(hashF, sampleF),
                ],
                5,
                TestContext.Current.CancellationToken
            ),
            Tools.Valuer.ToAsyncComparer<AsyncDataSample>(),
            5,
            TestContext.Current.CancellationToken
        );

        return set1.FindMissingFromAsync(set2, TestContext.Current.CancellationToken)
            .Assert()
            .IsAsync(missing1, TestContext.Current.CancellationToken)
            .Also(set2.FindMissingFromAsync(set1, TestContext.Current.CancellationToken))
            .IsAsync(missing2, TestContext.Current.CancellationToken);
    }

    [Fact]
    internal static Task IterateAsync_EmptyWorks()
    {
        return new AsyncHashSet<AsyncDataSample>(Tools.Valuer.ToAsyncComparer<AsyncDataSample>())
            .Assert()
            .HasCountAsync(0, TestContext.Current.CancellationToken);
    }

    private static async IAsyncEnumerable<AsyncDataSample> SlowlyIterate(
        IEnumerable<AsyncDataSample> list
    )
    {
        foreach (AsyncDataSample sample in list)
        {
            await Task.Delay(3000, TestContext.Current.CancellationToken);
            yield return sample;
        }
    }
}
