using System.Collections;
using Werecodent.CreateAndFake.DuplicatorTool.Hints;
using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Tests.DuplicatorTool.Hints;

public sealed class AsyncCollectionCopyHintTests : CopyHintTestBase<AsyncCollectionCopyHint>
{
    private static readonly Type[] _ValidTypes =
    [
        typeof(IAsyncEnumerable<int>),
        typeof(IAsyncEnumerable<string>),
        typeof(IAsyncEnumerable<object>),
    ];

    private static readonly Type[] _InvalidTypes = [typeof(DataHolderSample), typeof(IEnumerable)];

    public AsyncCollectionCopyHintTests()
        : base(_ValidTypes, _InvalidTypes) { }

    [Theory, RandomData]
    internal static Task TryCopy_Empty([Size(0)] IAsyncEnumerable<int> items)
    {
        return Tools.Asserter.IsAsync(
            items,
            items.Tools().Copy(),
            TestContext.Current.CancellationToken
        );
    }

    [Theory, RandomData]
    internal static async Task CopyAsync_Interrupt([Size(5)] IAsyncEnumerable<int> original)
    {
        IAsyncEnumerable<int> items = original.Tools().Copy();

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

        await Tools.Asserter.IsAsync(original, items, TestContext.Current.CancellationToken);
    }
}
