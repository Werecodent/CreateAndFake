using System.Collections;
using CreateAndFake.Toolbox.ValuerTool.CompareHints;

namespace CreateAndFakeTests.Toolbox.ValuerTool.CompareHints;

public sealed class AsyncEnumerableCompareHintTests : CompareHintTestBase<AsyncEnumerableCompareHint>
{
    private static readonly AsyncEnumerableCompareHint _TestInstance = new();

    private static readonly Type[] _ValidTypes =
    [
        typeof(IAsyncEnumerable<int>),
        typeof(IAsyncEnumerable<string>),
        typeof(IAsyncEnumerable<object>)
    ];

    private static readonly Type[] _InvalidTypes =
    [
        typeof(IEnumerable),
        typeof(object)
    ];

    public AsyncEnumerableCompareHintTests() : base(_TestInstance, _ValidTypes, _InvalidTypes) { }

    [Theory, RandomData]
    internal void Supports_NullTest(IAsyncEnumerable<string> data)
    {
        TestInstance.TryCompare(null, data, CreateChainer()).HasData.Assert().Is(false);
        TestInstance.TryCompare(data, null, CreateChainer()).HasData.Assert().Is(false);
    }
}
