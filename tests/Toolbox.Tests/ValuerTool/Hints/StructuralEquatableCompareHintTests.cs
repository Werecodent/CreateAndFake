using Werecodent.CreateAndFake.ValuerTool;
using Werecodent.CreateAndFake.ValuerTool.Engine;
using Werecodent.CreateAndFake.ValuerTool.Hints;

namespace Werecodent.CreateAndFake.Tests.ValuerTool.Hints;

public sealed class StructuralEquatableCompareHintTests
    : CompareHintTestBase<StructuralEquatableCompareHint>
{
    private static readonly Type[] _ValidTypes = [typeof(ValueTuple<int>)];

    public StructuralEquatableCompareHintTests()
        : base(_ValidTypes) { }

    protected override IValuerChainer CreateChainer(ValuerOptions options = null)
    {
        return new ValuerChainer(
            (options ?? Tools.Valuer.Options) with
            {
                UseEquatableComparisons = true,
            },
            new ValuerEngine()
        );
    }
}
