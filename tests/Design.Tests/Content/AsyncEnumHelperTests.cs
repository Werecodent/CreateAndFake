using CreateAndFake.Design.Content;
using CreateAndFake.Design.Exceptions;

namespace CreateAndFake.Design.Tests.Content;

public static class AsyncEnumHelperTests
{
    [Fact]
    internal static Task AsyncEnumHelper_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException(
            typeof(AsyncEnumHelper),
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task AsyncEnumHelper_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation(
            typeof(AsyncEnumHelper),
            TestContext.Current.CancellationToken
        );
    }

    [Theory, RandomData]
    internal static Task CreateFrom_ConvertsObjectsSuccessfully(IList<string> data)
    {
        return AsyncEnumHelper
            .CreateFromAsync(data, TestContext.Current.CancellationToken)
            .Assert()
            .IsAsync(data, TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal static async Task CreateFrom_CanBeCanceled(IList<string> data)
    {
        try
        {
            await foreach (
                string value in AsyncEnumHelper
                    .CreateFromAsync(data, TestContext.Current.CancellationToken)
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
    internal static async Task HasAnyAsync_FalseWhenGivenNull()
    {
        (await AsyncEnumHelper.HasAnyAsync<string>(null, TestContext.Current.CancellationToken))
            .Assert()
            .Is(false);
    }

    [Theory, RandomData]
    internal static async Task HasAnyAsync_FalseWithNoValues(
        [Size(0)] IAsyncEnumerable<string> data
    )
    {
        (await AsyncEnumHelper.HasAnyAsync(data, TestContext.Current.CancellationToken))
            .Assert()
            .Is(false);
    }

    [Theory, RandomData]
    internal static async Task HasAnyAsync_TrueWithValues(IAsyncEnumerable<string> data)
    {
        (await AsyncEnumHelper.HasAnyAsync(data, TestContext.Current.CancellationToken))
            .Assert()
            .Is(true);
    }

    [Theory, RandomData]
    internal static Task HasAnyAsync_CanBeCanceledInitially([Size(0)] IAsyncEnumerable<string> data)
    {
        return AsyncEnumHelper
            .HasAnyAsync(data, new CancellationToken(true))
            .Assert()
            .Throws<OperationCanceledException>();
    }

    [Fact]
    internal static async Task HasAnyAsync_CanBeCanceledAtIteration()
    {
        using CancellationTokenSource source = new();
        await AsyncEnumHelper
            .HasAnyAsync(AsyncEnumHelper.CreateCancelingIteration<string>(source), source.Token)
            .Assert()
            .Throws<OperationCanceledException>();
    }

    [Theory, RandomData]
    internal static async Task ToListAsync_ConvertsValues(IAsyncEnumerable<string> data)
    {
        await data.Assert()
            .IsAsync(
                await AsyncEnumHelper.ToListAsync(data, TestContext.Current.CancellationToken),
                TestContext.Current.CancellationToken
            );
    }

    [Theory, RandomData]
    internal static Task ToListAsync_CanBeCanceledInitially([Size(0)] IAsyncEnumerable<string> data)
    {
        return AsyncEnumHelper
            .ToListAsync(data, new CancellationToken(true))
            .Assert()
            .Throws<OperationCanceledException>();
    }

    [Fact]
    internal static async Task ToListAsync_CanBeCanceledAtIteration()
    {
        using CancellationTokenSource source = new();
        await AsyncEnumHelper
            .ToListAsync(AsyncEnumHelper.CreateCancelingIteration<string>(source), source.Token)
            .Assert()
            .Throws<OperationCanceledException>();
    }

    [Theory, RandomData]
    internal static async Task ToListAsync_CanBeCanceledDuringIteration(
        [Size(2)] IEnumerable<string> data
    )
    {
        using CancellationTokenSource source = new();
        await AsyncEnumHelper
            .ToListAsync(AsyncEnumHelper.CreateCancelingIteration(data, source), source.Token)
            .Assert()
            .Throws<OperationCanceledException>();
    }

    [Theory, RandomData]
    internal static async Task ToListAsync_CanBeCanceledAfterIterating(
        [Size(1)] IEnumerable<string> data
    )
    {
        using CancellationTokenSource source = new();
        await AsyncEnumHelper
            .ToListAsync(AsyncEnumHelper.CreateCancelingIteration(data, source), source.Token)
            .Assert()
            .Throws<OperationCanceledException>();
    }

    [Fact]
    internal static async Task CreateCancelingIteration_EmptyCancelsAtIteration()
    {
        using CancellationTokenSource source = new();
        await foreach (
            string _ in AsyncEnumHelper
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
            string _ in AsyncEnumHelper
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
            string _ in AsyncEnumHelper
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
        await AsyncEnumHelper.TriggerCancellationAsync(source);
        source.IsCancellationRequested.Assert().Is(true);
    }

    [Fact]
    internal static async Task TriggerCancellationAsync_CancelsOnce()
    {
        using CancellationTokenSource source = new();
        await AsyncEnumHelper.TriggerCancellationAsync(source);
        await AsyncEnumHelper.TriggerCancellationAsync(
            _ => throw new EngineException("Cancellation triggered twice."),
            source
        );
    }

    [Fact]
    internal static async Task TriggerCancellationAsync_SyncFallbackCancels()
    {
        using CancellationTokenSource source = new();
        await AsyncEnumHelper.TriggerCancellationAsync(null, source);
        source.IsCancellationRequested.Assert().Is(true);
    }
}
