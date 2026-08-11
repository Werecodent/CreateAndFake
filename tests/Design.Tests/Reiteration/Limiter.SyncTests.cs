using System.Diagnostics;
using Werecodent.CreateAndFake.Design.Reiteration;

namespace Werecodent.CreateAndFake.Design.Tests.Reiteration;

public static class LimiterSyncTests
{
    [Fact]
    internal static void Limiter_SyncTimeoutWorks()
    {
        // All sync timeout tests reduced to one test in order to reduce any risk of deadlock.
        // Async methods should be preferred for timeout limits.

        TimeSpan timeout = new(0, 0, 0, 0, 65);
        TimeSpan delay = new(0, 0, 0, 0, 35);
        Limiter testInstance = new(timeout, delay);

        int attempts = 0;
        Stopwatch watch = Stopwatch.StartNew();

        testInstance
            .Repeat("Timeout test.", () => attempts++, TestContext.Current.CancellationToken)
            .Assert()
            .HasCount(2)
            .Also(watch.Elapsed.TotalMilliseconds)
            .GreaterThanOrEqualTo(20d);

        attempts = 0;
        watch.Restart();

        testInstance
            .Assert(x =>
                x.StallUntil(
                    "Timeout test.",
                    () => attempts++,
                    () => false,
                    TestContext.Current.CancellationToken
                )
            )
            .Throws<TimeoutException>()
            .Also(attempts)
            .Is(2)
            .Also(watch.Elapsed.TotalMilliseconds)
            .GreaterThanOrEqualTo(20d);
    }

    [Theory, InlineData(1), InlineData(3)]
    internal static void Repeat_TryLimited(int tries)
    {
        int attempts = 0;

        new Limiter(tries)
            .Repeat(GetAMessage(), () => attempts++, TestContext.Current.CancellationToken)
            .Assert()
            .HasCount(tries)
            .Also(attempts)
            .Is(tries);
    }

    [Theory, InlineData(1), InlineData(3)]
    internal static void StallUntil_TryLimited(int tries)
    {
        int attempts = 0;

        new Limiter(tries)
            .Assert(x =>
                x.StallUntil(
                    GetAMessage(),
                    () => attempts++,
                    () => false,
                    TestContext.Current.CancellationToken
                )
            )
            .Throws<TimeoutException>()
            .Also(attempts)
            .Is(tries);
    }

    [Theory, InlineData(1), InlineData(3)]
    internal static void Retry_TryLimited(int tries)
    {
        Exception exception = Tools.Randomizer.Create<Exception>();
        int attempts = 0;

        new Limiter(tries)
            .Assert(x =>
                x.Retry(
                    GetAMessage(),
                    () =>
                    {
                        attempts++;
                        throw exception;
                    },
                    TestContext.Current.CancellationToken
                )
            )
            .Throws<TimeoutException>()
            .With.InnerException.Assert()
            .Is(exception)
            .Also(attempts)
            .Is(tries);
    }

    [Theory, InlineData(1), InlineData(3)]
    internal static void Attempt_TryLimited(int tries)
    {
        Exception exception = Tools.Randomizer.Create<Exception>();
        int attempts = 0;

        new Limiter(tries)
            .Assert(x =>
                x.Attempt(
                    GetAMessage(),
                    () =>
                    {
                        attempts++;
                        throw exception;
                    },
                    TestContext.Current.CancellationToken
                )
            )
            .ThrowsNo<Exception>()
            .Also(attempts)
            .Is(tries);
    }

    [Fact]
    internal static void Repeat_Cancelable()
    {
        Limiter
            .Few.Assert(x => x.Repeat(GetAMessage(), () => { }, new CancellationToken(true)))
            .Throws<OperationCanceledException>();

        using CancellationTokenSource tokenSource = new();

        Limiter
            .Few.Assert(x => x.Repeat(GetAMessage(), () => tokenSource.Cancel(), tokenSource.Token))
            .Throws<OperationCanceledException>();
    }

    [Fact]
    internal static void StallUntil_Cancelable()
    {
        Limiter
            .Few.Assert(x => x.StallUntil(GetAMessage(), () => false, new CancellationToken(true)))
            .Throws<OperationCanceledException>();

        using CancellationTokenSource tokenSource = new();

        Limiter
            .Few.Assert(x =>
                x.StallUntil(
                    GetAMessage(),
                    () => tokenSource.Cancel(),
                    () => false,
                    tokenSource.Token
                )
            )
            .Throws<OperationCanceledException>();
    }

