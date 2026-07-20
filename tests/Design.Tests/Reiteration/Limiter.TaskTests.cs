using System.Diagnostics;
using CreateAndFake.Design.Content;
using CreateAndFake.Design.Reiteration;

namespace CreateAndFake.Design.Tests.Reiteration;

public static class LimiterTaskTests
{
    private const int _WaitAccuracy = 5;

    private static readonly TimeSpan _SmallDelay = new(0, 0, 0, 0, 20);

    [Theory, InlineData(1), InlineData(5)]
    internal static Task RepeatAsync_TryLimited(int tries)
    {
        int attempts = 0;

        return new Limiter(tries)
            .RepeatAsync(
                GetAMessage(),
                ToTask(() => attempts++),
                TestContext.Current.CancellationToken
            )
            .Assert()
            .ThrowsNoAsync<Exception>(TestContext.Current.CancellationToken)
            .Also(() => attempts)
            .Is(tries);
    }

    [Theory, InlineData(1), InlineData(3)]
    internal static async Task StallUntilAsync_TryLimited(int tries)
    {
        int attempts = 0;
        await new Limiter(tries)
            .StallUntilAsync(
                GetAMessage(),
                ToTask(() => attempts++),
                () => false,
                TestContext.Current.CancellationToken
            )
            .Assert()
            .ThrowsAsync<TimeoutException>(TestContext.Current.CancellationToken)
            .Also(() => attempts)
            .Is(tries);

        attempts = 0;
        await new Limiter(tries)
            .StallUntilAsync(
                GetAMessage(),
                ToTask(() => attempts++),
                () => Task.FromResult(false),
                TestContext.Current.CancellationToken
            )
            .Assert()
            .ThrowsAsync<TimeoutException>(TestContext.Current.CancellationToken)
            .Also(() => attempts)
            .Is(tries);
    }

    [Theory, InlineData(1), InlineData(3)]
    internal static Task RetryAsync_TryLimited(int tries)
    {
        Exception exception = Tools.Randomizer.Create<Exception>();
        int attempts = 0;

        return new Limiter(tries)
            .RetryAsync(
                GetAMessage(),
                ToTask(() =>
                {
                    attempts++;
                    throw exception;
                }),
                TestContext.Current.CancellationToken
            )
            .Assert()
            .ThrowsAsync<TimeoutException>(TestContext.Current.CancellationToken)
            .That()
            .HasInnerException(exception)
            .Also(() => attempts)
            .Is(tries);
    }

    [Theory, InlineData(1), InlineData(3)]
    internal static Task AttemptAsync_TryLimited(int tries)
    {
        Exception exception = Tools.Randomizer.Create<Exception>();
        int attempts = 0;

        return new Limiter(tries)
            .AttemptAsync(
                GetAMessage(),
                ToTask(() =>
                {
                    attempts++;
                    throw exception;
                }),
                TestContext.Current.CancellationToken
            )
            .Assert()
            .ThrowsNoAsync<Exception>(TestContext.Current.CancellationToken)
            .Also(() => attempts)
            .Is(tries);
    }

    [Fact]
    internal static Task RepeatAsync_TimeoutLimited()
    {
        Stopwatch watch = Stopwatch.StartNew();

        return new Limiter(_SmallDelay)
            .RepeatAsync(
                GetAMessage(),
                () => Task.CompletedTask,
                TestContext.Current.CancellationToken
            )
            .Assert()
            .ThrowsNoAsync<Exception>(TestContext.Current.CancellationToken)
            .Also(() => watch.Elapsed.TotalMilliseconds)
            .GreaterThanOrEqualTo(_SmallDelay.TotalMilliseconds - _WaitAccuracy);
    }

    [Fact]
    internal static async Task StallUntilAsync_TimeoutLimited()
    {
        Stopwatch watch = Stopwatch.StartNew();
        await new Limiter(_SmallDelay)
            .StallUntilAsync(
                GetAMessage(),
                () => Task.CompletedTask,
                () => false,
                TestContext.Current.CancellationToken
            )
            .Assert()
            .ThrowsAsync<TimeoutException>(TestContext.Current.CancellationToken)
            .Also(() => watch.Elapsed.TotalMilliseconds)
            .GreaterThanOrEqualTo(_SmallDelay.TotalMilliseconds - _WaitAccuracy);

        watch.Restart();
        await new Limiter(_SmallDelay)
            .StallUntilAsync(
                GetAMessage(),
                () => Task.CompletedTask,
                () => Task.FromResult(false),
                TestContext.Current.CancellationToken
            )
            .Assert()
            .ThrowsAsync<TimeoutException>(TestContext.Current.CancellationToken)
            .Also(() => watch.Elapsed.TotalMilliseconds)
            .GreaterThanOrEqualTo(_SmallDelay.TotalMilliseconds - _WaitAccuracy);
    }

