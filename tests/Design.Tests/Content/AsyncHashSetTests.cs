using Werecodent.CreateAndFake.Design.Content;
using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Design.Tests.Content;

public static class AsyncHashSetTests
{
    [Theory, RandomData]
    internal static Task AsyncHashSet_GuardsNulls([Cap(6, 9)] int iterationLimit)
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(AsyncHashSet),
            TestContext.Current.CancellationToken,
            opt => opt with { InjectionValues = [iterationLimit] }
        );
    }

    [Theory, RandomData]
    internal static Task AsyncHashSet_NoParameterMutation([Cap(7, 9)] int iterationLimit)
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            typeof(AsyncHashSet),
            TestContext.Current.CancellationToken,
            opt => opt with { InjectionValues = [iterationLimit] }
        );
    }

    [Theory, RandomData]
    internal static Task CreateFromAsync_SetsInitialSyncContent(IEnumerable<AsyncDataSample> list)
    {
        AsyncHashSet<AsyncDataSample> set = AsyncHashSet.CreateFromAsync(
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
        AsyncHashSet<AsyncDataSample> set = AsyncHashSet.CreateFromAsync(
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
        CancellationToken canceler = TestContext.Current.CancellationToken;

        AsyncHashSet<AsyncDataSample> set = AsyncHashSet.CreateFromAsync(
            AsyncSeriesHelper.CreateFromAsync([pair, pair], 2, canceler),
            Tools.Valuer.ToAsyncComparer<AsyncDataSample>(),
            Tools.Valuer.Options.IterationLimit,
            canceler
        );

        return set.Assert().HasCountAsync(2, canceler);
    }
}
