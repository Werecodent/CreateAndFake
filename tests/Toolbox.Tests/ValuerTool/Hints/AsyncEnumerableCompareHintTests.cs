using System.Collections;
using CreateAndFake.Design.Exceptions;
using CreateAndFake.Samples.Scenarios;
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
}
