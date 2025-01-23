using System.Diagnostics;
using System.Reflection;
using CreateAndFake.Design;

namespace CreateAndFake.DesignTests;

#pragma warning disable xUnit1031 // Test methods should not use blocking code: Ensures blocking code works for library.

public static class AsyncAsyncLimiterTests
{
    private const int _WaitAccuracy = 5;

    private static readonly TimeSpan _SmallDelay = new(0, 0, 0, 0, 20);

    [Fact]
    internal static void AsyncLimiter_GuardsNulls()
    {
        Tools.Tester.PreventsNullRefException(AsyncLimiter.Few);
    }

    [Fact]
    internal static async Task Repeat_AtLeastOnce()
    {
        int attempts = 0;

        await new AsyncLimiter(0).Repeat(null, () => attempts++);
        attempts.Assert().Is(1);

        await new AsyncLimiter(TimeSpan.MinValue).Repeat("Message", () => attempts++);
        attempts.Assert().Is(2);
    }

    [Fact]
    internal static void StallUntil_AtLeastOnce()
    {
        int attempts = 0;

        new AsyncLimiter(0)
            .Assert(l => l.StallUntil("", () => attempts++, () => false).Wait())
            .Throws<TimeoutException>();
        attempts.Assert().Is(1);

        new AsyncLimiter(TimeSpan.MinValue)
            .Assert(l => l.StallUntil("", () => attempts++, () => false).Wait())
            .Throws<TimeoutException>();
        attempts.Assert().Is(2);
    }

    [Theory, RandomData]
    internal static void Retry_AtLeastOnce(Exception exception)
    {
        int attempts = 0;

        new AsyncLimiter(0)
            .Assert(l => l.Retry("", () => { attempts++; throw exception; }).Wait())
            .Throws<TimeoutException>().InnerException.Assert().Is(exception)
            .Also(attempts).Is(1);

        new AsyncLimiter(TimeSpan.MinValue).Assert(l => l.Retry("", () => { attempts++; throw exception; }).Wait())
            .Throws<TimeoutException>().InnerException.Assert().Is(exception)
            .Also(attempts).Is(2);
    }

    [Theory, RandomData]
    internal static async Task Attempt_AtLeastOnce(Exception exception)
    {
        int attempts = 0;

        await new AsyncLimiter(0).Attempt("", () => { attempts++; throw exception; });
        attempts.Assert().Is(1);

        await new AsyncLimiter(TimeSpan.MinValue).Attempt("", () => { attempts++; throw exception; });
        attempts.Assert().Is(2);
    }

    [Theory,
        InlineData(1),
        InlineData(3)]
    internal static async Task Repeat_TryLimited(int tries)
    {
        int attempts = 0;
        await new AsyncLimiter(tries).Repeat("", () => attempts++);
        attempts.Assert().Is(tries);
    }

    [Theory,
        InlineData(1),
        InlineData(3)]
    internal static void StallUntil_TryLimited(int tries)
    {
        int attempts = 0;

        new AsyncLimiter(tries)
            .Assert(l => l.StallUntil("", () => attempts++, () => false).Wait())
            .Throws<TimeoutException>();
        attempts.Assert().Is(tries);
    }

    [Theory,
        InlineData(1),
        InlineData(3)]
    internal static void Retry_TryLimited(int tries)
    {
        Exception exception = Tools.Randomizer.Create<Exception>();
        int attempts = 0;

        new AsyncLimiter(tries)
            .Assert(l => l.Retry("", () => { attempts++; throw exception; }).Wait())
            .Throws<TimeoutException>().InnerException.Assert().Is(exception)
            .Also(attempts).Is(tries);
    }

    [Theory,
        InlineData(1),
        InlineData(3)]
    internal static void Attempt_TryLimited(int tries)
    {
        Exception exception = Tools.Randomizer.Create<Exception>();
        int attempts = 0;

        new AsyncLimiter(tries)
            .Attempt("", () => { attempts++; throw exception; })
            .Wait(TestContext.Current.CancellationToken);
        attempts.Assert().Is(tries);
    }

    [Fact]
    internal static async Task Repeat_TimeoutLimited()
    {
        Stopwatch watch = Stopwatch.StartNew();
        await new AsyncLimiter(_SmallDelay).Repeat("", () => { });
        watch.Elapsed.TotalMilliseconds.Assert().GreaterThanOrEqualTo(_SmallDelay.TotalMilliseconds - _WaitAccuracy);
    }

    [Fact]
    internal static void StallUntil_TimeoutLimited()
    {
        Stopwatch watch = Stopwatch.StartNew();

        new AsyncLimiter(_SmallDelay)
            .Assert(l => l.StallUntil("", () => { }, () => false).Wait())
            .Throws<TimeoutException>();

        watch.Elapsed.TotalMilliseconds.Assert().GreaterThanOrEqualTo(_SmallDelay.TotalMilliseconds - _WaitAccuracy);
    }

