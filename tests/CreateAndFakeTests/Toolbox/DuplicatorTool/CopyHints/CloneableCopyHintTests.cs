using CreateAndFake.Toolbox.DuplicatorTool.CopyHints;
using CreateAndFakeTests.TestBases;

namespace CreateAndFakeTests.Toolbox.DuplicatorTool.CopyHints;

public sealed class CloneableCopyHintTests : CopyHintTestBase<CloneableCopyHint>
{
    private static readonly Type[] _ValidTypes = [.. Enumerable.Repeat(typeof(ICloneable), 10)];

    private static readonly Type[] _InvalidTypes = [typeof(object)];

    public CloneableCopyHintTests() : base(_ValidTypes, _InvalidTypes) { }
}