    [Theory, RandomData]
    internal static void Retry_Cancelable(Exception exception)
    {
        Limiter
            .Few.Assert(x =>
                x.Retry(GetAMessage(), () => throw exception, new CancellationToken(true))
            )
            .Throws<OperationCanceledException>();

        using CancellationTokenSource tokenSource = new();

        Limiter
            .Few.Assert(x =>
                x.Retry(
                    GetAMessage(),
                    () => throw exception,
                    () => tokenSource.Cancel(),
                    tokenSource.Token
                )
            )
            .Throws<OperationCanceledException>();
    }

    [Theory, RandomData]
    internal static void Attempt_Cancelable(Exception exception)
    {
        Limiter
            .Few.Assert(x => x.Attempt(null, () => throw exception, new CancellationToken(true)))
            .Throws<OperationCanceledException>();

        using CancellationTokenSource tokenSource = new();

        Limiter
            .Few.Assert(x =>
                x.Attempt(
                    GetAMessage(),
                    () => throw exception,
                    () => tokenSource.Cancel(),
                    tokenSource.Token
                )
            )
            .Throws<OperationCanceledException>();
    }

    [Theory, RandomData]
    internal static void Repeat_ResultsValid(List<int> data)
    {
        int attempt = 0;

        new Limiter(data.Count)
            .Repeat(GetAMessage(), () => data[attempt++], TestContext.Current.CancellationToken)
            .Assert()
            .Is(data.AsReadOnly());
    }

    [Theory, RandomData]
    internal static void StallUntil_ResultsValid(List<int> data)
    {
        int attempt = 0;

        new Limiter(data.Count)
            .StallUntil(
                GetAMessage(),
                () => data[attempt++],
                () => attempt == data.Count,
                TestContext.Current.CancellationToken
            )
            .Assert()
            .Is(data.AsReadOnly());
    }

    [Theory, RandomData]
    internal static void Retry_ResultsValid(int data)
    {
        new Limiter(1)
            .Retry(GetAMessage(), () => data, TestContext.Current.CancellationToken)
            .Assert()
            .Is(data);
    }

