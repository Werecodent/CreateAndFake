using System.Collections;
using System.Collections.Concurrent;
using Werecodent.CreateAndFake.DuplicatorTool.Hints;
using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Tests.DuplicatorTool.Hints;

public sealed class CollectionCopyHintTests : CopyHintTestBase<CollectionCopyHint>
{
    private static readonly Type[] _ValidTypes =
    [
        typeof(List<>),
        typeof(Queue<>),
        typeof(Stack<>),
        typeof(HashSet<>),
        typeof(LinkedList<>),
        typeof(ConcurrentQueue<>),
        typeof(ConcurrentStack<>),
        typeof(ConcurrentDictionary<,>),
        typeof(Dictionary<,>),
        typeof(int[]),
        typeof(string[]),
        typeof(ArrayList),
        typeof(Queue),
        typeof(Stack),
        typeof(Array),
    ];

    private static readonly Type[] _InvalidTypes = [typeof(DataHolderSample)];

    public CollectionCopyHintTests()
        : base(_ValidTypes, _InvalidTypes) { }
}
