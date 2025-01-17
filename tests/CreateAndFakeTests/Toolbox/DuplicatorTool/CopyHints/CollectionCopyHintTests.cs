using System.Collections;
using CreateAndFake.Toolbox.DuplicatorTool.CopyHints;
using CreateAndFake.Toolbox.RandomizerTool.CreateHints;
using CreateAndFakeTests.TestBases;

namespace CreateAndFakeTests.Toolbox.DuplicatorTool.CopyHints;

public sealed class CollectionCopyHintTests : CopyHintTestBase<CollectionCopyHint>
{
    private static readonly Type[] _ValidTypes = [
        .. CollectionCreateHint.PotentialCollections,
        typeof(int[]),
        typeof(string[]),
        typeof(ArrayList),
        typeof(Queue),
        typeof(Stack),
        typeof(Array)];

    private static readonly Type[] _InvalidTypes = [typeof(object)];

    public CollectionCopyHintTests() : base(_ValidTypes, _InvalidTypes) { }
}
