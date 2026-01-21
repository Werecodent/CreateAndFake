using System.Collections;
using CreateAndFake.Design.Content;
using CreateAndFake.ValuerTool.Engine;
using CreateAndFake.ValuerTool.Hints;

namespace CreateAndFake.Tests.ValuerTool.Hints;

public sealed class SyncAsyncEnumerableCompareHintTests
    : CompareHintTestBase<SyncAsyncEnumerableCompareHint>
{
    private static readonly SyncAsyncEnumerableCompareHint _TestInstance = new();

    private static readonly Type[] _ValidTypes = [];

    private static readonly Type[] _InvalidTypes = [typeof(IEnumerable), typeof(object)];

    public SyncAsyncEnumerableCompareHintTests()
        : base(_TestInstance, _ValidTypes, _InvalidTypes) { }

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
            CreateChainer()
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
            CreateChainer()
        );

        result.HasData.Assert().Is(true);
        (await AsyncEnumHelper.ToListAsync(result.Data, TestContext.Current.CancellationToken))
            .Assert()
            .IsNotEmpty();
    }
}
