using System.Collections;
using CreateAndFake.Toolbox.ValuerTool;
using CreateAndFake.Toolbox.ValuerTool.CompareHints;
using CreateAndFakeTests.TestSamples;

namespace CreateAndFakeTests.Toolbox.ValuerTool.CompareHints;

public sealed class ValuerEquatableCompareHintTests : CompareHintTestBase<ValuerEquatableCompareHint>
{
    private static readonly ValuerEquatableCompareHint _TestInstance = new();

    private static readonly Type[] _ValidTypes =
    [
        typeof(IValuerEquatable),
        typeof(ValuerEquatableSample),
        typeof(PrivateValuerEquatableSample)
    ];

    private static readonly Type[] _InvalidTypes =
    [
        typeof(IDictionary),
        typeof(object)
    ];

    public ValuerEquatableCompareHintTests() : base(_TestInstance, _ValidTypes, _InvalidTypes) { }
}
