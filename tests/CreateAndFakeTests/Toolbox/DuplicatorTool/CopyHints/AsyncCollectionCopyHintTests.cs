using System.Collections;
using CreateAndFake.Toolbox.DuplicatorTool.CopyHints;
using CreateAndFakeTests.TestBases;

namespace CreateAndFakeTests.Toolbox.DuplicatorTool.CopyHints;

public sealed class AsyncCollectionCopyHintTests : CopyHintTestBase<AsyncCollectionCopyHint>
{
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

    public AsyncCollectionCopyHintTests() : base(_ValidTypes, _InvalidTypes) { }

    [Theory, RandomData]
    internal static void TryCopy_Empty([Size(0)] IAsyncEnumerable<int> items)
    {
        items.CreateDeepClone().Assert().Is(items);
    }

    [Theory, RandomData]
    internal static async Task CopyAsync_Interrupt([Size(5)] IAsyncEnumerable<int> original)
    {
        IAsyncEnumerable<int> items = original.CreateDeepClone();

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

        items.Assert().Is(original);
    }
}
