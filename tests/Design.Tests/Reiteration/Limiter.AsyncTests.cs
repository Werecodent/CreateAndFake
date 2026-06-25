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
            .RepeatAsync("", () => attempts++, TestContext.Current.CancellationToken)
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
                "",
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
    internal static async Task RetryAsync_TryLimited(int tries)
    {
        Exception exception = Tools.Randomizer.Create<Exception>();
        int attempts = 0;

        TimeoutException error = await new Limiter(tries)
            .RetryAsync(
                "",
                () =>
                {
                    attempts++;
                    throw exception;
                },
                TestContext.Current.CancellationToken
            )
            .Assert()
            .ThrowsAsync<TimeoutException>(TestContext.Current.CancellationToken)
            .GetCaughtException();

        error.InnerException.Assert().Is(exception).Also(attempts).Is(tries);
    }

    [Theory, InlineData(1), InlineData(3)]
    internal static Task AttemptAsync_TryLimited(int tries)
    {
        Exception exception = Tools.Randomizer.Create<Exception>();
        int attempts = 0;

        return new Limiter(tries)
            .AttemptAsync(
                "",
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
            .RepeatAsync("", () => { }, TestContext.Current.CancellationToken)
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
            .StallUntilAsync("", () => { }, () => false, TestContext.Current.CancellationToken)
            .Assert()
            .ThrowsAsync<TimeoutException>(TestContext.Current.CancellationToken)
            .Also(() => watch.Elapsed.TotalMilliseconds)
            .GreaterThanOrEqualTo(_SmallDelay.TotalMilliseconds - _WaitAccuracy);
    }

    [Theory, RandomData]
    internal static async Task RetryAsync_TimeoutLimited(Exception exception)
    {
        Stopwatch watch = Stopwatch.StartNew();

        TimeoutException error = await new Limiter(_SmallDelay)
            .RetryAsync("", () => throw exception, TestContext.Current.CancellationToken)
            .Assert()
            .ThrowsAsync<TimeoutException>(TestContext.Current.CancellationToken)
            .GetCaughtException();

        error
            .InnerException.Assert()
            .Is(exception)
            .Also(watch.Elapsed.TotalMilliseconds)
            .GreaterThanOrEqualTo(_SmallDelay.TotalMilliseconds - _WaitAccuracy);
    }

    [Theory, RandomData]
    internal static Task AttemptAsync_TimeoutLimited(Exception exception)
    {
        Stopwatch watch = Stopwatch.StartNew();

        return new Limiter(_SmallDelay)
            .AttemptAsync(
                "",
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
            .RepeatAsync("", () => attempts++, TestContext.Current.CancellationToken)
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
            .StallUntilAsync("", () => ++attempts == tries, TestContext.Current.CancellationToken)
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
                "",
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
                "",
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
        using (CancellationTokenSource tokenSource = new())
        {
            await Limiter
                .Few.RepeatAsync("", tokenSource.Cancel, tokenSource.Token)
                .Assert()
                .ThrowsAsync<OperationCanceledException>(TestContext.Current.CancellationToken);
        }
        await Limiter
            .Few.RepeatAsync("Test", () => { }, new CancellationToken(true))
            .Assert()
            .ThrowsAsync<OperationCanceledException>(TestContext.Current.CancellationToken);

        await Limiter
            .Quick.RepeatAsync("", () => { }, new CancellationToken(true))
            .Assert()
            .ThrowsAsync<OperationCanceledException>(TestContext.Current.CancellationToken);
    }

    [Fact]
    internal static async Task StallUntilAsync_Cancelable()
    {
        using (CancellationTokenSource tokenSource = new())
        {
            await Limiter
                .Few.StallUntilAsync("", tokenSource.Cancel, () => false, tokenSource.Token)
                .Assert()
                .ThrowsAsync<OperationCanceledException>(TestContext.Current.CancellationToken);
        }
        await Limiter
            .Few.StallUntilAsync("Test", () => false, new CancellationToken(true))
            .Assert()
            .ThrowsAsync<OperationCanceledException>(TestContext.Current.CancellationToken);

        await Limiter
            .Fast.StallUntilAsync("", () => false, new CancellationToken(true))
            .Assert()
            .ThrowsAsync<OperationCanceledException>(TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal static async Task RetryAsync_Cancelable(Exception exception)
    {
        using (CancellationTokenSource tokenSource = new())
        {
            await Limiter
                .Few.RetryAsync("", () => throw exception, tokenSource.Cancel, tokenSource.Token)
                .Assert()
                .ThrowsAsync<OperationCanceledException>(TestContext.Current.CancellationToken);
        }
        await Limiter
            .Few.RetryAsync("Test", () => throw exception, new CancellationToken(true))
            .Assert()
            .ThrowsAsync<OperationCanceledException>(TestContext.Current.CancellationToken);

        await Limiter
            .Quick.RetryAsync("", () => throw exception, new CancellationToken(true))
            .Assert()
            .ThrowsAsync<OperationCanceledException>(TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal static async Task AttemptAsync_Cancelable(Exception exception)
    {
        using (CancellationTokenSource tokenSource = new())
        {
            await Limiter
                .Few.AttemptAsync("", () => throw exception, tokenSource.Cancel, tokenSource.Token)
                .Assert()
                .ThrowsAsync<OperationCanceledException>(TestContext.Current.CancellationToken);
        }
        await Limiter
            .Few.AttemptAsync(null, () => throw exception, new CancellationToken(true))
            .Assert()
            .ThrowsAsync<OperationCanceledException>(TestContext.Current.CancellationToken);

        await Limiter
            .Quick.AttemptAsync("", () => throw exception, new CancellationToken(true))
            .Assert()
            .ThrowsAsync<OperationCanceledException>(TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal static async Task RepeatAsync_ResultsValid(List<int> data)
    {
        int AttemptAsync = 0;

        (
            await new Limiter(data.Count)
                .RepeatAsync("", () => data[AttemptAsync++], TestContext.Current.CancellationToken)
                .Assert()
                .HasResultAsync(TestContext.Current.CancellationToken)
                .GetResultValue()
        )
            .Assert()
            .Is(data.AsReadOnly());
    }

    [Theory, RandomData]
    internal static async Task StallUntilAsync_ResultsValid(List<int> data)
    {
        int AttemptAsync = 0;
        (
            await new Limiter(data.Count).StallUntilAsync(
                "",
                () => data[AttemptAsync++],
                () => AttemptAsync == data.Count,
                TestContext.Current.CancellationToken
            )
        )
            .Assert()
            .Is(data.AsReadOnly());
    }

    [Theory, RandomData]
    internal static async Task RetryAsync_ResultsValid(int data)
    {
        (await new Limiter(1).RetryAsync("", () => data, TestContext.Current.CancellationToken))
            .Assert()
            .Is(data);
        (await new Limiter(1).RetryAsync("", () => data, TestContext.Current.CancellationToken))
            .Assert()
            .Is(data);
    }

    [Fact]
    internal static async Task RetryAsync_Continues()
    {
        int calls = 0;

        await new Limiter(2).RetryAsync<ArithmeticException>(
            "",
            () =>
            {
                calls++;
                if (calls == 1)
                {
                    throw new ArithmeticException();
                }
            },
            TestContext.Current.CancellationToken
        );
        calls.Assert().Is(2);

        await new Limiter(2).RetryAsync<SystemException>(
            "",
            () =>
            {
                calls++;
                if (calls == 3)
                {
                    throw new ArithmeticException();
                }
            },
            TestContext.Current.CancellationToken
        );
        calls.Assert().Is(4);
    }

    [Theory, RandomData]
    internal static async Task AttemptAsync_ResultsValid(int data)
    {
        (await new Limiter(1).AttemptAsync("", () => data, TestContext.Current.CancellationToken))
            .Assert()
            .Is(data);
        (await new Limiter(1).AttemptAsync("", () => data, TestContext.Current.CancellationToken))
            .Assert()
            .Is(data);
    }

    [Fact]
    internal static async Task AttemptAsync_Continues()
    {
        int calls = 0;

        await new Limiter(2).AttemptAsync<ArithmeticException>(
            "",
            () =>
            {
                calls++;
                if (calls == 1)
                {
                    throw new ArithmeticException();
                }
            },
            TestContext.Current.CancellationToken
        );
        calls.Assert().Is(2);

        await new Limiter(2).AttemptAsync<SystemException>(
            "",
            () =>
            {
                calls++;
                if (calls == 3)
                {
                    throw new ArithmeticException();
                }
            },
            TestContext.Current.CancellationToken
        );
        calls.Assert().Is(4);
    }

    [Theory, InlineData(1), InlineData(3)]
    internal static Task StallUntilAsync_CheckStateBehavior(int tries)
    {
        int attemptAsync = 0;
        int checkAttemptAsync = 0;

        return new Limiter(tries)
            .StallUntilAsync(
                "",
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
                "",
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
    internal static async Task RetryAsync_ReturnResetStateBehavior(int tries)
    {
        Exception exception = Tools.Randomizer.Create<Exception>();
        int attemptAsync = 0;
        int resetAttemptAsync = 0;

        int result = Tools.Randomizer.Create<int>();
        int ResetBehavior()
        {
            return (++attemptAsync == tries) ? result : throw exception;
        }

        (
            await new Limiter(tries)
                .RetryAsync(
                    "",
                    ResetBehavior,
                    () => resetAttemptAsync++,
                    TestContext.Current.CancellationToken
                )
                .Assert()
                .HasResultAsync(TestContext.Current.CancellationToken)
                .GetResultValue()
        )
            .Assert()
            .Is(result)
            .Also(attemptAsync)
            .Is(tries)
            .Also(resetAttemptAsync)
            .Is(tries - 1);
    }

    [Theory, RandomData]
    internal static async Task RetryAsync_WrongExceptionThrows(NotSupportedException exception)
    {
        await new Limiter(3)
            .RetryAsync<InvalidOperationException>(
                "",
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
                "",
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
                "",
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
    internal static async Task AttemptAsync_ReturnResetStateBehavior(int tries)
    {
        Exception exception = Tools.Randomizer.Create<Exception>();
        int AttemptAsync = 0;
        int resetAttemptAsync = 0;

        int result = Tools.Randomizer.Create<int>();
        int ResetBehavior()
        {
            return (++AttemptAsync == tries) ? result : throw exception;
        }

        (
            await new Limiter(tries).AttemptAsync(
                "",
                ResetBehavior,
                () => resetAttemptAsync++,
                TestContext.Current.CancellationToken
            )
        )
            .Assert()
            .Is(result)
            .Also(AttemptAsync)
            .Is(tries)
            .Also(resetAttemptAsync)
            .Is(tries - 1);
    }

    [Theory, RandomData]
    internal static async Task AttemptAsync_WrongExceptionThrows(NotSupportedException exception)
    {
        await new Limiter(3)
            .AttemptAsync<InvalidOperationException>(
                null,
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
                "",
                () => throw exception2,
                TestContext.Current.CancellationToken
            )
            .Assert()
            .ThrowsAsync<IOException>(TestContext.Current.CancellationToken)
            .That()
            .Is(exception2);
    }
}
