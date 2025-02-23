using System.Collections;
using CreateAndFake.RandomizerTool.CreateHints;

namespace CreateAndFake.Tests.RandomizerTool.CreateHints;

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
        IAsyncEnumerator<int> gen = items.GetAsyncEnumerator(TestContext.Current.CancellationToken);
        await using (gen)
        {
            (await gen.MoveNextAsync()).Assert().Is(false);
        }
    }

    [Theory, RandomData]
    internal static async Task GetItems_Repeatable(IAsyncEnumerable<int> items)
    {
        List<int> first = [];
        await foreach (int item in items)
        {
            first.Add(item);
        }

        List<int> second = [];
        await foreach (int item in items)
        {
            second.Add(item);
        }

        first.Assert().Is(second);
    }

    [Theory, RandomData]
    internal static async Task GetItems_Cancel(IAsyncEnumerable<int> items)
    {
        await items.GetAsyncEnumerator(TestContext.Current.CancellationToken).DisposeAsync();
    }

    [Theory, RandomData]
    internal static async Task GetItems_Interrupt([Size(5)] IAsyncEnumerable<int> items)
    {
        await items.GetAsyncEnumerator(TestContext.Current.CancellationToken).DisposeAsync();

        int count = 0;
        await foreach (int item in items)
        {
            count++;
            if (count == 3)
            {
                break;
            }
        }

        count = 0;
        await foreach (int item in items)
        {
            count++;
        }
        count.Assert().Is(5);
    }
}
