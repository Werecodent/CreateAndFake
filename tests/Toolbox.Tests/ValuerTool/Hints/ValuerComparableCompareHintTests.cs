using System.Collections;
using Werecodent.CreateAndFake.Samples.Scenarios;
using Werecodent.CreateAndFake.ValuerTool.Hints;

namespace Werecodent.CreateAndFake.Tests.ValuerTool.Hints;

public sealed class ValuerComparableCompareHintTests
    : CompareHintTestBase<ValuerComparableCompareHint>
{
    private static readonly ValuerComparableCompareHint _TestInstance = new();

    private static readonly Type[] _ValidTypes = [typeof(ValuerComparableSample)];

    private static readonly Type[] _InvalidTypes = [typeof(IDictionary), typeof(DataHolderSample)];

    public ValuerComparableCompareHintTests()
        : base(_TestInstance, _ValidTypes, _InvalidTypes) { }
}
