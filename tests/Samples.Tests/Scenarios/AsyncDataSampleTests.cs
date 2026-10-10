using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Samples.Tests.Scenarios;

public static class AsyncDataSampleTests
{
    [Fact]
    public static Task AsyncDataSample_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<AsyncDataSample>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    public static Task AsyncDataSample_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<AsyncDataSample>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    public static Task ReadFromNumberValueAsync_InitialDefault()
    {
        return new AsyncDataSample()
            .ReadFromNumberValueAsync()
            .Assert()
            .HasResultAsync(default, TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    public static Task ReadFromNumberValueAsync_CorrectValue(int value)
    {
        return new AsyncDataSample() { NumberValue = Task.FromResult(value) }
            .ReadFromNumberValueAsync()
            .Assert()
            .HasResultAsync(value, TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    public static Task ReadFromNumberValueAsync_WithDelay(int value)
    {
        async Task<int> getNumber()
        {
            await Task.Delay(1000, TestContext.Current.CancellationToken);
            return value;
        }

        return new AsyncDataSample() { NumberValue = getNumber() }
            .ReadFromNumberValueAsync()
            .Assert()
            .HasResultAsync(value, TestContext.Current.CancellationToken);
    }
}
