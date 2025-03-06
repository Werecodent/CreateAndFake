using CreateAndFake.DuplicatorTool;
using CreateAndFake.DuplicatorTool.CopyHints;

namespace CreateAndFake.Tests.DuplicatorTool.CopyHints;

public sealed class DuplicatableCopyHintTests : CopyHintTestBase<DuplicatableCopyHint>
{
    private static readonly Type[] _ValidTypes = [typeof(IDuplicatable)];

    private static readonly Type[] _InvalidTypes = [typeof(object)];

    public DuplicatableCopyHintTests()
        : base(_ValidTypes, _InvalidTypes) { }
}
