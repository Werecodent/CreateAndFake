using System.Collections.Frozen;
using CreateAndFake.DuplicatorTool.Hints;
using CreateAndFake.Samples.Scenarios;

namespace CreateAndFake.Tests.DuplicatorTool.Hints;

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
