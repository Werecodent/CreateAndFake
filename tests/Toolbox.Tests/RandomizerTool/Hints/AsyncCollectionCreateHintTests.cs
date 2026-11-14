using Werecodent.CreateAndFake.Design.Content;
using Werecodent.CreateAndFake.RandomizerTool.Hints;

namespace Werecodent.CreateAndFake.Tests.RandomizerTool.Hints;

public sealed class AsyncCollectionCreateHintTests : CreateHintTestBase<AsyncCollectionCreateHint>
{
    private static readonly Type[] _ValidTypes =
    [
        typeof(AsyncHashSet<string>),
        typeof(AsyncHashSet<int>),
        typeof(IAsyncSet<string>),
        typeof(IAsyncSet<int>),
        typeof(IAsyncEnumerable<string>),
        typeof(IAsyncEnumerable<int>),
        typeof(AsyncList<string>),
        typeof(AsyncList<int>),
    ];

    public AsyncCollectionCreateHintTests()
        : base(_ValidTypes) { }

    [Theory, RandomData]
    internal static async Task TryToCreate_Empty([Size(0)] IAsyncSet<int> items)
    {
        IAsyncEnumerator<int> gen = items.GetAsyncEnumerator(TestContext.Current.CancellationToken);
        await using (gen)
        {
            (await gen.MoveNextAsync()).Assert().Is(false);
        }
    }

    [Theory, RandomData]
    internal static async Task TryToCreate_Repeatable(IAsyncSet<int> items)
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
    internal static async Task TryToCreate_Cancel(IAsyncSet<int> items)
    {
        await items.GetAsyncEnumerator(TestContext.Current.CancellationToken).DisposeAsync();
    }

    [Theory, RandomData]
    internal static async Task TryToCreate_Interrupt([Size(5)] IAsyncSet<int> items)
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

    [Theory, RandomData]
    internal static async Task TryToCreate_EmptyList([Size(0)] IAsyncEnumerable<int> items)
    {
        IAsyncEnumerator<int> gen = items.GetAsyncEnumerator(TestContext.Current.CancellationToken);
        await using (gen)
        {
            (await gen.MoveNextAsync()).Assert().Is(false);
        }
    }

    [Theory, RandomData]
    internal static async Task TryToCreate_RepeatableList(IAsyncEnumerable<int> items)
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
    internal static async Task TryToCreate_CancelList(IAsyncEnumerable<int> items)
    {
        await items.GetAsyncEnumerator(TestContext.Current.CancellationToken).DisposeAsync();
    }

    [Theory, RandomData]
    internal static async Task TryToCreate_InterruptList([Size(5)] IAsyncEnumerable<int> items)
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

    [Theory, RandomData]
    internal static Task TryToCreate_LargeAsyncSetsValid([Size(100)] IAsyncSet<object> items)
    {
        return items.Assert().HasCountAsync(100, TestContext.Current.CancellationToken);
    }
}