    [Theory, RandomData]
    internal static Task RetryAsync_TimeoutLimited(Exception exception)
    {
        Stopwatch watch = Stopwatch.StartNew();

        return new Limiter(_SmallDelay)
            .RetryAsync(
                GetAMessage(),
                () => Task.FromException(exception),
                TestContext.Current.CancellationToken
            )
            .Assert()
            .ThrowsAsync<TimeoutException>(TestContext.Current.CancellationToken)
            .That()
            .HasInnerException(exception)
            .Also(() => watch.Elapsed.TotalMilliseconds)
            .GreaterThanOrEqualTo(_SmallDelay.TotalMilliseconds - _WaitAccuracy);
    }

    [Theory, RandomData]
    internal static Task AttemptAsync_TimeoutLimited(Exception exception)
    {
        Stopwatch watch = Stopwatch.StartNew();

        return new Limiter(_SmallDelay)
            .AttemptAsync(
                GetAMessage(),
                ToTask(() => watch.IsRunning ? throw exception : new object()),
                TestContext.Current.CancellationToken
            )
            .Assert()
            .HasResultAsync(TestContext.Current.CancellationToken)
            .That()
            .IsNull()
            .Also(() => watch.Elapsed.TotalMilliseconds)
            .GreaterThanOrEqualTo(_SmallDelay.TotalMilliseconds - _WaitAccuracy);
    }

    [Theory, InlineData(1), InlineData(2), InlineData(3)]
    internal static Task RepeatAsync_DelayOccurs(int tries)
    {
        int attempts = 0;
        Stopwatch watch = Stopwatch.StartNew();

        return new Limiter(tries, _SmallDelay)
            .RepeatAsync(
                GetAMessage(),
                ToTask(() => attempts++),
                TestContext.Current.CancellationToken
            )
            .Assert()
            .ThrowsNoAsync<Exception>(TestContext.Current.CancellationToken)
            .Also(() => attempts)
            .Is(tries)
            .Also(() => watch.Elapsed.TotalMilliseconds)
            .GreaterThanOrEqualTo((_SmallDelay.TotalMilliseconds - _WaitAccuracy) * (tries - 1));
    }

    [Theory, InlineData(1), InlineData(2), InlineData(3)]
    internal static Task StallUntilAsync_DelayOccurs(int tries)
    {
        int attempts = 0;
        Stopwatch watch = Stopwatch.StartNew();

        return new Limiter(tries, _SmallDelay)
            .StallUntilAsync(
                GetAMessage(),
                ToTask(() => ++attempts == tries),
                TestContext.Current.CancellationToken
            )
            .Assert()
            .ThrowsNoAsync<Exception>(TestContext.Current.CancellationToken)
            .Also(() => watch.Elapsed.TotalMilliseconds)
            .GreaterThanOrEqualTo((_SmallDelay.TotalMilliseconds - _WaitAccuracy) * (tries - 1));
    }

    [Theory, InlineData(1), InlineData(2), InlineData(3)]
    internal static Task RetryAsync_DelayOccurs(int tries)
    {
        Exception exception = Tools.Randomizer.Create<Exception>();
        int attempts = 0;
        Stopwatch watch = Stopwatch.StartNew();

        return new Limiter(tries, _SmallDelay)
            .RetryAsync(
                GetAMessage(),
                ToTask(() =>
                {
                    if (++attempts < tries)
                    {
                        throw exception;
                    }
                }),
                TestContext.Current.CancellationToken
            )
            .Assert()
            .ThrowsNoAsync<Exception>(TestContext.Current.CancellationToken)
            .Also(() => watch.Elapsed.TotalMilliseconds)
            .GreaterThanOrEqualTo((_SmallDelay.TotalMilliseconds - _WaitAccuracy) * (tries - 1));
    }

