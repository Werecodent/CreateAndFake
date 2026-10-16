using System.Collections;
using Werecodent.CreateAndFake.Design.Content;
using Werecodent.CreateAndFake.Samples.Scenarios;
using Werecodent.CreateAndFake.ValuerTool.Engine;
using Werecodent.CreateAndFake.ValuerTool.Hints;

namespace Werecodent.CreateAndFake.Tests.ValuerTool.Hints;

public sealed class AsyncEnumerableCompareHintTests
    : CompareHintTestBase<AsyncEnumerableCompareHint>
{
    private static readonly Type[] _ValidTypes =
    [
        typeof(IAsyncEnumerable<int>),
        typeof(IAsyncEnumerable<string>),
        typeof(IAsyncEnumerable<object>),
    ];

    private static readonly Type[] _InvalidTypes = [typeof(IEnumerable), typeof(DataHolderSample)];

    protected override bool RequiresAsync { get; } = true;

    public AsyncEnumerableCompareHintTests()
        : base(_ValidTypes, _InvalidTypes) { }

    [Theory, RandomData]
    internal Task TryToAsyncCompare_CanCheckCollectionType(IAsyncEnumerable<string> data)
    {
        async IAsyncEnumerable<string> iterate()
        {
            await foreach (
                string item in data.WithCancellation(TestContext.Current.CancellationToken)
                    .ConfigureAwait(false)
            )
            {
                yield return item;
            }
        }

        return TestInstance
            .TryToAsyncCompare(
                data,
                iterate(),
                CreateChainer(Tools.Valuer.Options with { CheckCollectionType = true }),
                TestContext.Current.CancellationToken
            )
            .Data.Assert()
            .HasCountAsync(1, TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal async Task TryToAsyncCompare_NoDifferencesWhenEqualWithDifferentTypes(
        IAsyncEnumerable<string> data
    )
    {
        IEnumerable<string> data2 = await AsyncSeriesHelper.ToListAsync(
            data,
            Tools.Valuer.Options.IterationLimit,
            TestContext.Current.CancellationToken
        );

        DifferenceHintAsyncResult result = TestInstance.TryToAsyncCompare(
            data2,
            data,
            CreateChainer(),
            TestContext.Current.CancellationToken
        );

        result.HasData.Assert().Is(true);
        await result.Data.Assert().HasCountAsync(0, TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal Task TryToAsyncCompare_FindsDifferencesWithDifferentTypes(
        IAsyncEnumerable<string> data,
        IEnumerable<string> data2
    )
    {
        DifferenceHintAsyncResult result = TestInstance.TryToAsyncCompare(
            data,
            data2,
            CreateChainer(),
            TestContext.Current.CancellationToken
        );

        result.HasData.Assert().Is(true);
        return result.Data.Assert().HasCountMoreThanAsync(0, TestContext.Current.CancellationToken);
    }
}
