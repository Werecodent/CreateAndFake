using Werecodent.CreateAndFake.Design.Content;
using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Design.Tests.Content;

public static class AsyncListTests
{
    [Fact]
    internal static void Debug_AsyncList_ToString()
    {
        typeof(AsyncList<>).Tools().CreateRandomInstance().ToString().Assert().Debug();
    }

    [Fact]
    internal static Task AsyncList_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(AsyncList<>),
            TestContext.Current.CancellationToken
        );
    }

    [Theory, RandomData]
    internal static Task AsyncList_NoParameterMutation([Cap(6, 9)] int iterationLimit)
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            typeof(AsyncList<>),
            TestContext.Current.CancellationToken,
            opt => opt with { InjectionValues = [iterationLimit] }
        );
    }

    [Fact]
    internal static Task AsyncList_FrameworkSupport()
    {
        return Tools.Tester.VerifyToolSetSupportAsync(
            [typeof(AsyncList<>)],
            TestContext.Current.CancellationToken
        );
    }

    [Theory, RandomData]
    internal static async Task GetAsyncEnumerator_Repeatable(IReadOnlyCollection<DataSample> sample)
    {
        AsyncList<DataSample> instance = new(sample, Tools.Valuer.Options.IterationLimit);
        await instance.Assert().IsAsync(sample, TestContext.Current.CancellationToken);
        await instance.Assert().IsAsync(sample, TestContext.Current.CancellationToken);
        await instance.Assert().HasCountAsync(sample.Count, TestContext.Current.CancellationToken);
    }

    [Fact]
    internal static Task GetAsyncEnumerator_EmptyWorks()
    {
        return new AsyncList<DataSample>([], 1)
            .Assert()
            .HasCountAsync(0, TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal static Task GetAsyncEnumerator_Cancelable([Size(1)] List<DataSample> items)
    {
        return new AsyncList<DataSample>(items, 1)
            .GetAsyncEnumerator(new CancellationToken(true))
            .MoveNextAsync()
            .Assert()
            .ThrowsAsync<OperationCanceledException>(TestContext.Current.CancellationToken);
    }
}
