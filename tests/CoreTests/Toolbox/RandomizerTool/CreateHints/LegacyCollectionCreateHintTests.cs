using System.Collections;
using CreateAndFake.Toolbox.RandomizerTool.CreateHints;

namespace CreateAndFakeTests.Toolbox.RandomizerTool.CreateHints;

public sealed class LegacyCollectionCreateHintTests : CreateHintTestBase<LegacyCollectionCreateHint>
{
    private static readonly LegacyCollectionCreateHint _TestInstance = new();

    private static readonly Type[] _ValidTypes = [
        .. LegacyCollectionCreateHint.PotentialCollections,
        typeof(IEnumerable),
        typeof(IList),
        typeof(IDictionary)];

    private static readonly Type[] _InvalidTypes = [typeof(object)];

    public LegacyCollectionCreateHintTests() : base(_TestInstance, _ValidTypes, _InvalidTypes) { }
}
