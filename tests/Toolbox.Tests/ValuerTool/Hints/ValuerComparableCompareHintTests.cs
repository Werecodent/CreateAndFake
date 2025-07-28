using System.Collections;
using CreateAndFake.ValuerTool;
using CreateAndFake.ValuerTool.Hints;

namespace CreateAndFake.Tests.ValuerTool.Hints;

public sealed class ValuerComparableCompareHintTests
    : CompareHintTestBase<ValuerComparableCompareHint>
{
    private static readonly ValuerComparableCompareHint _TestInstance = new();

    private static readonly Type[] _ValidTypes = [typeof(IValuerComparable)];

    private static readonly Type[] _InvalidTypes = [typeof(IDictionary), typeof(object)];

    public ValuerComparableCompareHintTests()
        : base(_TestInstance, _ValidTypes, _InvalidTypes) { }
}
