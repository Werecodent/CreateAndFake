using Werecodent.CreateAndFake.Design.Comparisons;
using Werecodent.CreateAndFake.DuplicatorTool.Hints;
using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Tests.DuplicatorTool.Hints;

public sealed class DeepCloneableCopyHintTests : CopyHintTestBase<DeepCloneableCopyHint>
{
    private static readonly Type[] _ValidTypes =
    [
        .. Enumerable.Repeat(typeof(IDeepCloneable<>), 10),
    ];

    private static readonly Type[] _InvalidTypes = [typeof(DataHolderSample)];

    public DeepCloneableCopyHintTests()
        : base(_ValidTypes, _InvalidTypes) { }
}
