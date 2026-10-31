using Werecodent.CreateAndFake.Design.Content;

namespace Werecodent.CreateAndFake.Design.Tests.Content;

public sealed class AsyncSeriesHelper_T_Tests() : AutoTester(typeof(AsyncSeriesHelper<>))
{
    [Fact]
    internal static void Empty_Cached()
    {
        Tools.Asserter.ReferenceEqual(
            AsyncSeriesHelper<string>.Empty,
            AsyncSeriesHelper<string>.Empty
        );
    }

    [Fact]
    internal static Task Empty_IsEmpty()
    {
        return AsyncSeriesHelper<string>
            .Empty.Assert()
            .IsEmptyAsync(TestContext.Current.CancellationToken);
    }

    [Fact]
    internal static async Task Empty_DefaultCurrent()
    {
        await using IAsyncEnumerator<int> series = AsyncSeriesHelper<int>.Empty.GetAsyncEnumerator(
            TestContext.Current.CancellationToken
        );
        series.Current.Assert().Is(default(int));
    }
}
