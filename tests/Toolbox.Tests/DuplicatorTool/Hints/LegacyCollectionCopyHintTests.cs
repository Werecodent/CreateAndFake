using System.Collections;
using CreateAndFake.DuplicatorTool.Hints;
using CreateAndFake.RandomizerTool.Hints;
using CreateAndFake.Samples.Scenarios;

namespace CreateAndFake.Tests.DuplicatorTool.Hints;

public sealed class LegacyCollectionCopyHintTests : CopyHintTestBase<LegacyCollectionCopyHint>
{
    private static readonly Type[] _ValidTypes =
    [
        .. LegacyCollectionCreateHint.PotentialCollections.Except([
            typeof(ArrayList),
            typeof(Queue),
            typeof(Stack),
            typeof(Array),
        ]),
    ];

    private static readonly Type[] _InvalidTypes = [typeof(DataHolderSample)];

    public LegacyCollectionCopyHintTests()
        : base(_ValidTypes, _InvalidTypes) { }
}
