using System.Collections;
using CreateAndFake.Design.Content;
using CreateAndFake.Design.Exceptions;
using CreateAndFake.Samples.Scenarios;
using CreateAndFake.ValuerTool.Engine;
using CreateAndFake.ValuerTool.Hints;

namespace CreateAndFake.Tests.ValuerTool.Hints;

public sealed class AsyncEnumerableCompareHintTests
    : CompareHintTestBase<AsyncEnumerableCompareHint>
{
    private static readonly AsyncEnumerableCompareHint _TestInstance = new();

    private static readonly Type[] _ValidTypes =
    [
        // typeof(IAsyncEnumerable<int>),
        // typeof(IAsyncEnumerable<string>),
        // typeof(IAsyncEnumerable<object>),
    ];

    private static readonly Type[] _InvalidTypes = [typeof(IEnumerable), typeof(DataHolderSample)];

    public AsyncEnumerableCompareHintTests()
        : base(_TestInstance, _ValidTypes, _InvalidTypes) { }

    [Theory, RandomData]
    internal void TryCompare_BlocksComparison(IAsyncEnumerable<string> data)
    {
        TestInstance
            .Assert(hint => hint.TryCompare(data, data, CreateChainer()))
            .Throws<EngineException>();
    }

    [Theory, RandomData]
    internal void TryGetHashCode_BlocksHashing(IAsyncEnumerable<int> data)
    {
        TestInstance
            .Assert(hint => hint.TryGetHashCode(data, CreateChainer()))
            .Throws<EngineException>();
    }

    [Theory, RandomData]
    internal async Task TryCompare_NoDifferencesWhenEqual(IAsyncEnumerable<string> data)
    {
        IEnumerable<string> data2 = await AsyncEnumHelper.ToListAsync(
            data,
            TestContext.Current.CancellationToken
        );

        DifferenceHintAsyncResult result = TestInstance.TryAsyncCompare(
            data2,
            data,
            CreateChainer(),
            TestContext.Current.CancellationToken
        );

        result.HasData.Assert().Is(true);
        (await AsyncEnumHelper.ToListAsync(result.Data, TestContext.Current.CancellationToken))
            .Assert()
            .IsEmpty();
    }

    [Theory, RandomData]
    internal async Task TryCompare_FindsDifferences(
        IAsyncEnumerable<string> data,
        IEnumerable<string> data2
    )
    {
        DifferenceHintAsyncResult result = TestInstance.TryAsyncCompare(
            data,
            data2,
            CreateChainer(),
            TestContext.Current.CancellationToken
        );

        result.HasData.Assert().Is(true);
        (await AsyncEnumHelper.ToListAsync(result.Data, TestContext.Current.CancellationToken))
            .Assert()
            .IsNotEmpty();
    }
}
