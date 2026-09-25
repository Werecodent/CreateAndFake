using System.Collections;
using System.Collections.Frozen;
using System.Collections.Immutable;
using Werecodent.CreateAndFake.RandomizerTool.Hints;
using Werecodent.CreateAndFake.Samples.Scenarios;

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
                typeof(IAsyncEnumerable<>),
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
        : base(_ValidTypes, _InvalidTypes) { }

    [Fact]
    public void TryToCreate_RetriesSetsWithDuplicates()
    {
        for (int i = 0; i < 20; i++)
        {
            TestInstance.TryToCreate(typeof(IDictionary<bool, int>), CreateChainer());
        }
    }

    [Theory, RandomData]
    internal static async Task TryToCreate_Empty([Size(0)] IAsyncEnumerable<int> items)
    {
        IAsyncEnumerator<int> gen = items.GetAsyncEnumerator(TestContext.Current.CancellationToken);
        await using (gen)
        {
            (await gen.MoveNextAsync()).Assert().Is(false);
        }
    }

    [Theory, RandomData]
    internal static async Task TryToCreate_Repeatable(IAsyncEnumerable<int> items)
    {
        List<int> first = [];
        await foreach (int item in items.WithCancellation(TestContext.Current.CancellationToken))
        {
            first.Add(item);
        }

        List<int> second = [];
        await foreach (int item in items.WithCancellation(TestContext.Current.CancellationToken))
        {
            second.Add(item);
        }

        first.Assert().Is(second);
    }

    [Theory, RandomData]
    internal static async Task TryToCreate_Cancel(IAsyncEnumerable<int> items)
    {
        await items.GetAsyncEnumerator(TestContext.Current.CancellationToken).DisposeAsync();
    }

    [Theory, RandomData]
    internal static async Task TryToCreate_Interrupt([Size(5)] IAsyncEnumerable<int> items)
    {
        await items.GetAsyncEnumerator(TestContext.Current.CancellationToken).DisposeAsync();

        int count = 0;
        await foreach (int item in items.WithCancellation(TestContext.Current.CancellationToken))
        {
            count++;
            if (count == 3)
            {
                break;
            }
        }

        count = 0;
        await foreach (int item in items.WithCancellation(TestContext.Current.CancellationToken))
        {
            count++;
        }
        count.Assert().Is(5);
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
