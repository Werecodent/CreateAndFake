using System.Collections;
using CreateAndFake.Toolbox.RandomizerTool.CreateHints;
using CreateAndFakeTests.TestBases;

namespace CreateAndFakeTests.Toolbox.RandomizerTool.CreateHints;

public sealed class AsyncCollectionCreateHintTests : CreateHintTestBase<AsyncCollectionCreateHint>
{
    private static readonly AsyncCollectionCreateHint _TestInstance = new();

    private static readonly Type[] _ValidTypes =
    [
        typeof(IAsyncEnumerable<int>),
        typeof(IAsyncEnumerable<string>),
        typeof(IAsyncEnumerable<object>)
    ];

    private static readonly Type[] _InvalidTypes =
    [
        typeof(object),
        typeof(IEnumerable)
    ];

    public AsyncCollectionCreateHintTests() : base(_TestInstance, _ValidTypes, _InvalidTypes) { }

    [Theory, RandomData]
    internal static async Task GetItems_Empty([Size(0)] IAsyncEnumerable<int> items)
    {
        IAsyncEnumerator<int> gen = items.GetAsyncEnumerator();
        await using (gen.ConfigureAwait(true))
        {
            (await gen.MoveNextAsync().ConfigureAwait(true)).Assert().Is(false);
        }
    }

    [Theory, RandomData]
    internal static async Task GetItems_Repeatable(IAsyncEnumerable<int> items)
    {
        List<int> first = [];
        await foreach (int item in items.ConfigureAwait(true))
        {
            first.Add(item);
        }

        List<int> second = [];
        await foreach (int item in items.ConfigureAwait(true))
        {
            second.Add(item);
        }

        first.Assert().Is(second);
    }

    [Theory, RandomData]
    internal static async Task GetItems_Cancel(IAsyncEnumerable<int> items)
    {
        await items.GetAsyncEnumerator().DisposeAsync().ConfigureAwait(true);
    }

    [Theory, RandomData]
    internal static async Task GetItems_Interrupt([Size(5)] IAsyncEnumerable<int> items)
    {
        await items.GetAsyncEnumerator().DisposeAsync().ConfigureAwait(true);

        int count = 0;
        await foreach (int item in items.ConfigureAwait(true))
        {
            count++;
            if (count == 3)
            {
                break;
            }
        }

        count = 0;
        await foreach (int item in items.ConfigureAwait(true))
        {
            count++;
        }
        count.Assert().Is(5);
    }
}
