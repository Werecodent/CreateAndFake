using System.Collections;
using CreateAndFake.Design.Content;
using CreateAndFake.Samples.OldSamples;
using CreateAndFake.ValuerTool.Hints;

namespace CreateAndFake.Tests.ValuerTool.Hints;

public sealed class ValueEquatableCompareHintTests : CompareHintTestBase<ValueEquatableCompareHint>
{
    private static readonly ValueEquatableCompareHint _TestInstance = new();

    private static readonly Type[] _ValidTypes =
    [
        typeof(IValueEquatable),
        typeof(ValueEquatableSample),
    ];

    private static readonly Type[] _InvalidTypes = [typeof(IDictionary), typeof(object)];

    public ValueEquatableCompareHintTests()
        : base(_TestInstance, _ValidTypes, _InvalidTypes) { }
}
