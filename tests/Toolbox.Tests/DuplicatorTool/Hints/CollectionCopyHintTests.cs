using System.Collections;
using System.Collections.Concurrent;
using CreateAndFake.DuplicatorTool.Hints;
using CreateAndFake.Samples.Scenarios;

namespace CreateAndFake.Tests.DuplicatorTool.Hints;

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
