using System.Collections;
using CreateAndFake.DuplicatorTool.Hints;
using CreateAndFake.RandomizerTool.Hints;
using CreateAndFake.Samples.Scenarios;

namespace CreateAndFake.Tests.DuplicatorTool.Hints;

public sealed class CollectionCopyHintTests : CopyHintTestBase<CollectionCopyHint>
{
    private static readonly Type[] _ValidTypes =
    [
        .. CollectionCreateHint.PotentialCollections,
        typeof(int[]),
        typeof(string[]),
        typeof(ArrayList),
        typeof(Queue),
        typeof(Stack),
        typeof(Array),
    ];

    private static readonly Type[] _InvalidTypes = [typeof(DataHolderSample)];

    public CollectionCopyHintTests()
        : base(_ValidTypes, _InvalidTypes) { }
}
