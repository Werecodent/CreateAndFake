using CreateAndFake.DuplicatorTool.Hints;

namespace CreateAndFake.Tests.DuplicatorTool.Hints;

public sealed class CloneableCopyHintTests : CopyHintTestBase<CloneableCopyHint>
{
    private static readonly Type[] _ValidTypes = [typeof(string)];

    private static readonly Type[] _InvalidTypes = [typeof(object)];

    public CloneableCopyHintTests()
        : base(_ValidTypes, _InvalidTypes) { }
}