    [Theory, RandomData]
    internal static void Retry_TimeoutLimited(Exception exception)
    {
        Stopwatch watch = Stopwatch.StartNew();

        new AsyncLimiter(_SmallDelay)
            .Assert(l => l.Retry("", () => { throw exception; }).Wait())
            .Throws<TimeoutException>().InnerException
            .Assert()
            .Is(exception)
            .Also(watch.Elapsed.TotalMilliseconds)
            .GreaterThanOrEqualTo(_SmallDelay.TotalMilliseconds - _WaitAccuracy);
    }

    [Theory, RandomData]
    internal static void Attempt_TimeoutLimited(Exception exception)
    {
        Stopwatch watch = Stopwatch.StartNew();

        new AsyncLimiter(_SmallDelay)
            .Attempt("", () => watch.IsRunning ? throw exception : new object()).Result
            .Assert()
            .Is(null)
            .Also(watch.Elapsed.TotalMilliseconds)
            .GreaterThanOrEqualTo(_SmallDelay.TotalMilliseconds - _WaitAccuracy);
    }

    [Theory,
        InlineData(1),
        InlineData(2),
        InlineData(3)]
    internal static async Task Repeat_DelayOccurs(int tries)
    {
        Stopwatch watch = Stopwatch.StartNew();
        await new AsyncLimiter(tries, _SmallDelay).Repeat("", () => { });

        watch.Elapsed.TotalMilliseconds.Assert().GreaterThanOrEqualTo(
            (_SmallDelay.TotalMilliseconds - _WaitAccuracy) * (tries - 1));
    }

    [Theory,
        InlineData(1),
        InlineData(2),
        InlineData(3)]
    internal static async Task StallUntil_DelayOccurs(int tries)
    {
        int attempts = 0;

        Stopwatch watch = Stopwatch.StartNew();
        await new AsyncLimiter(tries, _SmallDelay).StallUntil("", () => ++attempts == tries);

        watch.Elapsed.TotalMilliseconds.Assert().GreaterThanOrEqualTo(
            (_SmallDelay.TotalMilliseconds - _WaitAccuracy) * (tries - 1));
    }

    [Theory,
        InlineData(1),
        InlineData(2),
        InlineData(3)]
    internal static async Task Retry_DelayOccurs(int tries)
    {
        Exception exception = Tools.Randomizer.Create<Exception>();
        int attempts = 0;

        Stopwatch watch = Stopwatch.StartNew();
        await new AsyncLimiter(tries, _SmallDelay)
            .Retry("", () =>
            {
                if (++attempts != tries) { throw exception; }
            });

        watch.Elapsed.TotalMilliseconds.Assert().GreaterThanOrEqualTo(
            (_SmallDelay.TotalMilliseconds - _WaitAccuracy) * (tries - 1));
    }

    [Theory,
        InlineData(1),
        InlineData(2),
        InlineData(3)]
    internal static async Task Attempt_DelayOccurs(int tries)
    {
        Exception exception = Tools.Randomizer.Create<Exception>();
        int attempts = 0;

        Stopwatch watch = Stopwatch.StartNew();
        await new AsyncLimiter(tries, _SmallDelay)
            .Attempt("", () =>
            {
                if (++attempts != tries) { throw exception; }
            });

        watch.Elapsed.TotalMilliseconds.Assert().GreaterThanOrEqualTo(
            (_SmallDelay.TotalMilliseconds - _WaitAccuracy) * (tries - 1));
    }

    [Fact]
    internal static void Repeat_Cancelable()
    {
        using (CancellationTokenSource tokenSource = new())
        {
            AsyncLimiter.Few
                .Assert(l => l.Repeat("", () => tokenSource.Cancel(), tokenSource.Token).Wait())
                .Throws<TaskCanceledException>();
        }
        AsyncLimiter.Few
            .Assert(l => l.Repeat("Test", () => { }, new CancellationToken(true)).Wait())
            .Throws<TaskCanceledException>();

        AsyncLimiter.Quick
            .Assert(l => l.Repeat("", () => { }, new CancellationToken(true)).Wait())
            .Throws<TaskCanceledException>();
    }

    [Fact]
    internal static void StallUntil_Cancelable()
    {
        using (CancellationTokenSource tokenSource = new())
        {
            AsyncLimiter.Few
                .Assert(l => l.StallUntil("", () => tokenSource.Cancel(), () => false, tokenSource.Token).Wait())
                .Throws<TaskCanceledException>();
        }
        AsyncLimiter.Few
            .Assert(l => l.StallUntil("Test", () => false, new CancellationToken(true)).Wait())
            .Throws<TaskCanceledException>();

        AsyncLimiter.Quick
            .Assert(l => l.StallUntil("", () => false, new CancellationToken(true)).Wait())
            .Throws<TaskCanceledException>();
    }