    [Fact]
    internal static void Retry_Continues()
    {
        int calls = 0;

        new Limiter(2)
            .Assert(x =>
                x.Retry<ArithmeticException>(
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
            )
            .ThrowsNo<Exception>()
            .Also(calls)
            .Is(2);

        new Limiter(2)
            .Assert(x =>
                x.Retry<SystemException>(
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
            )
            .ThrowsNo<Exception>()
            .Also(calls)
            .Is(4);
    }

    [Theory, RandomData]
    internal static void Attempt_ResultsValid(int data)
    {
        new Limiter(1)
            .Attempt(GetAMessage(), () => data, TestContext.Current.CancellationToken)
            .Assert()
            .Is(data);
    }

    [Fact]
    internal static void Attempt_Continues()
    {
        int calls = 0;

        new Limiter(2)
            .Assert(x =>
                x.Attempt<ArithmeticException>(
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
            )
            .ThrowsNo<Exception>()
            .Also(calls)
            .Is(2);

        new Limiter(2)
            .Assert(x =>
                x.Attempt<SystemException>(
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
            )
            .ThrowsNo<Exception>()
            .Also(calls)
            .Is(4);
    }

    [Theory, InlineData(1), InlineData(3)]
    internal static void StallUntil_CheckStateBehavior(int tries)
    {
        int attempt = 0;
        int checkAttempt = 0;

        new Limiter(tries)
            .StallUntil(
                GetAMessage(),
                () => attempt++,
                () => ++checkAttempt == tries,
                TestContext.Current.CancellationToken
            )
            .Assert()
            .HasCount(tries)
            .Also(checkAttempt)
            .Is(attempt)
            .And.Is(tries);
    }

    [Theory, InlineData(1), InlineData(3)]
    internal static void Retry_ResetStateBehavior(int tries)
    {
        Exception exception = Tools.Randomizer.Create<Exception>();
        int attempt = 0;
        int resetAttempt = 0;

        new Limiter(tries)
            .Assert(x =>
                x.Retry(
                    GetAMessage(),
                    () =>
                    {
                        if (++attempt < tries)
                        {
                            throw exception;
                        }
                    },
                    () => resetAttempt++,
                    TestContext.Current.CancellationToken
                )
            )
            .ThrowsNo<Exception>()
            .Also(attempt)
            .Is(tries)
            .Also(resetAttempt)
            .Is(tries - 1);
    }

    [Theory, InlineData(1), InlineData(3)]
    internal static void Retry_ReturnResetStateBehavior(int tries)
    {
        Exception exception = Tools.Randomizer.Create<Exception>();
        int attempt = 0;
        int resetAttempt = 0;

        int result = Tools.Randomizer.Create<int>();
        int resetBehavior()
        {
            return (++attempt == tries) ? result : throw exception;
        }

        new Limiter(tries)
            .Retry(
                GetAMessage(),
                resetBehavior,
                () => resetAttempt++,
                TestContext.Current.CancellationToken
            )
            .Assert()
            .Is(result)
            .Also(attempt)
            .Is(tries)
            .Also(resetAttempt)
            .Is(tries - 1);
    }

    [Theory, RandomData]
    internal static void Retry_WrongExceptionThrows(NotSupportedException exception)
    {
        new Limiter(3)
            .Assert(x =>
                x.Retry<InvalidOperationException>(
                    GetAMessage(),
                    (Action)(() => throw exception),
                    TestContext.Current.CancellationToken
                )
            )
            .Throws<NotSupportedException>()
            .That.Is(exception);

        IOException exception2 = new();
        new Limiter(3)
            .Assert(x =>
                x.Retry<DirectoryNotFoundException, bool>(
                    GetAMessage(),
                    () => throw exception2,
                    TestContext.Current.CancellationToken
                )
            )
            .Throws<IOException>()
            .That.Is(exception2);
    }

    [Theory, InlineData(1), InlineData(3)]
    internal static void Attempt_ResetStateBehavior(int tries)
    {
        Exception exception = Tools.Randomizer.Create<Exception>();
        int attempt = 0;
        int resetAttempt = 0;

        new Limiter(tries)
            .Assert(x =>
                x.Attempt(
                    GetAMessage(),
                    () =>
                    {
                        if (++attempt < tries)
                        {
                            throw exception;
                        }
                    },
                    () => resetAttempt++,
                    TestContext.Current.CancellationToken
                )
            )
            .ThrowsNo<Exception>()
            .Also(attempt)
            .Is(tries)
            .Also(resetAttempt)
            .Is(tries - 1);
    }

    [Theory, InlineData(1), InlineData(3)]
    internal static void Attempt_ReturnResetStateBehavior(int tries)
    {
        Exception exception = Tools.Randomizer.Create<Exception>();
        int attempt = 0;
        int resetAttempt = 0;

        int result = Tools.Randomizer.Create<int>();
        int resetBehavior()
        {
            return (++attempt == tries) ? result : throw exception;
        }

        new Limiter(tries)
            .Attempt(
                GetAMessage(),
                resetBehavior,
                () => resetAttempt++,
                TestContext.Current.CancellationToken
            )
            .Assert()
            .Is(result)
            .Also(attempt)
            .Is(tries)
            .Also(resetAttempt)
            .Is(tries - 1);
    }

    [Theory, RandomData]
    internal static void Attempt_WrongExceptionThrows(NotSupportedException exception)
    {
        Limiter
            .Once.Assert(x =>
                x.Attempt<InvalidOperationException>(
                    null,
                    (Action)(() => throw exception),
                    TestContext.Current.CancellationToken
                )
            )
            .Throws<NotSupportedException>()
            .That.Is(exception);

        IOException exception2 = new();
        Limiter
            .Once.Assert(x =>
                x.Attempt<DirectoryNotFoundException, bool>(
                    "Wrong subclass exception test.",
                    () => throw exception2,
                    TestContext.Current.CancellationToken
                )
            )
            .Throws<IOException>()
            .That.Is(exception2);
    }

    private static string GetAMessage()
    {
        return Tools.Gen.NextItem(["", null, "Test."]);
    }
}
