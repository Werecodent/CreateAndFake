using Werecodent.CreateAndFake.Samples.Scenarios;
using Werecodent.CreateAndFake.ValuerTool;
using Werecodent.CreateAndFake.ValuerTool.Hints;

namespace Werecodent.CreateAndFake.Tests.ValuerTool.Hints;

public sealed class ValuerEquatableCompareHintTests
    : CompareHintTestBase<ValuerEquatableCompareHint>
{
    private static readonly Type[] _ValidTypes =
    [
        typeof(IValuerEquatable),
        typeof(ValuerEquatableSample),
        typeof(PrivateValuerEquatableSample),
    ];

    public ValuerEquatableCompareHintTests()
        : base(_ValidTypes) { }
}