    [Theory, InlineData(1), InlineData(2), InlineData(3)]
    internal static Task AttemptAsync_DelayOccurs(int tries)
    {
        Exception exception = Tools.Randomizer.Create<Exception>();
        int attempts = 0;
        Stopwatch watch = Stopwatch.StartNew();

        return new Limiter(tries, _SmallDelay)
            .AttemptAsync(
                GetAMessage(),
                ToTask(() =>
                {
                    if (++attempts < tries)
                    {
                        throw exception;
                    }
                }),
                TestContext.Current.CancellationToken
            )
            .Assert()
            .ThrowsNoAsync<Exception>(TestContext.Current.CancellationToken)
            .Also(() => watch.Elapsed.TotalMilliseconds)
            .GreaterThanOrEqualTo((_SmallDelay.TotalMilliseconds - _WaitAccuracy) * (tries - 1));
    }

    [Fact]
    internal static async Task RepeatAsync_Cancelable()
    {
        await Limiter
            .Slow.RepeatAsync(GetAMessage(), () => Task.CompletedTask, new CancellationToken(true))
            .Assert()
            .ThrowsAsync<OperationCanceledException>(TestContext.Current.CancellationToken);

        using CancellationTokenSource tokenSource = new();

        await Limiter
            .Few.RepeatAsync(
                GetAMessage(),
                () => AsyncSeriesHelper.TriggerCancellationAsync(tokenSource),
                tokenSource.Token
            )
            .Assert()
            .ThrowsAsync<OperationCanceledException>(TestContext.Current.CancellationToken);
    }

