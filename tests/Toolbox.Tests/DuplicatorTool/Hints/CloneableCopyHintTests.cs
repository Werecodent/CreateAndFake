using Werecodent.CreateAndFake.DuplicatorTool.Hints;

namespace Werecodent.CreateAndFake.Tests.DuplicatorTool.Hints;

public sealed class CloneableCopyHintTests : CopyHintTestBase<CloneableCopyHint>
{
    private static readonly Type[] _ValidTypes = [typeof(string)];

    public CloneableCopyHintTests()
        : base(_ValidTypes) { }
}
