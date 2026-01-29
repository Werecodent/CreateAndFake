using System.Diagnostics;
using CreateAndFake.Design.Reiteration;

namespace CreateAndFake.Design.Tests.Reiteration;

public static class LimiterSyncTests
{
    [Fact]
    internal static void DelayOrFault_SyncTimeoutWorks()
    {
        // All sync timeout tests reduced to one test in order to reduce any risk of deadlock.
        // Async methods should be preferred for timeout limits.

        TimeSpan timeout = new(0, 0, 0, 0, 40);
        TimeSpan delay = new(0, 0, 0, 0, 25);
        Limiter testInstance = new(timeout, delay);

        int attempts = 0;
        Stopwatch watch = Stopwatch.StartNew();
        testInstance.Repeat(
            "Timeout test.",
            () => attempts++,
            TestContext.Current.CancellationToken
        );

        attempts.Assert().Is(2);
        watch.Elapsed.TotalMilliseconds.Assert().GreaterThanOrEqualTo(20d);

        attempts = 0;
        watch.Restart();

        testInstance
            .Assert(l =>
                l.StallUntil(
                    "",
                    () => attempts++,
                    () => false,
                    TestContext.Current.CancellationToken
                )
            )
            .Throws<TimeoutException>();

        attempts.Assert().Is(2);
        watch.Elapsed.TotalMilliseconds.Assert().GreaterThanOrEqualTo(20d);
    }

    [Theory, InlineData(1), InlineData(3)]
    internal static void Repeat_TryLimited(int tries)
    {
        int attempts = 0;
        new Limiter(tries).Repeat("", () => attempts++, TestContext.Current.CancellationToken);
        attempts.Assert().Is(tries);
    }

    [Theory, InlineData(1), InlineData(3)]
    internal static void StallUntil_TryLimited(int tries)
    {
        int attempts = 0;

        new Limiter(tries)
            .Assert(l =>
                l.StallUntil(
                    "",
                    () => attempts++,
                    () => false,
                    TestContext.Current.CancellationToken
                )
            )
            .Throws<TimeoutException>();
        attempts.Assert().Is(tries);
    }

    [Theory, InlineData(1), InlineData(3)]
    internal static void Retry_TryLimited(int tries)
    {
        Exception exception = Tools.Randomizer.Create<Exception>();
        int attempts = 0;

        new Limiter(tries)
            .Assert(l =>
                l.Retry(
                    "",
                    () =>
                    {
                        attempts++;
                        throw exception;
                    },
                    TestContext.Current.CancellationToken
                )
            )
            .Throws<TimeoutException>()
            .InnerException.Assert()
            .Is(exception)
            .Also(attempts)
            .Is(tries);
    }

    [Theory, InlineData(1), InlineData(3)]
    internal static void Attempt_TryLimited(int tries)
    {
        Exception exception = Tools.Randomizer.Create<Exception>();
        int attempts = 0;

        new Limiter(tries).Attempt(
            "",
            () =>
            {
                attempts++;
                throw exception;
            },
            TestContext.Current.CancellationToken
        );
        attempts.Assert().Is(tries);
    }

    [Fact]
    internal static void Repeat_Cancelable()
    {
        using (CancellationTokenSource tokenSource = new())
        {
            Limiter
                .Few.Assert(l => l.Repeat("", () => tokenSource.Cancel(), tokenSource.Token))
                .Throws<OperationCanceledException>();
        }
        Limiter
            .Few.Assert(l => l.Repeat("Test", () => { }, new CancellationToken(true)))
            .Throws<OperationCanceledException>();
    }

    [Fact]
    internal static void StallUntil_Cancelable()
    {
        using (CancellationTokenSource tokenSource = new())
        {
            Limiter
                .Few.Assert(l =>
                    l.StallUntil("", () => tokenSource.Cancel(), () => false, tokenSource.Token)
                )
                .Throws<OperationCanceledException>();
        }
        Limiter
            .Few.Assert(l => l.StallUntil("Test", () => false, new CancellationToken(true)))
            .Throws<OperationCanceledException>();
    }

    [Theory, RandomData]
    internal static void Retry_Cancelable(Exception exception)
    {
        using (CancellationTokenSource tokenSource = new())
        {
            Limiter
                .Few.Assert(l =>
                    l.Retry(
                        "",
                        () => throw exception,
                        () => tokenSource.Cancel(),
                        tokenSource.Token
                    )
                )
                .Throws<OperationCanceledException>();
        }
        Limiter
            .Few.Assert(l => l.Retry("Test", () => throw exception, new CancellationToken(true)))
            .Throws<OperationCanceledException>();
    }

    [Theory, RandomData]
    internal static void Attempt_Cancelable(Exception exception)
    {
        using (CancellationTokenSource tokenSource = new())
        {
            Limiter
                .Few.Assert(l =>
                    l.Attempt(
                        "",
                        () => throw exception,
                        () => tokenSource.Cancel(),
                        tokenSource.Token
                    )
                )
                .Throws<OperationCanceledException>();
        }
        Limiter
            .Few.Assert(l => l.Attempt(null, () => throw exception, new CancellationToken(true)))
            .Throws<OperationCanceledException>();
    }

