using Werecodent.CreateAndFake.Design.Comparisons;
using Werecodent.CreateAndFake.DuplicatorTool.Hints;
using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Tests.DuplicatorTool.Hints;

public sealed class DeepCloneableCopyHintTests : CopyHintTestBase<DeepCloneableCopyHint>
{
    private class InnerCloneable(string data) : IDeepCloneable<InnerCloneable>
    {
        public InnerCloneable DeepClone()
        {
            return new InnerCloneable(data);
        }
    }

    private sealed class InheritedCloneable(string data) : InnerCloneable(data);

    private sealed class OuterCloneable(string data)
        : InnerCloneable(""),
            IDeepCloneable<OuterCloneable>
    {
        public new OuterCloneable DeepClone()
        {
            return new OuterCloneable(data);
        }
    }

    private static readonly Type[] _ValidTypes =
    [
        typeof(IDeepCloneable<>),
        typeof(InnerCloneable),
        typeof(OuterCloneable),
    ];

    private static readonly Type[] _InvalidTypes =
    [
        typeof(DataHolderSample),
        typeof(InheritedCloneable),
    ];

    public DeepCloneableCopyHintTests()
        : base(_ValidTypes, _InvalidTypes) { }
}
