using Werecodent.CreateAndFake.Samples.Scenarios;
using Werecodent.CreateAndFake.ValuerTool.Hints;

namespace Werecodent.CreateAndFake.Tests.ValuerTool.Hints;

public sealed class ValuerComparableCompareHintTests
    : CompareHintTestBase<ValuerComparableCompareHint>
{
    private static readonly Type[] _ValidTypes = [typeof(ValuerComparableSample)];

    public ValuerComparableCompareHintTests()
        : base(_ValidTypes) { }
}
