using System.Collections;
using Werecodent.CreateAndFake.Samples.Scenarios;
using Werecodent.CreateAndFake.ValuerTool;
using Werecodent.CreateAndFake.ValuerTool.Hints;

namespace Werecodent.CreateAndFake.Tests.ValuerTool.Hints;

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

    private static readonly Type[] _InvalidTypes = [typeof(IDictionary), typeof(DataHolderSample)];

    public ValuerEquatableCompareHintTests()
        : base(_TestInstance, _ValidTypes, _InvalidTypes) { }
}
