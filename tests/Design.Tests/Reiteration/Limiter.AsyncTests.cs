using System.Diagnostics;
using CreateAndFake.Design.Reiteration;

namespace CreateAndFake.Design.Tests.Reiteration;

public static class LimiterAsyncTests
{
    private const int _WaitAccuracy = 5;

    private static readonly TimeSpan _SmallDelay = new(0, 0, 0, 0, 20);

    [Theory, InlineData(1), InlineData(5)]
    internal static Task RepeatAsync_TryLimited(int tries)
    {
        int attempts = 0;

        return new Limiter(tries)
            .RepeatAsync(GetAMessage(), () => attempts++, TestContext.Current.CancellationToken)
            .Assert()
            .ThrowsNoAsync<Exception>(TestContext.Current.CancellationToken)
            .Also(() => attempts)
            .Is(tries);
    }

    [Theory, InlineData(1), InlineData(3)]
    internal static Task StallUntilAsync_TryLimited(int tries)
    {
        int attempts = 0;

        return new Limiter(tries)
            .StallUntilAsync(
                GetAMessage(),
                () => attempts++,
                () => false,
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
                () =>
                {
                    attempts++;
                    throw exception;
                },
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
                () =>
                {
                    attempts++;
                    throw exception;
                },
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
            .RepeatAsync(GetAMessage(), () => { }, TestContext.Current.CancellationToken)
            .Assert()
            .ThrowsNoAsync<Exception>(TestContext.Current.CancellationToken)
            .Also(() => watch.Elapsed.TotalMilliseconds)
            .GreaterThanOrEqualTo(_SmallDelay.TotalMilliseconds - _WaitAccuracy);
    }

    [Fact]
    internal static Task StallUntilAsync_TimeoutLimited()
    {
        Stopwatch watch = Stopwatch.StartNew();

        return new Limiter(_SmallDelay)
            .StallUntilAsync(
                GetAMessage(),
                () => { },
                () => false,
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
            .RetryAsync(GetAMessage(), () => throw exception, TestContext.Current.CancellationToken)
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
                () => watch.IsRunning ? throw exception : new object(),
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
            .RepeatAsync(GetAMessage(), () => attempts++, TestContext.Current.CancellationToken)
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
                () => ++attempts == tries,
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
                () =>
                {
                    if (++attempts < tries)
                    {
                        throw exception;
                    }
                },
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
                () =>
                {
                    if (++attempts < tries)
                    {
                        throw exception;
                    }
                },
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
            .Quick.RepeatAsync(GetAMessage(), () => { }, new CancellationToken(true))
            .Assert()
            .ThrowsAsync<OperationCanceledException>(TestContext.Current.CancellationToken);

        using CancellationTokenSource tokenSource = new();

        await Limiter
            .Few.RepeatAsync(GetAMessage(), tokenSource.Cancel, tokenSource.Token)
            .Assert()
            .ThrowsAsync<OperationCanceledException>(TestContext.Current.CancellationToken);
    }

    [Fact]
    internal static async Task StallUntilAsync_Cancelable()
    {
        await Limiter
            .Fast.StallUntilAsync(GetAMessage(), () => false, new CancellationToken(true))
            .Assert()
            .ThrowsAsync<OperationCanceledException>(TestContext.Current.CancellationToken);

        using CancellationTokenSource tokenSource = new();

        await Limiter
            .Few.StallUntilAsync(GetAMessage(), tokenSource.Cancel, () => false, tokenSource.Token)
            .Assert()
            .ThrowsAsync<OperationCanceledException>(TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal static async Task RetryAsync_Cancelable(Exception exception)
    {
        await Limiter
            .Quick.RetryAsync(GetAMessage(), () => throw exception, new CancellationToken(true))
            .Assert()
            .ThrowsAsync<OperationCanceledException>(TestContext.Current.CancellationToken);

        using CancellationTokenSource tokenSource = new();

        await Limiter
            .Few.RetryAsync(
                GetAMessage(),
                () => throw exception,
                tokenSource.Cancel,
                tokenSource.Token
            )
            .Assert()
            .ThrowsAsync<OperationCanceledException>(TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal static async Task AttemptAsync_Cancelable(Exception exception)
    {
        await Limiter
            .Quick.AttemptAsync(GetAMessage(), () => throw exception, new CancellationToken(true))
            .Assert()
            .ThrowsAsync<OperationCanceledException>(TestContext.Current.CancellationToken);

        using CancellationTokenSource tokenSource = new();

        await Limiter
            .Few.AttemptAsync(
                GetAMessage(),
                () => throw exception,
                tokenSource.Cancel,
                tokenSource.Token
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
                () => data[attemptAsync++],
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
                () => data[attemptAsync++],
                () => attemptAsync == data.Count,
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
            .RetryAsync(GetAMessage(), () => data, TestContext.Current.CancellationToken)
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
                () =>
                {
                    calls++;
                    if (calls == 1)
                    {
                        throw new ArithmeticException();
                    }
                },
                TestContext.Current.CancellationToken
            )
            .Assert()
            .ThrowsNoAsync<Exception>(TestContext.Current.CancellationToken)
            .Also(() => calls)
            .Is(2);

        await new Limiter(2)
            .RetryAsync<SystemException>(
                GetAMessage(),
                () =>
                {
                    calls++;
                    if (calls == 3)
                    {
                        throw new ArithmeticException();
                    }
                },
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
            .AttemptAsync(GetAMessage(), () => data, TestContext.Current.CancellationToken)
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
                () =>
                {
                    calls++;
                    if (calls == 1)
                    {
                        throw new ArithmeticException();
                    }
                },
                TestContext.Current.CancellationToken
            )
            .Assert()
            .ThrowsNoAsync<Exception>(TestContext.Current.CancellationToken)
            .Also(() => calls)
            .Is(2);

        await new Limiter(2)
            .AttemptAsync<SystemException>(
                GetAMessage(),
                () =>
                {
                    calls++;
                    if (calls == 3)
                    {
                        throw new ArithmeticException();
                    }
                },
                TestContext.Current.CancellationToken
            )
            .Assert()
            .ThrowsNoAsync<Exception>(TestContext.Current.CancellationToken)
            .Also(() => calls)
            .Is(4);
    }

    [Theory, InlineData(1), InlineData(3)]
    internal static Task StallUntilAsync_CheckStateBehavior(int tries)
    {
        int attemptAsync = 0;
        int checkAttemptAsync = 0;

        return new Limiter(tries)
            .StallUntilAsync(
                GetAMessage(),
                () => attemptAsync++,
                () => ++checkAttemptAsync == tries,
                TestContext.Current.CancellationToken
            )
            .Assert()
            .ThrowsNoAsync<Exception>(TestContext.Current.CancellationToken)
            .Also(() => tries)
            .Is(attemptAsync)
            .And()
            .Is(checkAttemptAsync);
    }

    [Theory, InlineData(1), InlineData(3)]
    internal static Task RetryAsync_ResetStateBehavior(int tries)
    {
        Exception exception = Tools.Randomizer.Create<Exception>();
        int attemptAsync = 0;
        int resetAttemptAsync = 0;

        return new Limiter(tries)
            .RetryAsync(
                GetAMessage(),
                () =>
                {
                    if (++attemptAsync < tries)
                    {
                        throw exception;
                    }
                },
                () => resetAttemptAsync++,
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
    internal static Task RetryAsync_ReturnResetStateBehavior(int tries)
    {
        Exception exception = Tools.Randomizer.Create<Exception>();
        int attemptAsync = 0;
        int resetAttemptAsync = 0;

        int result = Tools.Randomizer.Create<int>();
        int resetBehavior()
        {
            return (++attemptAsync == tries) ? result : throw exception;
        }

        return new Limiter(tries)
            .RetryAsync(
                GetAMessage(),
                resetBehavior,
                () => resetAttemptAsync++,
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
                (Action)(() => throw exception),
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
                () => throw exception2,
                TestContext.Current.CancellationToken
            )
            .Assert()
            .ThrowsAsync<IOException>(TestContext.Current.CancellationToken)
            .That()
            .Is(exception2);
    }

    [Theory, InlineData(1), InlineData(3)]
    internal static Task AttemptAsync_ResetStateBehavior(int tries)
    {
        Exception exception = Tools.Randomizer.Create<Exception>();
        int attemptAsync = 0;
        int resetAttemptAsync = 0;

        return new Limiter(tries)
            .AttemptAsync(
                GetAMessage(),
                () =>
                {
                    if (++attemptAsync < tries)
                    {
                        throw exception;
                    }
                },
                () => resetAttemptAsync++,
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
    internal static Task AttemptAsync_ReturnResetStateBehavior(int tries)
    {
        Exception exception = Tools.Randomizer.Create<Exception>();
        int attemptAsync = 0;
        int resetAttemptAsync = 0;

        int result = Tools.Randomizer.Create<int>();
        int resetBehavior()
        {
            return (++attemptAsync == tries) ? result : throw exception;
        }

        return new Limiter(tries)
            .AttemptAsync(
                GetAMessage(),
                resetBehavior,
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
    }

    [Theory, RandomData]
    internal static async Task AttemptAsync_WrongExceptionThrows(NotSupportedException exception)
    {
        await new Limiter(3)
            .AttemptAsync<InvalidOperationException>(
                GetAMessage(),
                (Action)(() => throw exception),
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
                () => throw exception2,
                TestContext.Current.CancellationToken
            )
            .Assert()
            .ThrowsAsync<IOException>(TestContext.Current.CancellationToken)
            .That()
            .Is(exception2);
    }

    private static string GetAMessage()
    {
        return Tools.Gen.NextItem(["", null, "Test."]);
    }
}
