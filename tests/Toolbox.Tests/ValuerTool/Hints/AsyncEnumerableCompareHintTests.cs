using System.Collections;
using Werecodent.CreateAndFake.Design.Content;
using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.Samples.Scenarios;
using Werecodent.CreateAndFake.ValuerTool.Engine;
using Werecodent.CreateAndFake.ValuerTool.Hints;

namespace Werecodent.CreateAndFake.Tests.ValuerTool.Hints;

public sealed class AsyncEnumerableCompareHintTests
    : CompareHintTestBase<AsyncEnumerableCompareHint>
{
    private static readonly AsyncEnumerableCompareHint _TestInstance = new();

    private static readonly Type[] _ValidTypes =
    [
        typeof(IAsyncEnumerable<int>),
        typeof(IAsyncEnumerable<string>),
        typeof(IAsyncEnumerable<object>),
    ];

    private static readonly Type[] _InvalidTypes = [typeof(IEnumerable), typeof(DataHolderSample)];

    public AsyncEnumerableCompareHintTests()
        : base(_TestInstance, _ValidTypes, _InvalidTypes) { }

    [Theory, RandomData]
    internal void TryToCompare_BlocksComparison(IAsyncEnumerable<string> data)
    {
        TestInstance
            .Assert(x => x.TryToCompare(data, data, CreateChainer()))
            .Throws<EngineException>();
    }

    [Theory, RandomData]
    internal void TryToGetHashCode_BlocksHashing(IAsyncEnumerable<int> data)
    {
        TestInstance
            .Assert(x => x.TryToGetHashCode(data, CreateChainer()))
            .Throws<EngineException>();
    }

    [Theory, RandomData]
    internal async Task TryToCompare_NoDifferencesWhenEqual(IAsyncEnumerable<string> data)
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
        (
            await AsyncSeriesHelper.ToListAsync(
                result.Data,
                Tools.Valuer.Options.IterationLimit,
                TestContext.Current.CancellationToken
            )
        )
            .Assert()
            .IsEmpty();
    }

    [Theory, RandomData]
    internal async Task TryToCompare_FindsDifferences(
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
        (
            await AsyncSeriesHelper.ToListAsync(
                result.Data,
                Tools.Valuer.Options.IterationLimit,
                TestContext.Current.CancellationToken
            )
        )
            .Assert()
            .IsNotEmpty();
    }
}
