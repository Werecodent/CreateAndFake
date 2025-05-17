using System.Collections;
using CreateAndFake.RandomizerTool.Hints;

namespace CreateAndFake.Tests.RandomizerTool.Hints;

public sealed class LegacyCollectionCreateHintTests : CreateHintTestBase<LegacyCollectionCreateHint>
{
    private static readonly LegacyCollectionCreateHint _TestInstance = new();

    private static readonly Type[] _ValidTypes =
    [
        .. LegacyCollectionCreateHint.PotentialCollections,
        typeof(IEnumerable),
        typeof(IList),
        typeof(IDictionary),
    ];

    private static readonly Type[] _InvalidTypes = [typeof(object)];

    public LegacyCollectionCreateHintTests()
        : base(_TestInstance, _ValidTypes, _InvalidTypes) { }
}
