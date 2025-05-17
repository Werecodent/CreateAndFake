using System.Collections;
using CreateAndFake.Tests.TestSamples;
using CreateAndFake.ValuerTool;
using CreateAndFake.ValuerTool.Hints;

namespace CreateAndFake.Tests.ValuerTool.Hints;

public sealed class ValuerEquatableCompareHintTests
    : CompareHintTestBase<ValuerEquatableCompareHint>
{
    private static readonly ValuerEquatableCompareHint _TestInstance = new();

    private static readonly Type[] _ValidTypes =
    [
        typeof(IValuerEquatable),
        typeof(ValuerEquatableSample),
        typeof(PrivateValuerEquatableSample),
    ];

    private static readonly Type[] _InvalidTypes = [typeof(IDictionary), typeof(object)];

    public ValuerEquatableCompareHintTests()
        : base(_TestInstance, _ValidTypes, _InvalidTypes) { }
}
