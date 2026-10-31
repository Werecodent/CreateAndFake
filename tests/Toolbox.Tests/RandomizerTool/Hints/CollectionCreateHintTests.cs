using System.Collections.Frozen;
using System.Collections.Immutable;
using Werecodent.CreateAndFake.RandomizerTool.Hints;

namespace Werecodent.CreateAndFake.Tests.RandomizerTool.Hints;

public sealed class CollectionCreateHintTests : CreateHintTestBase<CollectionCreateHint>
{
    private static readonly Type[] _ItemTypes =
    [
        typeof(string),
        typeof(IComparable),
        typeof(int),
        typeof(double),
        typeof(KeyValuePair<string, int>),
    ];

    private static readonly Type[] _ValidTypes =
    [
        .. CollectionCreateHint
            .PotentialCollections.Concat([
                typeof(IEnumerable<>),
                typeof(IList<>),
                typeof(ISet<>),
                typeof(IDictionary<,>),
                typeof(IReadOnlyCollection<>),
                typeof(IReadOnlyList<>),
                typeof(IReadOnlyDictionary<,>),
                typeof(int[]),
                typeof(string[]),
                typeof(object[]),
                typeof(IImmutableList<>),
                typeof(IImmutableQueue<>),
                typeof(IImmutableStack<>),
                typeof(IImmutableDictionary<,>),
                typeof(FrozenSet<>),
                typeof(FrozenDictionary<,>),
            ])
            .Select(MakeDefined),
    ];

    public CollectionCreateHintTests()
        : base(_ValidTypes) { }

    [Fact]
    public void TryToCreate_RetriesSetsWithDuplicates()
    {
        for (int i = 0; i < 20; i++)
        {
            TestInstance.TryToCreate(typeof(IDictionary<bool, int>), CreateChainer());
        }
    }

    [Fact]
    public void TryToCreate_CorrectSizeEvenWithDuplicates()
    {
        for (int i = 0; i < 20; i++)
        {
            Tools.Randomizer.CreateSized<HashSet<bool>>(2).Assert().HasCount(2);
        }
    }

    private static Type MakeDefined(Type type)
    {
        if (type.IsGenericTypeDefinition)
        {
            return type.MakeGenericType([
                .. type.GetGenericArguments().Select(_ => Tools.Gen.NextItem(_ItemTypes)),
            ]);
        }
        else
        {
            return type;
        }
    }
}
