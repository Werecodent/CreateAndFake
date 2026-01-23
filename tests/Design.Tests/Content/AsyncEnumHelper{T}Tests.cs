using CreateAndFake.Design.Content;

namespace CreateAndFake.Design.Tests.Content;

public static class AsyncEnumHelper_T_Tests
{
    [Fact]
    internal static Task AsyncEnumHelper_T_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException(
            typeof(AsyncEnumHelper<>),
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task AsyncEnumHelper_T_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation(
            typeof(AsyncEnumHelper<string>),
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static async Task Empty_NoItemsRepeatably()
    {
        await AsyncEnumHelper<string>
            .Empty.Assert()
            .IsEmptyAsync(TestContext.Current.CancellationToken);
        await AsyncEnumHelper<string>
            .Empty.Assert()
            .IsEmptyAsync(TestContext.Current.CancellationToken);
    }

    [Fact]
    internal static async Task Empty_DefaultCurrent()
    {
        await using IAsyncEnumerator<int> series = AsyncEnumHelper<int>.Empty.GetAsyncEnumerator(
            TestContext.Current.CancellationToken
        );
        series.Current.Assert().Is(default(int));
    }
}
