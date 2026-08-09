using Werecodent.CreateAndFake.DuplicatorTool.Hints;
using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Tests.DuplicatorTool.Hints;

public sealed class CloneableCopyHintTests : CopyHintTestBase<CloneableCopyHint>
{
    private static readonly Type[] _ValidTypes = [typeof(string)];

    private static readonly Type[] _InvalidTypes = [typeof(DataHolderSample)];

    public CloneableCopyHintTests()
        : base(_ValidTypes, _InvalidTypes) { }
}
