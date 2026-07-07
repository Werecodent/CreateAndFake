using CreateAndFake.Design.Content;
using CreateAndFake.Design.Exceptions;
using CreateAndFake.Design.Properties;

namespace CreateAndFake.Design.Tests.Content;

public static class AsyncSeriesHelperTests
{
    [Fact]
    internal static Task AsyncSeriesHelper_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(AsyncSeriesHelper),
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task AsyncSeriesHelper_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            typeof(AsyncSeriesHelper),
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = [typeof(IterationLimitException)] }
        );
    }

    [Theory, RandomData]
    internal static Task CreateFromAsync_ConvertsObjectsSuccessfully(IList<string> data)
    {
        return AsyncSeriesHelper
            .CreateFromAsync(data, data.Count, TestContext.Current.CancellationToken)
            .Assert()
            .IsAsync(data, TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal static async Task CreateFromAsync_CanBeCanceled(IList<string> data)
    {
        try
        {
            await foreach (
                string value in AsyncSeriesHelper
                    .CreateFromAsync(data, data.Count, TestContext.Current.CancellationToken)
                    .WithCancellation(new CancellationToken(true))
            )
            {
                value.Assert().Fail();
            }
        }
        catch (OperationCanceledException)
        {
            data.Assert().Pass();
        }
    }

    [Fact]
    internal static Task HasAnyAsync_FalseWhenGivenNull()
    {
        return AsyncSeriesHelper
            .HasAnyAsync<string>(null, TestContext.Current.CancellationToken)
            .Assert()
            .IsAsync(Task.FromResult(false), TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal static Task HasAnyAsync_FalseWithNoValues([Size(0)] IAsyncEnumerable<string> data)
    {
        return AsyncSeriesHelper
            .HasAnyAsync(data, TestContext.Current.CancellationToken)
            .Assert()
            .IsAsync(Task.FromResult(false), TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal static Task HasAnyAsync_TrueWithValues(IAsyncEnumerable<string> data)
    {
        return AsyncSeriesHelper
            .HasAnyAsync(data, TestContext.Current.CancellationToken)
            .Assert()
            .IsAsync(Task.FromResult(true), TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal static Task HasAnyAsync_CanBeCanceledInitially([Size(0)] IAsyncEnumerable<string> data)
    {
        return AsyncSeriesHelper
            .HasAnyAsync(data, new CancellationToken(true))
            .Assert()
            .ThrowsAsync<OperationCanceledException>(TestContext.Current.CancellationToken);
    }

    [Fact]
    internal static async Task HasAnyAsync_CanBeCanceledAtIteration()
    {
        using CancellationTokenSource source = new();

        await AsyncSeriesHelper
            .HasAnyAsync(AsyncSeriesHelper.CreateCancelingIteration<string>(source), source.Token)
            .Assert()
            .ThrowsAsync<OperationCanceledException>(TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal static async Task ForEachAsync_IteratesSuccessfully(
        [Size(2)] IAsyncEnumerable<string> data
    )
    {
        List<string> results = [];
        await AsyncSeriesHelper.ForEachAsync(data, 2, new CancellationToken(false), results.Add);
        await results.Assert().IsAsync(data, TestContext.Current.CancellationToken);

        results.Clear();
        await AsyncSeriesHelper.ForEachAsync(
            data,
            2,
            new CancellationToken(false),
            v =>
            {
                results.Add(v);
                return Task.CompletedTask;
            }
        );
        await results.Assert().IsAsync(data, TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal static async Task ForEachAsync_HasIterationLimit(
        [Size(2)] IAsyncEnumerable<string> data
    )
    {
        await AsyncSeriesHelper
            .ForEachAsync(data, 1, new CancellationToken(false), _ => { })
            .Assert()
            .ThrowsAsync<IterationLimitException>(TestContext.Current.CancellationToken);

        await AsyncSeriesHelper
            .ForEachAsync(data, 1, new CancellationToken(false), _ => Task.CompletedTask)
            .Assert()
            .ThrowsAsync<IterationLimitException>(TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal static async Task ForEachAsync_CanBeCanceledInitially(
        [Size(0)] IAsyncEnumerable<string> data
    )
    {
        await AsyncSeriesHelper
            .ForEachAsync(data, 0, new CancellationToken(true), _ => { })
            .Assert()
            .ThrowsAsync<OperationCanceledException>(TestContext.Current.CancellationToken);

        await AsyncSeriesHelper
            .ForEachAsync(data, 0, new CancellationToken(true), _ => Task.CompletedTask)
            .Assert()
            .ThrowsAsync<OperationCanceledException>(TestContext.Current.CancellationToken);
    }

    [Fact]
    internal static async Task ForEachAsync_CanBeCanceledAtIteration()
    {
        using CancellationTokenSource source = new();
        await AsyncSeriesHelper
            .ForEachAsync(
                AsyncSeriesHelper.CreateCancelingIteration<string>(source),
                10,
                source.Token,
                _ => { }
            )
            .Assert()
            .ThrowsAsync<OperationCanceledException>(TestContext.Current.CancellationToken);

        using CancellationTokenSource source2 = new();
        await AsyncSeriesHelper
            .ForEachAsync(
                AsyncSeriesHelper.CreateCancelingIteration<string>(source2),
                10,
                source.Token,
                _ => Task.CompletedTask
            )
            .Assert()
            .ThrowsAsync<OperationCanceledException>(TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal static async Task ForEachAsync_CanBeCanceledAfterIterating(
        [Size(1)] ICollection<string> data
    )
    {
        using CancellationTokenSource source = new();
        await AsyncSeriesHelper
            .ForEachAsync(
                AsyncSeriesHelper.CreateCancelingIteration(data, source),
                data.Count,
                source.Token,
                _ => { }
            )
            .Assert()
            .ThrowsAsync<OperationCanceledException>(TestContext.Current.CancellationToken);

        using CancellationTokenSource source2 = new();
        await AsyncSeriesHelper
            .ForEachAsync(
                AsyncSeriesHelper.CreateCancelingIteration(data, source2),
                data.Count,
                source.Token,
                _ => Task.CompletedTask
            )
            .Assert()
            .ThrowsAsync<OperationCanceledException>(TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal static async Task ToListAsync_ConvertsValues(IAsyncEnumerable<string> data)
    {
        await data.Assert()
            .IsAsync(
                await AsyncSeriesHelper.ToListAsync(
                    data,
                    DesignDefaults.IterationLimit,
                    TestContext.Current.CancellationToken
                ),
                TestContext.Current.CancellationToken
            );
    }

    [Theory, RandomData]
    internal static Task ToListAsync_CanBeCanceledInitially([Size(0)] IAsyncEnumerable<string> data)
    {
        return AsyncSeriesHelper
            .ToListAsync(data, 0, new CancellationToken(true))
            .Assert()
            .ThrowsAsync<OperationCanceledException>(TestContext.Current.CancellationToken);
    }

    [Fact]
    internal static async Task ToListAsync_CanBeCanceledAtIteration()
    {
        using CancellationTokenSource source = new();
        await AsyncSeriesHelper
            .ToListAsync(
                AsyncSeriesHelper.CreateCancelingIteration<string>(source),
                DesignDefaults.IterationLimit,
                source.Token
            )
            .Assert()
            .ThrowsAsync<OperationCanceledException>(TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal static async Task ToListAsync_CanBeCanceledDuringIteration(
        [Size(2)] IEnumerable<string> data
    )
    {
        using CancellationTokenSource source = new();
        await AsyncSeriesHelper
            .ToListAsync(AsyncSeriesHelper.CreateCancelingIteration(data, source), 2, source.Token)
            .Assert()
            .ThrowsAsync<OperationCanceledException>(TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal static async Task ToListAsync_CanBeCanceledAfterIterating(
        [Size(1)] IEnumerable<string> data
    )
    {
        using CancellationTokenSource source = new();
        await AsyncSeriesHelper
            .ToListAsync(AsyncSeriesHelper.CreateCancelingIteration(data, source), 1, source.Token)
            .Assert()
            .ThrowsAsync<OperationCanceledException>(TestContext.Current.CancellationToken);
    }

    [Fact]
    internal static async Task CreateCancelingIteration_EmptyCancelsAtIteration()
    {
        using CancellationTokenSource source = new();
        await foreach (
            string _ in AsyncSeriesHelper
                .CreateCancelingIteration<string>(source)
                .WithCancellation(TestContext.Current.CancellationToken)
        )
        {
            source.Assert().Fail();
        }
        source.IsCancellationRequested.Assert().Is(true);
    }

    [Theory, RandomData]
    internal static async Task CreateCancelingIteration_OnlyYieldCancelsAfterIteration(
        [Size(1)] IEnumerable<string> data
    )
    {
        using CancellationTokenSource source = new();
        await foreach (
            string _ in AsyncSeriesHelper
                .CreateCancelingIteration(data, source)
                .WithCancellation(TestContext.Current.CancellationToken)
        )
        {
            source.IsCancellationRequested.Assert().Is(false);
        }
        source.IsCancellationRequested.Assert().Is(true);
    }

    [Theory, RandomData]
    internal static async Task CreateCancelingIteration_MultipleYieldCancelsDuringIteration(
        [Size(2)] IEnumerable<string> data
    )
    {
        using CancellationTokenSource source = new();
        int i = 0;
        await foreach (
            string _ in AsyncSeriesHelper
                .CreateCancelingIteration(data, source)
                .WithCancellation(TestContext.Current.CancellationToken)
        )
        {
            if (i++ == 0)
            {
                source.IsCancellationRequested.Assert().Is(false);
            }
            else
            {
                source.IsCancellationRequested.Assert().Is(true);
            }
        }
        i.Assert().Is(2);
    }

    [Fact]
    internal static async Task TriggerCancellationAsync_Cancels()
    {
        using CancellationTokenSource source = new();
        await AsyncSeriesHelper.TriggerCancellationAsync(source);
        source.IsCancellationRequested.Assert().Is(true);
    }

    [Fact]
    internal static async Task TriggerCancellationAsync_CancelsOnce()
    {
        using CancellationTokenSource source = new();
        await AsyncSeriesHelper.TriggerCancellationAsync(source);
        await AsyncSeriesHelper.TriggerCancellationAsync(
            _ => throw new EngineException("Cancellation triggered twice."),
            source
        );
    }

    [Fact]
    internal static async Task TriggerCancellationAsync_SyncFallbackCancels()
    {
        using CancellationTokenSource source = new();
        await AsyncSeriesHelper.TriggerCancellationAsync(null, source);
        source.IsCancellationRequested.Assert().Is(true);
    }
}
