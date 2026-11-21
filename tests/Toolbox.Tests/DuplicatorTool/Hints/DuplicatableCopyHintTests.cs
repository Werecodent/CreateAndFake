using Werecodent.CreateAndFake.DuplicatorTool;
using Werecodent.CreateAndFake.DuplicatorTool.Hints;
using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Tests.DuplicatorTool.Hints;

public sealed class DuplicatableCopyHintTests : CopyHintTestBase<DuplicatableCopyHint>
{
    private class InnerCloneable(string data) : IDuplicatable<InnerCloneable>
    {
        public InnerCloneable DeepClone(IDuplicator duplicator)
        {
            return new InnerCloneable(data);
        }
    }

    private sealed class InheritedCloneable(string data) : InnerCloneable(data);

    private sealed class OuterCloneable(string data)
        : InnerCloneable(""),
            IDuplicatable<OuterCloneable>
    {
        public new OuterCloneable DeepClone(IDuplicator duplicator)
        {
            return new OuterCloneable(data);
        }
    }

    private static readonly Type[] _ValidTypes =
    [
        typeof(IDuplicatable<>),
        typeof(InnerCloneable),
        typeof(OuterCloneable),
    ];

    private static readonly Type[] _InvalidTypes =
    [
        typeof(DataHolderSample),
        typeof(InheritedCloneable),
    ];

    public DuplicatableCopyHintTests()
        : base(_ValidTypes, _InvalidTypes) { }
}
