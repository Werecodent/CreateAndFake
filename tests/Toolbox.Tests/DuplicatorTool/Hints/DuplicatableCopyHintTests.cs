using CreateAndFake.DuplicatorTool;
using CreateAndFake.DuplicatorTool.Hints;
using CreateAndFake.Samples.Scenarios;

namespace CreateAndFake.Tests.DuplicatorTool.Hints;

public sealed class DuplicatableCopyHintTests : CopyHintTestBase<DuplicatableCopyHint>
{
    private static readonly Type[] _ValidTypes = [typeof(IDuplicatable)];

    private static readonly Type[] _InvalidTypes = [typeof(DataHolderSample)];

    public DuplicatableCopyHintTests()
        : base(_ValidTypes, _InvalidTypes) { }
}
