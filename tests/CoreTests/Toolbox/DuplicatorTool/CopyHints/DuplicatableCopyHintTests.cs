using CreateAndFake.Toolbox.DuplicatorTool;
using CreateAndFake.Toolbox.DuplicatorTool.CopyHints;

namespace CreateAndFakeTests.Toolbox.DuplicatorTool.CopyHints;

public sealed class DuplicatableCopyHintTests : CopyHintTestBase<DuplicatableCopyHint>
{
    private static readonly Type[] _ValidTypes = [typeof(IDuplicatable)];

    private static readonly Type[] _InvalidTypes = [typeof(object)];

    public DuplicatableCopyHintTests() : base(_ValidTypes, _InvalidTypes) { }
}
