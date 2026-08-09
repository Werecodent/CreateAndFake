using Werecodent.CreateAndFake.DuplicatorTool;
using Werecodent.CreateAndFake.DuplicatorTool.Hints;
using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Tests.DuplicatorTool.Hints;

public sealed class DuplicatableCopyHintTests : CopyHintTestBase<DuplicatableCopyHint>
{
    private static readonly Type[] _ValidTypes = [typeof(IDuplicatable<>)];

    private static readonly Type[] _InvalidTypes = [typeof(DataHolderSample)];

    public DuplicatableCopyHintTests()
        : base(_ValidTypes, _InvalidTypes) { }
}
