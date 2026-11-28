using System.Collections;
using Werecodent.CreateAndFake.RandomizerTool.Hints;
using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Tests.RandomizerTool.Hints;

public sealed class LegacyCollectionCreateHintTests : CreateHintTestBase<LegacyCollectionCreateHint>
{
    private static readonly Type[] _ValidTypes =
    [
        .. LegacyCollectionCreateHint.PotentialCollections,
        typeof(IEnumerable),
        typeof(IList),
        typeof(IDictionary),
    ];

    private static readonly Type[] _InvalidTypes = [typeof(DataHolderSample)];

    public LegacyCollectionCreateHintTests()
        : base(_ValidTypes, _InvalidTypes) { }
}
