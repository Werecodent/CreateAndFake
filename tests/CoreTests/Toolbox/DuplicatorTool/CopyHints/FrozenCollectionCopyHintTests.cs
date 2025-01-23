using System.Collections.Frozen;
using CreateAndFake.Toolbox.DuplicatorTool.CopyHints;

namespace CreateAndFakeTests.Toolbox.DuplicatorTool.CopyHints;

public sealed class FrozenCollectionCopyHintTests : CopyHintTestBase<FrozenCollectionCopyHint>
{
    private static readonly Type[] _ValidTypes = [typeof(FrozenSet<int>), typeof(FrozenDictionary<string, int>)];

    private static readonly Type[] _InvalidTypes = [typeof(object)];

    public FrozenCollectionCopyHintTests() : base(_ValidTypes, _InvalidTypes) { }
}
