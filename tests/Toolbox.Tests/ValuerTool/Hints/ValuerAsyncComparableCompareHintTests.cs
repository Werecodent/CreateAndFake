using System.Collections;
using CreateAndFake.Design.Exceptions;
using CreateAndFake.Samples.Scenarios;
using CreateAndFake.ValuerTool;
using CreateAndFake.ValuerTool.Hints;

namespace CreateAndFake.Tests.ValuerTool.Hints;

public sealed class ValuerAsyncComparableCompareHintTests
    : CompareHintTestBase<ValuerAsyncComparableCompareHint>
{
    private static readonly ValuerAsyncComparableCompareHint _TestInstance = new();

    private static readonly Type[] _ValidTypes =
    [ //typeof(IValuerAsyncComparable)
    ];

    private static readonly Type[] _InvalidTypes = [typeof(IDictionary), typeof(DataHolderSample)];

    public ValuerAsyncComparableCompareHintTests()
        : base(_TestInstance, _ValidTypes, _InvalidTypes) { }

    [Theory, RandomData]
    internal void TryToCompare_BlocksComparison(IValuerAsyncComparable data)
    {
        TestInstance
            .Assert(x => x.TryToCompare(data, data, CreateChainer()))
            .Throws<EngineException>();
    }

    [Theory, RandomData]
    internal void TryToGetHashCode_BlocksHashing(IValuerAsyncComparable data)
    {
        TestInstance
            .Assert(x => x.TryToGetHashCode(data, CreateChainer()))
            .Throws<EngineException>();
    }
}
