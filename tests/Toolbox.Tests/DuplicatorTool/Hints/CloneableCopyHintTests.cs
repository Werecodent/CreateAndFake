using CreateAndFake.DuplicatorTool.Hints;
using CreateAndFake.Samples.Scenarios;

namespace CreateAndFake.Tests.DuplicatorTool.Hints;

public sealed class CloneableCopyHintTests : CopyHintTestBase<CloneableCopyHint>
{
    private static readonly Type[] _ValidTypes = [typeof(string)];

    private static readonly Type[] _InvalidTypes = [typeof(DataHolderSample)];

    public CloneableCopyHintTests()
        : base(_ValidTypes, _InvalidTypes) { }
}
