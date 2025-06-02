using System.Collections;
using CreateAndFake.DuplicatorTool.Hints;

namespace CreateAndFake.Tests.DuplicatorTool.Hints;

public sealed class AsyncCollectionCopyHintTests : CopyHintTestBase<AsyncCollectionCopyHint>
{
    private static readonly Type[] _ValidTypes =
    [
        typeof(IAsyncEnumerable<int>),
        typeof(IAsyncEnumerable<string>),
        typeof(IAsyncEnumerable<object>),
    ];

    private static readonly Type[] _InvalidTypes = [typeof(object), typeof(IEnumerable)];

    public AsyncCollectionCopyHintTests()
        : base(_ValidTypes, _InvalidTypes) { }

    [Theory, RandomData]
    internal static Task TryCopy_Empty([Size(0)] IAsyncEnumerable<int> items)
    {
        return Tools.AsyncAsserter.Is(items, items.CreateDeepClone());
    }

    [Theory, RandomData]
    internal static async Task CopyAsync_Interrupt([Size(5)] IAsyncEnumerable<int> original)
    {
        IAsyncEnumerable<int> items = original.CreateDeepClone();

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

        await Tools.AsyncAsserter.Is(original, items);
    }
}
