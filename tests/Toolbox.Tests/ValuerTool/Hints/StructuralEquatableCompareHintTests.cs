using Werecodent.CreateAndFake.ValuerTool.Hints;

namespace Werecodent.CreateAndFake.Tests.ValuerTool.Hints;

public sealed class StructuralEquatableCompareHintTests
    : CompareHintTestBase<StructuralEquatableCompareHint>
{
    private static readonly Type[] _ValidTypes = [];

    public StructuralEquatableCompareHintTests()
        : base(_ValidTypes) { }
}
