using CreateAndFake.DuplicatorTool.Hints;
using CreateAndFake.RandomizerTool.Hints;

namespace CreateAndFake.Tests.DuplicatorTool.Hints;

public sealed class ImmutableCollectionCopyHintTests : CopyHintTestBase<ImmutableCollectionCopyHint>
{
    private static readonly Type[] _ValidTypes =
    [
        .. ImmutableCollectionCreateHint.PotentialCollections,
    ];

    private static readonly Type[] _InvalidTypes = [typeof(object)];

    public ImmutableCollectionCopyHintTests()
        : base(_ValidTypes, _InvalidTypes) { }
}