    [Fact]
    internal static async Task StallUntilAsync_Cancelable()
    {
        await Limiter
            .Fast.StallUntilAsync(
                GetAMessage(),
                () => Task.FromResult(false),
                new CancellationToken(true)
            )
            .Assert()
            .ThrowsAsync<OperationCanceledException>(TestContext.Current.CancellationToken);

        using CancellationTokenSource tokenSource = new();
        await Limiter
            .Few.StallUntilAsync(
                GetAMessage(),
                () => AsyncSeriesHelper.TriggerCancellationAsync(tokenSource),
                () => false,
                tokenSource.Token
            )
            .Assert()
            .ThrowsAsync<OperationCanceledException>(TestContext.Current.CancellationToken);

        using CancellationTokenSource tokenSource2 = new();
        await Limiter
            .Few.StallUntilAsync(
                GetAMessage(),
                () => AsyncSeriesHelper.TriggerCancellationAsync(tokenSource2),
                () => Task.FromResult(false),
                tokenSource2.Token
            )
            .Assert()
            .ThrowsAsync<OperationCanceledException>(TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal static async Task RetryAsync_Cancelable(Exception exception)
    {
        await Limiter
            .Slow.RetryAsync(
                GetAMessage(),
                () => Task.FromException(exception),
                new CancellationToken(true)
            )
            .Assert()
            .ThrowsAsync<OperationCanceledException>(TestContext.Current.CancellationToken);

        using CancellationTokenSource tokenSource = new();
        await Limiter
            .Few.RetryAsync(
                GetAMessage(),
                () => Task.FromException(exception),
                tokenSource.Cancel,
                tokenSource.Token
            )
            .Assert()
            .ThrowsAsync<OperationCanceledException>(TestContext.Current.CancellationToken);

        using CancellationTokenSource tokenSource2 = new();
        await Limiter
            .Few.RetryAsync(
                GetAMessage(),
                () => Task.FromException(exception),
                () => AsyncSeriesHelper.TriggerCancellationAsync(tokenSource2),
                tokenSource2.Token
            )
            .Assert()
            .ThrowsAsync<OperationCanceledException>(TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal static async Task AttemptAsync_Cancelable(Exception exception)
    {
        await Limiter
            .Slow.AttemptAsync(
                GetAMessage(),
                () => Task.FromException(exception),
                new CancellationToken(true)
            )
            .Assert()
            .ThrowsAsync<OperationCanceledException>(TestContext.Current.CancellationToken);

        using CancellationTokenSource tokenSource = new();
        await Limiter
            .Few.AttemptAsync(
                GetAMessage(),
                () => Task.FromException(exception),
                tokenSource.Cancel,
                tokenSource.Token
            )
            .Assert()
            .ThrowsAsync<OperationCanceledException>(TestContext.Current.CancellationToken);

        using CancellationTokenSource tokenSource2 = new();
        await Limiter
            .Few.AttemptAsync(
                GetAMessage(),
                () => Task.FromException(exception),
                () => AsyncSeriesHelper.TriggerCancellationAsync(tokenSource2),
                tokenSource2.Token
            )
            .Assert()
            .ThrowsAsync<OperationCanceledException>(TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal static Task RepeatAsync_ResultsValid(List<int> data)
    {
        int attemptAsync = 0;

        return new Limiter(data.Count)
            .RepeatAsync(
                GetAMessage(),
                ToTask(() => data[attemptAsync++]),
                TestContext.Current.CancellationToken
            )
            .Assert()
            .HasResultAsync(TestContext.Current.CancellationToken)
            .That()
            .Is(data.AsReadOnly());
    }

    [Theory, RandomData]
    internal static Task StallUntilAsync_ResultsValid(List<int> data)
    {
        int attemptAsync = 0;

        return new Limiter(data.Count)
            .StallUntilAsync(
                GetAMessage(),
                ToTask(() => data[attemptAsync++]),
                ToTask(() => attemptAsync == data.Count),
                TestContext.Current.CancellationToken
            )
            .Assert()
            .HasResultAsync(TestContext.Current.CancellationToken)
            .That()
            .Is(data.AsReadOnly());
    }

    [Theory, RandomData]
    internal static Task RetryAsync_ResultsValid(int data)
    {
        return new Limiter(1)
            .RetryAsync(
                GetAMessage(),
                () => Task.FromResult(data),
                TestContext.Current.CancellationToken
            )
            .Assert()
            .IsAsync(Task.FromResult(data), TestContext.Current.CancellationToken);
    }

    [Fact]
    internal static async Task RetryAsync_Continues()
    {
        int calls = 0;

        await new Limiter(2)
            .RetryAsync<ArithmeticException>(
                GetAMessage(),
                ToTask(() =>
                {
                    calls++;
                    if (calls == 1)
                    {
                        throw new ArithmeticException();
                    }
                }),
                TestContext.Current.CancellationToken
            )
            .Assert()
            .ThrowsNoAsync<Exception>(TestContext.Current.CancellationToken)
            .Also(() => calls)
            .Is(2);

        await new Limiter(2)
            .RetryAsync<SystemException>(
                GetAMessage(),
                ToTask(() =>
                {
                    calls++;
                    if (calls == 3)
                    {
                        throw new ArithmeticException();
                    }
                }),
                TestContext.Current.CancellationToken
            )
            .Assert()
            .ThrowsNoAsync<Exception>(TestContext.Current.CancellationToken)
            .Also(() => calls)
            .Is(4);
    }

    [Theory, RandomData]
    internal static Task AttemptAsync_ResultsValid(int data)
    {
        return new Limiter(1)
            .AttemptAsync(
                GetAMessage(),
                () => Task.FromResult(data),
                TestContext.Current.CancellationToken
            )
            .Assert()
            .IsAsync(Task.FromResult(data), TestContext.Current.CancellationToken);
    }

    [Fact]
    internal static async Task AttemptAsync_Continues()
    {
        int calls = 0;

        await new Limiter(2)
            .AttemptAsync<ArithmeticException>(
                GetAMessage(),
                ToTask(() =>
                {
                    calls++;
                    if (calls == 1)
                    {
                        throw new ArithmeticException();
                    }
                }),
                TestContext.Current.CancellationToken
            )
            .Assert()
            .ThrowsNoAsync<Exception>(TestContext.Current.CancellationToken)
            .Also(() => calls)
            .Is(2);

        await new Limiter(2)
            .AttemptAsync<SystemException>(
                GetAMessage(),
                ToTask(() =>
                {
                    calls++;
                    if (calls == 3)
                    {
                        throw new ArithmeticException();
                    }
                }),
                TestContext.Current.CancellationToken
            )
            .Assert()
            .ThrowsNoAsync<Exception>(TestContext.Current.CancellationToken)
            .Also(() => calls)
            .Is(4);
    }

    [Theory, RandomData]
    internal static Task AttemptAsync_DefaultOnFail(Exception error)
    {
        return new Limiter(1)
            .AttemptAsync(
                GetAMessage(),
                ToTask(() => error != null ? throw error : 1),
                ToTask(() => Task.FromResult<object>(null)),
                TestContext.Current.CancellationToken
            )
            .Assert()
            .HasResultAsync(TestContext.Current.CancellationToken)
            .That()
            .Is(0);
    }

    [Theory, InlineData(1), InlineData(3)]
    internal static async Task StallUntilAsync_CheckStateBehavior(int tries)
    {
        int attemptAsync = 0;
        int checkAttemptAsync = 0;

        await new Limiter(tries)
            .StallUntilAsync(
                GetAMessage(),
                ToTask(() => attemptAsync++),
                () => ++checkAttemptAsync == tries,
                TestContext.Current.CancellationToken
            )
            .Assert()
            .ThrowsNoAsync<Exception>(TestContext.Current.CancellationToken)
            .Also(() => attemptAsync)
            .Is(tries)
            .Also(() => checkAttemptAsync)
            .Is(tries);

        attemptAsync = 0;
        checkAttemptAsync = 0;

        await new Limiter(tries)
            .StallUntilAsync(
                GetAMessage(),
                ToTask(() => attemptAsync++),
                ToTask(() => ++checkAttemptAsync == tries),
                TestContext.Current.CancellationToken
            )
            .Assert()
            .ThrowsNoAsync<Exception>(TestContext.Current.CancellationToken)
            .Also(() => attemptAsync)
            .Is(tries)
            .Also(() => checkAttemptAsync)
            .Is(tries);
    }

    [Theory, InlineData(1), InlineData(3)]
    internal static async Task RetryAsync_ResetStateBehavior(int tries)
    {
        Exception exception = Tools.Randomizer.Create<Exception>();
        int attemptAsync = 0;
        int resetAttemptAsync = 0;

        await new Limiter(tries)
            .RetryAsync(
                GetAMessage(),
                ToTask(() =>
                {
                    if (++attemptAsync < tries)
                    {
                        throw exception;
                    }
                }),
                () => resetAttemptAsync++,
                TestContext.Current.CancellationToken
            )
            .Assert()
            .ThrowsNoAsync<Exception>(TestContext.Current.CancellationToken)
            .Also(() => attemptAsync)
            .Is(tries)
            .Also(() => resetAttemptAsync)
            .Is(tries - 1);

        attemptAsync = 0;
        resetAttemptAsync = 0;

        await new Limiter(tries)
            .RetryAsync(
                GetAMessage(),
                ToTask(() =>
                {
                    if (++attemptAsync < tries)
                    {
                        throw exception;
                    }
                }),
                ToTask((Action)(() => _ = resetAttemptAsync++)),
                TestContext.Current.CancellationToken
            )
            .Assert()
            .ThrowsNoAsync<Exception>(TestContext.Current.CancellationToken)
            .Also(() => attemptAsync)
            .Is(tries)
            .Also(() => resetAttemptAsync)
            .Is(tries - 1);
    }

    [Theory, InlineData(1), InlineData(3)]
    internal static async Task RetryAsync_ReturnResetStateBehavior(int tries)
    {
        Exception exception = Tools.Randomizer.Create<Exception>();
        int attemptAsync = 0;
        int resetAttemptAsync = 0;

        int result = Tools.Randomizer.Create<int>();
        int resetBehavior()
        {
            return (++attemptAsync == tries) ? result : throw exception;
        }

        await new Limiter(tries)
            .RetryAsync(
                GetAMessage(),
                ToTask(resetBehavior),
                () => resetAttemptAsync++,
                TestContext.Current.CancellationToken
            )
            .Assert()
            .IsAsync(Task.FromResult(result), TestContext.Current.CancellationToken)
            .Also(() => attemptAsync)
            .Is(tries)
            .Also(() => resetAttemptAsync)
            .Is(tries - 1);

        attemptAsync = 0;
        resetAttemptAsync = 0;
        await new Limiter(tries)
            .RetryAsync(
                GetAMessage(),
                ToTask(resetBehavior),
                ToTask((Action)(() => _ = resetAttemptAsync++)),
                TestContext.Current.CancellationToken
            )
            .Assert()
            .IsAsync(Task.FromResult(result), TestContext.Current.CancellationToken)
            .Also(() => attemptAsync)
            .Is(tries)
            .Also(() => resetAttemptAsync)
            .Is(tries - 1);
    }

    [Theory, RandomData]
    internal static async Task RetryAsync_WrongExceptionThrows(NotSupportedException exception)
    {
        await new Limiter(3)
            .RetryAsync<InvalidOperationException>(
                GetAMessage(),
                () => Task.FromException(exception),
                TestContext.Current.CancellationToken
            )
            .Assert()
            .ThrowsAsync<NotSupportedException>(TestContext.Current.CancellationToken)
            .That()
            .Is(exception);

        IOException exception2 = new();

        await new Limiter(3)
            .RetryAsync<DirectoryNotFoundException, bool>(
                GetAMessage(),
                ToTask(() => exception2 != null ? throw exception2 : false),
                TestContext.Current.CancellationToken
            )
            .Assert()
            .ThrowsAsync<IOException>(TestContext.Current.CancellationToken)
            .That()
            .Is(exception2);
    }

    [Theory, InlineData(1), InlineData(3)]
    internal static async Task AttemptAsync_ResetStateBehavior(int tries)
    {
        Exception exception = Tools.Randomizer.Create<Exception>();
        int attemptAsync = 0;
        int resetAttemptAsync = 0;

        await new Limiter(tries)
            .AttemptAsync(
                GetAMessage(),
                ToTask(() =>
                {
                    if (++attemptAsync < tries)
                    {
                        throw exception;
                    }
                }),
                () => resetAttemptAsync++,
                TestContext.Current.CancellationToken
            )
            .Assert()
            .ThrowsNoAsync<Exception>(TestContext.Current.CancellationToken)
            .Also(() => attemptAsync)
            .Is(tries)
            .Also(() => resetAttemptAsync)
            .Is(tries - 1);

        attemptAsync = 0;
        resetAttemptAsync = 0;

        await new Limiter(tries)
            .AttemptAsync(
                GetAMessage(),
                ToTask(() =>
                {
                    if (++attemptAsync < tries)
                    {
                        throw exception;
                    }
                }),
                ToTask((Action)(() => _ = resetAttemptAsync++)),
                TestContext.Current.CancellationToken
            )
            .Assert()
            .ThrowsNoAsync<Exception>(TestContext.Current.CancellationToken)
            .Also(() => attemptAsync)
            .Is(tries)
            .Also(() => resetAttemptAsync)
            .Is(tries - 1);
    }

    [Theory, InlineData(1), InlineData(3)]
    internal static async Task AttemptAsync_ReturnResetStateBehavior(int tries)
    {
        Exception exception = Tools.Randomizer.Create<Exception>();
        int attemptAsync = 0;
        int resetAttemptAsync = 0;

        int result = Tools.Randomizer.Create<int>();
        int resetBehavior()
        {
            return (++attemptAsync == tries) ? result : throw exception;
        }

        await new Limiter(tries)
            .AttemptAsync(
                GetAMessage(),
                ToTask(resetBehavior),
                () => resetAttemptAsync++,
                TestContext.Current.CancellationToken
            )
            .Assert()
            .HasResultAsync(TestContext.Current.CancellationToken)
            .That()
            .Is(result)
            .Also(() => attemptAsync)
            .Is(tries)
            .Also(() => resetAttemptAsync)
            .Is(tries - 1);

        attemptAsync = 0;
        resetAttemptAsync = 0;

        await new Limiter(tries)
            .AttemptAsync(
                GetAMessage(),
                ToTask(resetBehavior),
                ToTask((Action)(() => _ = resetAttemptAsync++)),
                TestContext.Current.CancellationToken
            )
            .Assert()
            .HasResultAsync(TestContext.Current.CancellationToken)
            .That()
            .Is(result)
            .Also(() => attemptAsync)
            .Is(tries)
            .Also(() => resetAttemptAsync)
            .Is(tries - 1);
    }

    [Theory, RandomData]
    internal static async Task AttemptAsync_WrongExceptionThrows(NotSupportedException exception)
    {
        await new Limiter(3)
            .AttemptAsync<InvalidOperationException>(
                GetAMessage(),
                () => Task.FromException(exception),
                TestContext.Current.CancellationToken
            )
            .Assert()
            .ThrowsAsync<NotSupportedException>(TestContext.Current.CancellationToken)
            .That()
            .Is(exception);

        IOException exception2 = new();

        await new Limiter(3)
            .AttemptAsync<DirectoryNotFoundException, bool>(
                GetAMessage(),
                ToTask(() => exception2 != null ? throw exception2 : false),
                TestContext.Current.CancellationToken
            )
            .Assert()
            .ThrowsAsync<IOException>(TestContext.Current.CancellationToken)
            .That()
            .Is(exception2);
    }

    private static Func<Task> ToTask(Action behavior)
    {
        return () => Task.Run(behavior, TestContext.Current.CancellationToken);
    }

    private static Func<Task<T>> ToTask<T>(Func<T> behavior)
    {
        return () => Task.Run(behavior, TestContext.Current.CancellationToken);
    }

    private static string GetAMessage()
    {
        return Tools.Gen.NextItem(["", null, "Test."]);
    }
}
