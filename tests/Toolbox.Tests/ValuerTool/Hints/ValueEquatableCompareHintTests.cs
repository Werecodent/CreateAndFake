using System.Collections;
using Werecodent.CreateAndFake.Design.Comparisons;
using Werecodent.CreateAndFake.Samples.Scenarios;
using Werecodent.CreateAndFake.ValuerTool.Hints;

namespace Werecodent.CreateAndFake.Tests.ValuerTool.Hints;

public sealed class ValueEquatableCompareHintTests : CompareHintTestBase<ValueEquatableCompareHint>
{
    private static readonly ValueEquatableCompareHint _TestInstance = new();

    private static readonly Type[] _ValidTypes =
    [
        typeof(IValueEquatable),
        typeof(ValueEquatableSample),
    ];

    private static readonly Type[] _InvalidTypes = [typeof(IDictionary), typeof(DataHolderSample)];

    public ValueEquatableCompareHintTests()
        : base(_TestInstance, _ValidTypes, _InvalidTypes) { }
}
