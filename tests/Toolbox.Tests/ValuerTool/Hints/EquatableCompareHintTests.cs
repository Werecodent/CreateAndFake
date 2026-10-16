using Werecodent.CreateAndFake.Design.Reiteration;
using Werecodent.CreateAndFake.ValuerTool.Hints;

namespace Werecodent.CreateAndFake.Tests.ValuerTool.Hints;

public sealed class EquatableCompareHintTests : CompareHintTestBase<EquatableCompareHint>
{
    private static readonly Type[] _ValidTypes = [typeof(Limiter)];

    public EquatableCompareHintTests()
        : base(_ValidTypes) { }
}
