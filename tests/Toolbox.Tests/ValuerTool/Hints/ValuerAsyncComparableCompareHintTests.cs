using System.Collections;
using CreateAndFake.Design.Tooling;
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

    private static readonly Type[] _InvalidTypes = [typeof(IDictionary), typeof(object)];

    public ValuerAsyncComparableCompareHintTests()
        : base(_TestInstance, _ValidTypes, _InvalidTypes) { }

    [Theory, RandomData]
    internal void TryCompare_BlocksComparison(IValuerAsyncComparable data)
    {
        TestInstance
            .Assert(hint => hint.TryCompare(data, data, CreateChainer()))
            .Throws<ToolException>();
    }

    [Theory, RandomData]
    internal void TryGetHashCode_BlocksHashing(IValuerAsyncComparable data)
    {
        TestInstance
            .Assert(hint => hint.TryGetHashCode(data, CreateChainer()))
            .Throws<ToolException>();
    }
}
