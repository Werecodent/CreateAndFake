using System.Collections;
using CreateAndFake.RandomizerTool.Hints;
using CreateAndFake.Samples.Scenarios;

namespace CreateAndFake.Tests.RandomizerTool.Hints;

public sealed class CollectionCreateHintTests : CreateHintTestBase<CollectionCreateHint>
{
    private static readonly CollectionCreateHint _TestInstance = new();

    private static readonly Type[] _ItemTypes =
    [
        typeof(string),
        typeof(object),
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
            ])
            .Select(MakeDefined),
    ];

    private static readonly Type[] _InvalidTypes =
    [
        typeof(DataHolderSample),
        typeof(IEnumerable),
        typeof(IEnumerable<>),
    ];

    public CollectionCreateHintTests()
        : base(_TestInstance, _ValidTypes, _InvalidTypes) { }

    [Fact]
    public void TryCreate_RetriesSetsWithDuplicates()
    {
        for (int i = 0; i < 20; i++)
        {
            _TestInstance.TryCreate(typeof(IDictionary<bool, int>), CreateChainer());
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
