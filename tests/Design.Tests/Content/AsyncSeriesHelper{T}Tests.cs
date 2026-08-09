using Werecodent.CreateAndFake.Design.Content;

namespace Werecodent.CreateAndFake.Design.Tests.Content;

public static class AsyncSeriesHelper_T_Tests
{
    [Fact]
    internal static Task AsyncSeriesHelper_T_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(AsyncSeriesHelper<>),
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task AsyncSeriesHelper_T_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            typeof(AsyncSeriesHelper<>),
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static void Empty_Cached()
    {
        AsyncSeriesHelper<string>.Empty.Assert().ReferenceEqual(AsyncSeriesHelper<string>.Empty);
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
