using CreateAndFake.Design.Content;
using CreateAndFake.DuplicatorTool.Hints;
using CreateAndFake.Samples.Scenarios;

namespace CreateAndFake.Tests.DuplicatorTool.Hints;

public sealed class DeepCloneableCopyHintTests : CopyHintTestBase<DeepCloneableCopyHint>
{
    private static readonly Type[] _ValidTypes = [.. Enumerable.Repeat(typeof(IDeepCloneable), 10)];

    private static readonly Type[] _InvalidTypes = [typeof(DataHolderSample)];

    public DeepCloneableCopyHintTests()
        : base(_ValidTypes, _InvalidTypes) { }
}