    [Theory, RandomData]
    internal static void Repeat_ResultsValid(List<int> data)
    {
        int attempt = 0;
        new Limiter(data.Count)
            .Repeat("", () => data[attempt++], TestContext.Current.CancellationToken)
            .Assert()
            .Is(data.AsReadOnly());
    }

    [Theory, RandomData]
    internal static void StallUntil_ResultsValid(List<int> data)
    {
        int attempt = 0;
        new Limiter(data.Count)
            .StallUntil(
                "",
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
            .Retry("", () => data, TestContext.Current.CancellationToken)
            .Assert()
            .Is(data);
        new Limiter(1)
            .Retry("", () => data, TestContext.Current.CancellationToken)
            .Assert()
            .Is(data);
    }

    [Fact]
    internal static void Retry_Continues()
    {
        int calls = 0;

        new Limiter(2).Retry<ArithmeticException>(
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

        new Limiter(2).Retry<SystemException>(
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
    internal static void Attempt_ResultsValid(int data)
    {
        new Limiter(1)
            .Attempt("", () => data, TestContext.Current.CancellationToken)
            .Assert()
            .Is(data);
        new Limiter(1)
            .Attempt("", () => data, TestContext.Current.CancellationToken)
            .Assert()
            .Is(data);
    }

    [Fact]
    internal static void Attempt_Continues()
    {
        int calls = 0;

        new Limiter(2).Attempt<ArithmeticException>(
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

        new Limiter(2).Attempt<SystemException>(
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
    internal static void StallUntil_CheckStateBehavior(int tries)
    {
        int attempt = 0;
        int checkAttempt = 0;

        new Limiter(tries).StallUntil(
            "",
            () => attempt++,
            () => ++checkAttempt == tries,
            TestContext.Current.CancellationToken
        );
        tries.Assert().Is(attempt).And.Is(checkAttempt);
    }

    [Theory, InlineData(1), InlineData(3)]
    internal static void Retry_ResetStateBehavior(int tries)
    {
        Exception exception = Tools.Randomizer.Create<Exception>();
        int attempt = 0;
        int resetAttempt = 0;

        new Limiter(tries).Retry(
            "",
            () =>
            {
                if (++attempt < tries)
                {
                    throw exception;
                }
            },
            () => resetAttempt++,
            TestContext.Current.CancellationToken
        );

        attempt.Assert().Is(tries).Also(resetAttempt).Is(tries - 1);
    }

    [Theory, InlineData(1), InlineData(3)]
    internal static void Retry_ReturnResetStateBehavior(int tries)
    {
        Exception exception = Tools.Randomizer.Create<Exception>();
        int attempt = 0;
        int resetAttempt = 0;

        int result = Tools.Randomizer.Create<int>();
        int ResetBehavior()
        {
            return (++attempt == tries) ? result : throw exception;
        }

        new Limiter(tries)
            .Retry("", ResetBehavior, () => resetAttempt++, TestContext.Current.CancellationToken)
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
            .Assert(l =>
                l.Retry<InvalidOperationException>(
                    "",
                    (Action)(() => throw exception),
                    TestContext.Current.CancellationToken
                )
            )
            .Throws<NotSupportedException>()
            .Assert()
            .Is(exception);

        IOException exception2 = new();

        new Limiter(3)
            .Assert(l =>
                l.Retry<DirectoryNotFoundException, bool>(
                    "",
                    () => throw exception2,
                    TestContext.Current.CancellationToken
                )
            )
            .Throws<IOException>()
            .Assert()
            .Is(exception2);
    }

    [Theory, InlineData(1), InlineData(3)]
    internal static void Attempt_ResetStateBehavior(int tries)
    {
        Exception exception = Tools.Randomizer.Create<Exception>();
        int attempt = 0;
        int resetAttempt = 0;

        new Limiter(tries).Attempt(
            "",
            () =>
            {
                if (++attempt < tries)
                {
                    throw exception;
                }
            },
            () => resetAttempt++,
            TestContext.Current.CancellationToken
        );

        attempt.Assert().Is(tries).Also(resetAttempt).Is(tries - 1);
    }

    [Theory, InlineData(1), InlineData(3)]
    internal static void Attempt_ReturnResetStateBehavior(int tries)
    {
        Exception exception = Tools.Randomizer.Create<Exception>();
        int attempt = 0;
        int resetAttempt = 0;

        int result = Tools.Randomizer.Create<int>();
        int ResetBehavior()
        {
            return (++attempt == tries) ? result : throw exception;
        }

        new Limiter(tries)
            .Attempt("", ResetBehavior, () => resetAttempt++, TestContext.Current.CancellationToken)
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
            .Once.Assert(l =>
                l.Attempt<InvalidOperationException>(
                    null,
                    (Action)(() => throw exception),
                    TestContext.Current.CancellationToken
                )
            )
            .Throws<NotSupportedException>()
            .Assert()
            .Is(exception);

        IOException exception2 = new();
        Limiter
            .Once.Assert(l =>
                l.Attempt<DirectoryNotFoundException, bool>(
                    "Wrong subclass exception test.",
                    () => throw exception2,
                    TestContext.Current.CancellationToken
                )
            )
            .Throws<IOException>()
            .Assert()
            .Is(exception2);
    }
}