    [Theory, RandomData]
    internal static void Retry_Cancelable(Exception exception)
    {
        using (CancellationTokenSource tokenSource = new())
        {
            AsyncLimiter.Few
                .Assert(l => l.Retry("", () => throw exception, () => tokenSource.Cancel(), tokenSource.Token).Wait())
                .Throws<TaskCanceledException>();
        }
        AsyncLimiter.Few
            .Assert(l => l.Retry("Test", () => throw exception, new CancellationToken(true)).Wait())
            .Throws<TaskCanceledException>();

        AsyncLimiter.Quick
            .Assert(l => l.Retry("", () => throw exception, new CancellationToken(true)).Wait())
            .Throws<TaskCanceledException>();
    }

    [Theory, RandomData]
    internal static void Attempt_Cancelable(Exception exception)
    {
        using (CancellationTokenSource tokenSource = new())
        {
            AsyncLimiter.Few
                .Assert(l => l.Attempt("", () => throw exception, () => tokenSource.Cancel(), tokenSource.Token).Wait())
                .Throws<TaskCanceledException>();
        }
        AsyncLimiter.Few
            .Assert(l => l.Attempt(null, () => throw exception, new CancellationToken(true)).Wait())
            .Throws<TaskCanceledException>();

        AsyncLimiter.Quick
            .Assert(l => l.Attempt("", () => throw exception, new CancellationToken(true)).Wait())
            .Throws<TaskCanceledException>();
    }

    [Theory, RandomData]
    internal static async Task Repeat_ResultsValid(List<int> data)
    {
        int attempt = 0;
        (await new AsyncLimiter(data.Count).Repeat("", () => data[attempt++])).Assert().Is(data.AsReadOnly());
    }

    [Theory, RandomData]
    internal static async Task StallUntil_ResultsValid(List<int> data)
    {
        int attempt = 0;
        (await new AsyncLimiter(data.Count)
            .StallUntil("", () => data[attempt++], () => attempt == data.Count))
            .Assert()
            .Is(data.AsReadOnly());
    }

    [Theory, RandomData]
    internal static async Task Retry_ResultsValid(int data)
    {
        (await new AsyncLimiter(1).Retry("", () => data)).Assert().Is(data);
        (await new AsyncLimiter(1).Retry("", () => data)).Assert().Is(data);
    }

    [Fact]
    internal static async Task Retry_Continues()
    {
        int calls = 0;

        await new AsyncLimiter(2).Retry<ArithmeticException>("", () =>
        {
            calls++;
            if (calls == 1)
            {
                throw new ArithmeticException();
            }
        });
        calls.Assert().Is(2);

        await new AsyncLimiter(2).Retry<SystemException>("", () =>
        {
            calls++;
            if (calls == 3)
            {
                throw new ArithmeticException();
            }
        });
        calls.Assert().Is(4);
    }

    [Theory, RandomData]
    internal static async Task Attempt_ResultsValid(int data)
    {
        (await new AsyncLimiter(1).Attempt("", () => data)).Assert().Is(data);
        (await new AsyncLimiter(1).Attempt("", () => data)).Assert().Is(data);
    }

    [Fact]
    internal static async Task Attempt_Continues()
    {
        int calls = 0;

        await new AsyncLimiter(2).Attempt<ArithmeticException>("", () =>
        {
            calls++;
            if (calls == 1)
            {
                throw new ArithmeticException();
            }
        });
        calls.Assert().Is(2);

        await new AsyncLimiter(2).Attempt<SystemException>("", () =>
        {
            calls++;
            if (calls == 3)
            {
                throw new ArithmeticException();
            }
        });
        calls.Assert().Is(4);
    }

    [Theory,
        InlineData(1),
        InlineData(3)]
    internal static async Task StallUntil_CheckStateBehavior(int tries)
    {
        int attempt = 0;
        int checkAttempt = 0;

        await new AsyncLimiter(tries).StallUntil("", () => attempt++, () => ++checkAttempt == tries);
        tries.Assert().Is(attempt).And.Is(checkAttempt);
    }

    [Theory,
        InlineData(1),
        InlineData(3)]
    internal static async Task Retry_ResetStateBehavior(int tries)
    {
        Exception exception = Tools.Randomizer.Create<Exception>();
        int attempt = 0;
        int resetAttempt = 0;

        await new AsyncLimiter(tries)
            .Retry("", () =>
            {
                if (++attempt != tries) { throw exception; }
            }, () => resetAttempt++);

        attempt.Assert().Is(tries).Also(resetAttempt).Is(tries - 1);
    }

