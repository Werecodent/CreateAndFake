using Werecodent.CreateAndFake.Design.Comparisons;
using Werecodent.CreateAndFake.Samples.Scenarios;
using Werecodent.CreateAndFake.ValuerTool.Hints;

namespace Werecodent.CreateAndFake.Tests.ValuerTool.Hints;

public sealed class ValueEquatableCompareHintTests : CompareHintTestBase<ValueEquatableCompareHint>
{
    private static readonly Type[] _ValidTypes =
    [
        typeof(IValueEquatable),
        typeof(ValueEquatableSample),
    ];

    public ValueEquatableCompareHintTests()
        : base(_ValidTypes) { }
}
