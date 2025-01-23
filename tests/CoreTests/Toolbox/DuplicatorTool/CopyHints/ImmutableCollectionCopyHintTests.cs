using CreateAndFake.Toolbox.DuplicatorTool.CopyHints;
using CreateAndFake.Toolbox.RandomizerTool.CreateHints;

namespace CreateAndFakeTests.Toolbox.DuplicatorTool.CopyHints;

public sealed class ImmutableCollectionCopyHintTests : CopyHintTestBase<ImmutableCollectionCopyHint>
{
    private static readonly Type[] _ValidTypes = [.. ImmutableCollectionCreateHint.PotentialCollections];

    private static readonly Type[] _InvalidTypes = [typeof(object)];

    public ImmutableCollectionCopyHintTests() : base(_ValidTypes, _InvalidTypes) { }
}
