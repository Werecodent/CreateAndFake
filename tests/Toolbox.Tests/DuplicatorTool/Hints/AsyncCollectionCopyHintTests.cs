using Werecodent.CreateAndFake.DuplicatorTool.Hints;

namespace Werecodent.CreateAndFake.Tests.DuplicatorTool.Hints;

public sealed class AsyncCollectionCopyHintTests : CopyHintTestBase<AsyncCollectionCopyHint>
{
    [Theory, RandomData]
    internal static Task TryCopy_Empty([Size(0)] IAsyncEnumerable<int> items)
    {
        return Tools.Asserter.IsAsync(
            items,
            items.Tools().Copy(),
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal Task CopyAsync_CanCancel()
    {
        return Tools.Tester.VerifySupportsCancellationAsync(
            size =>
                (IAsyncEnumerable<int>)
                    TestInstance
                        .TryCopy(
                            Tools.Randomizer.CreateSized<IAsyncEnumerable<int>>(size),
                            CreateChainer()
                        )
                        .Data,
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
        count.Assert().Is(3);

        count = 0;
        await foreach (int item in items.WithCancellation(TestContext.Current.CancellationToken))
        {
            count++;
        }
        count.Assert().Is(5);

        await Tools.Asserter.IsAsync(original, items, TestContext.Current.CancellationToken);
    }
}
