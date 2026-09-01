using System.Collections;
using Werecodent.CreateAndFake.Design.Content;
using Werecodent.CreateAndFake.RandomizerTool.Hints;
using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Tests.RandomizerTool.Hints;

public sealed class AsyncHashCreateHintTests : CreateHintTestBase<AsyncHashCreateHint>
{
    private static readonly Type[] _ValidTypes =
    [
        typeof(AsyncHashSet<string>),
        typeof(AsyncHashSet<int>),
        typeof(IAsyncSet<string>),
        typeof(IAsyncSet<int>),
    ];

    private static readonly Type[] _InvalidTypes =
    [
        typeof(DataHolderSample),
        typeof(IEnumerable),
        typeof(IEnumerable<>),
    ];

    public AsyncHashCreateHintTests()
        : base(_ValidTypes, _InvalidTypes) { }

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
}
