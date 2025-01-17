using CreateAndFake.Design.Content;
using CreateAndFake.Toolbox.DuplicatorTool.CopyHints;
using CreateAndFakeTests.TestBases;

namespace CreateAndFakeTests.Toolbox.DuplicatorTool.CopyHints;

public sealed class DeepCloneableCopyHintTests : CopyHintTestBase<DeepCloneableCopyHint>
{
    private static readonly Type[] _ValidTypes = [.. Enumerable.Repeat(typeof(IDeepCloneable), 10)];

    private static readonly Type[] _InvalidTypes = [typeof(object)];

    public DeepCloneableCopyHintTests() : base(_ValidTypes, _InvalidTypes) { }
}
