using System.Collections;
using System.Collections.Concurrent;
using Werecodent.CreateAndFake.DuplicatorTool.Hints;

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

    public CollectionCopyHintTests()
        : base(_ValidTypes) { }
}
