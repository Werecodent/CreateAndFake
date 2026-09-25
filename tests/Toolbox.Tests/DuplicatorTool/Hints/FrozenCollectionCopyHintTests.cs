using System.Collections.Frozen;
using Werecodent.CreateAndFake.DuplicatorTool.Hints;
using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Tests.DuplicatorTool.Hints;

public sealed class FrozenCollectionCopyHintTests : CopyHintTestBase<FrozenCollectionCopyHint>
{
    private static readonly Type[] _ValidTypes =
    [
        typeof(FrozenSet<int>),
        typeof(FrozenDictionary<string, int>),
    ];

    private static readonly Type[] _InvalidTypes = [typeof(DataHolderSample)];

    public FrozenCollectionCopyHintTests()
        : base(_ValidTypes, _InvalidTypes) { }
}
