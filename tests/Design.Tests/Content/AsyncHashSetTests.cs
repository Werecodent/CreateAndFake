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
    internal static void Debug_AsyncHashSet_CompletedToString(AsyncHashSet<DataSample> sample)
    {
        sample.ToString().Assert().Debug();
    }

    [Theory, RandomData]
    internal static void Debug_AsyncHashSet_UncompletedToString(IList<DataSample> sample)
    {
        using CancellationTokenSource source = new();
        AsyncHashSet<DataSample>
            .CreateFromAsync(
                SlowlyIterate(sample),
                Tools.Valuer.ToAsyncComparer<DataSample>(),
                Tools.Valuer.Options.IterationLimit,
                TestContext.Current.CancellationToken
            )
            .ToString()
            .Assert()
            .Debug();
    }

    [Theory, RandomData]
    internal static Task Debug_AsyncHashSet_EnumerateString(AsyncHashSet<DataSample> sample)
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
    internal static Task CreateFromAsync_SetsInitialSyncContent(IEnumerable<DataHolderSample> list)
    {
        AsyncHashSet<DataHolderSample> set = AsyncHashSet<DataHolderSample>.CreateFromAsync(
            list,
            Tools.Valuer.ToAsyncComparer<DataHolderSample>(),
            Tools.Valuer.Options.IterationLimit,
            TestContext.Current.CancellationToken
        );

        return set.Assert().IsAsync(list, TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal static Task CreateFromAsync_SetsInitialAsyncContent(
        IAsyncEnumerable<DataHolderSample> list
    )
    {
        AsyncHashSet<DataHolderSample> set = AsyncHashSet<DataHolderSample>.CreateFromAsync(
            list,
            Tools.Valuer.ToAsyncComparer<DataHolderSample>(),
            Tools.Valuer.Options.IterationLimit,
            TestContext.Current.CancellationToken
        );

        return set.Assert().IsAsync(list, TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal static async Task ContainsAsync_UsesObjectByValue(
        DataHolderSample original,
        [Copy] DataHolderSample clone,
        DataHolderSample variant
    )
    {
        AsyncHashSet<DataHolderSample> set = AsyncHashSet<DataHolderSample>.CreateFromAsync(
            [original],
            Tools.Valuer.ToAsyncComparer<DataHolderSample>(),
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
        [Stub] IAsyncEqualityComparer<DataHolderSample> comparer,
        DataHolderSample original,
        [Copy] DataHolderSample clone,
        DataHolderSample variant,
        int valueHash,
        int otherHash
    )
    {
        CancellationToken ct = TestContext.Current.CancellationToken;

        comparer
            .GetHashCodeAsync(Arg.Any<DataHolderSample>(), Arg.Any<CancellationToken>())
            .SetupReturn(Task.FromResult(valueHash));
        comparer
            .EqualsAsync(
                Arg.Any<DataHolderSample>(),
                Arg.Any<DataHolderSample>(),
                Arg.Any<CancellationToken>()
            )
            .SetupReturn(
                Behavior.Call(
                    (DataHolderSample x, DataHolderSample y, CancellationToken t) =>
                        Tools.Valuer.EqualsAsync(x, y, t)
                )
            );

        AsyncHashSet<DataHolderSample> set = new(comparer);

        await set.ContainsAsync(original, ct).Assert().HasResultAsync(false, ct);
        await set.AddAsync(original, ct).Assert().HasResultAsync(true, ct);
        await set.ContainsAsync(original, ct).Assert().HasResultAsync(true, ct);

        await set.ContainsAsync(clone, ct).Assert().HasResultAsync(true, ct);
        await set.AddAsync(clone, ct).Assert().HasResultAsync(false, ct);

        await set.ContainsAsync(new KeyValuePair<int, DataHolderSample>(valueHash, variant), ct)
            .Assert()
            .HasResultAsync(false, ct);

        await set.ContainsAsync(variant, ct).Assert().HasResultAsync(false, ct);
        await set.AddAsync(variant, ct).Assert().HasResultAsync(true, ct);
        await set.ContainsAsync(variant, ct).Assert().HasResultAsync(true, ct);

        await set.ContainsAsync(new KeyValuePair<int, DataHolderSample>(otherHash, original), ct)
            .Assert()
            .HasResultAsync(false, ct);
        await set.ContainsAsync(new KeyValuePair<int, DataHolderSample>(valueHash, original), ct)
            .Assert()
            .HasResultAsync(true, ct);
        await set.ContainsAsync(original, ct).Assert().HasResultAsync(true, ct);
    }

    private static async IAsyncEnumerable<DataSample> SlowlyIterate(IEnumerable<DataSample> list)
    {
        foreach (DataSample sample in list)
        {
            await Task.Delay(3000, TestContext.Current.CancellationToken);
            yield return sample;
        }
    }
}
