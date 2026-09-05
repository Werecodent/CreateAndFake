using System.Collections.Immutable;
using Werecodent.CreateAndFake.DuplicatorTool.Hints;
using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Tests.DuplicatorTool.Hints;

public sealed class ImmutableCollectionCopyHintTests : CopyHintTestBase<ImmutableCollectionCopyHint>
{
    private static readonly Type[] _ValidTypes =
    [
        typeof(ImmutableList<>),
        typeof(ImmutableArray<>),
        typeof(ImmutableQueue<>),
        typeof(ImmutableStack<>),
        typeof(ImmutableHashSet<>),
        typeof(ImmutableDictionary<,>),
    ];

    private static readonly Type[] _InvalidTypes = [typeof(DataHolderSample)];

    public ImmutableCollectionCopyHintTests()
        : base(_ValidTypes, _InvalidTypes) { }
}