    [Theory,
        InlineData(1),
        InlineData(3)]
    internal static async Task Retry_ReturnResetStateBehavior(int tries)
    {
        Exception exception = Tools.Randomizer.Create<Exception>();
        int attempt = 0;
        int resetAttempt = 0;

        int result = Tools.Randomizer.Create<int>();
        int ResetBehavior()
        {
            return (++attempt == tries) ? result : throw exception;
        }

        (await new AsyncLimiter(tries).Retry("", ResetBehavior, () => resetAttempt++))
            .Assert().Is(result)
            .Also(attempt).Is(tries)
            .Also(resetAttempt).Is(tries - 1);
    }

    [Theory, RandomData]
    internal static void Retry_WrongExceptionThrows(NotSupportedException exception)
    {
        new AsyncLimiter(3)
            .Assert(l => l.Retry<InvalidOperationException>("", (Action)(() => throw exception)).Wait())
            .Throws<NotSupportedException>()
            .Assert().Is(exception);

        IOException exception2 = new();

        new AsyncLimiter(3)
            .Assert(l => l.Retry<DirectoryNotFoundException, bool>("", () => throw exception2).Wait())
            .Throws<AggregateException>().InnerExceptions.Single().Assert().Is(exception2);
    }

    [Theory,
        InlineData(1),
        InlineData(3)]
    internal static async Task Attempt_ResetStateBehavior(int tries)
    {
        Exception exception = Tools.Randomizer.Create<Exception>();
        int attempt = 0;
        int resetAttempt = 0;

        await new AsyncLimiter(tries)
            .Attempt("", () =>
            {
                if (++attempt != tries) { throw exception; }
            }, () => resetAttempt++);

        attempt.Assert().Is(tries).Also(resetAttempt).Is(tries - 1);
    }

    [Theory,
        InlineData(1),
        InlineData(3)]
    internal static async Task Attempt_ReturnResetStateBehavior(int tries)
    {
        Exception exception = Tools.Randomizer.Create<Exception>();
        int attempt = 0;
        int resetAttempt = 0;

        int result = Tools.Randomizer.Create<int>();
        int ResetBehavior()
        {
            return (++attempt == tries) ? result : throw exception;
        }

        (await new AsyncLimiter(tries).Attempt("", ResetBehavior, () => resetAttempt++))
            .Assert().Is(result)
            .Also(attempt).Is(tries)
            .Also(resetAttempt).Is(tries - 1);
    }

    [Theory, RandomData]
    internal static void Attempt_WrongExceptionThrows(NotSupportedException exception)
    {
        new AsyncLimiter(3)
            .Assert(l => l.Attempt<InvalidOperationException>(null, (Action)(() => throw exception)).Wait())
            .Throws<NotSupportedException>().Assert().Is(exception);

        IOException exception2 = new();

        new AsyncLimiter(3)
            .Assert(l => l.Attempt<DirectoryNotFoundException, bool>("", () => throw exception2).Wait())
            .Throws<AggregateException>().InnerExceptions.Single().Assert().Is(exception2);
    }

    [Fact]
    internal static void AsyncLimiter_DefaultsSet()
    {
        foreach (PropertyInfo info in typeof(AsyncLimiter)
            .GetProperties(BindingFlags.Static | BindingFlags.Public)
            .Where(p => p.PropertyType == typeof(AsyncLimiter)))
        {
            info.GetValue(null).Assert().IsNot(null);
        }
    }

    [Theory, RandomData]
    internal static void Equality_MatchesValue(int tries, TimeSpan elapsed)
    {
        AsyncLimiter original = new(tries, elapsed);
        AsyncLimiter dupe = new(tries, elapsed);
        AsyncLimiter variant1 = new(tries.CreateVariant(), elapsed);
        AsyncLimiter variant2 = new(tries, elapsed.CreateVariant());

        true.Assert()
            .Is(original.Equals(original))
            .And.Is(original.Equals(dupe))
            .And.IsNot(original.Equals(variant1))
            .And.IsNot(original.Equals(variant2))
            .Also(original.GetHashCode())
            .Is(original.GetHashCode())
            .And.Is(dupe.GetHashCode())
            .And.IsNot(variant1.GetHashCode())
            .And.IsNot(variant2.GetHashCode());
    }

    [Theory, RandomData]
    internal static void ToString_Readable(int tries, TimeSpan timeout, TimeSpan delay)
    {
        new AsyncLimiter(timeout, tries, delay).ToString().Assert().Is($"{tries}-{timeout}-{delay}");
    }
}

#pragma warning restore xUnit1031 // Test methods should not use blocking code