using System.Collections.Frozen;
using System.Reflection;
using CreateAndFake.Design.Reiteration;
using CreateAndFake.Design.Types;

namespace CreateAndFake.Design.Tests.Reiteration;

public static class LimiterTests
{
    private static readonly FrozenSet<Type> _IgnorableExceptions =
    [
        typeof(ArgumentOutOfRangeException),
        typeof(TimeoutException),
        typeof(FormatException),
    ];

    [Fact]
    internal static void Debug_Limiter_NamesWithDurationEstimates()
    {
        Limiter customLimiter = new(TimeSpan.FromSeconds(1), 2, null);

        new Dictionary<Limiter, TimeSpan>()
        {
            { Limiter.Once, Limiter.Once.GetMaxDurationEstimate() },
            { Limiter.Few, Limiter.Few.GetMaxDurationEstimate() },
            { Limiter.Dozen, Limiter.Dozen.GetMaxDurationEstimate() },
            { Limiter.Score, Limiter.Score.GetMaxDurationEstimate() },
            { Limiter.Hundred, Limiter.Hundred.GetMaxDurationEstimate() },
            { Limiter.Myriad, Limiter.Myriad.GetMaxDurationEstimate() },
            { Limiter.Quick, Limiter.Quick.GetMaxDurationEstimate() },
            { Limiter.Fast, Limiter.Fast.GetMaxDurationEstimate() },
            { Limiter.Slow, Limiter.Slow.GetMaxDurationEstimate() },
            { customLimiter, customLimiter.GetMaxDurationEstimate() },
        }
            .Assert()
            .Debug();
    }

    [Fact]
    internal static Task Limiter_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            Limiter.Few,
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = _IgnorableExceptions }
        );
    }

    [Fact]
    internal static Task Limiter_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            Limiter.Few,
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = _IgnorableExceptions }
        );
    }

    [Fact]
    internal static void Limiter_DefaultsConvertible()
    {
        foreach (
            PropertyInfo info in TypeDescriber
                .For<Limiter>()
                .Properties.OnlyPublic.Where(p => p.PropertyType == typeof(Limiter))
        )
        {
            Limiter instance = (Limiter)info.GetValue(null);
            Limiter.ConvertFrom(instance.ToString(), null).Assert().ReferenceEqual(instance);
        }
    }

    [Fact]
    internal static void Limiter_CompareWorks()
    {
        Limiter
            .Once.Equals((ILimiter)Limiter.Once)
            .Assert()
            .Is(true, ".Equals")
            .Also(Limiter.Dozen.Equals(Limiter.Few))
            .Is(false, ".Equals")
            .Also(Limiter.Myriad == Limiter.Myriad)
            .Is(true, "==")
            .Also(Limiter.Myriad == Limiter.Hundred)
            .Is(false, "==")
            .Also(Limiter.Fast != Limiter.Once)
            .Is(true, "!=")
            .Also(Limiter.Dozen != Limiter.Dozen)
            .Is(false, "!=")
            .Also(Limiter.Dozen > Limiter.Few)
            .Is(true, ">")
            .Also(Limiter.Once > Limiter.Few)
            .Is(false, ">")
            .Also(Limiter.Hundred >= Limiter.Hundred)
            .Is(true, ">=")
            .Also(Limiter.Quick >= Limiter.Slow)
            .Is(false, ">=")
            .Also(Limiter.Once < Limiter.Fast)
            .Is(true, "<")
            .Also(Limiter.Myriad < Limiter.Few)
            .Is(false, "<")
            .Also(Limiter.Slow <= Limiter.Slow)
            .Is(true, "<=")
            .Also(Limiter.Myriad <= Limiter.Hundred)
            .Is(false, "<=");
    }

    [Fact]
    internal static void Limiter_PositiveTimesOnly()
    {
        new Limiter(TimeSpan.FromTicks(1), 1, TimeSpan.Zero)
            .Assert()
            .IsNotNull()
            .Also(() => new Limiter(TimeSpan.Zero, 1, TimeSpan.Zero))
            .Throws<ArgumentOutOfRangeException>()
            .Also(() => new Limiter(TimeSpan.FromTicks(1), 0, TimeSpan.Zero))
            .Throws<ArgumentOutOfRangeException>()
            .Also(() => new Limiter(TimeSpan.FromTicks(1), 1, TimeSpan.FromTicks(-1)))
            .Throws<ArgumentOutOfRangeException>()
            .Also(new Limiter(TimeSpan.FromTicks(1), 1, null))
            .IsNotNull()
            .Also(new Limiter(TimeSpan.FromTicks(1), 1, TimeSpan.FromTicks(1)));
    }

    [Fact]
    internal static void Equals_MatchesValue()
    {
        int tries = Tools.Gen.Next(int.MaxValue);
        TimeSpan elapsed = Tools.Gen.Next(TimeSpan.MaxValue);

        Limiter original = new(tries, elapsed);
        Limiter dupe = new(tries, elapsed);
        Limiter variant1 = new(Tools.Gen.Next(int.MaxValue), elapsed);
        Limiter variant2 = new(tries, Tools.Gen.Next(TimeSpan.MaxValue));

        true
            .Assert()
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

    [Fact]
    internal static void ToString_Readable()
    {
        int tries = Tools.Gen.Next(int.MaxValue);
        TimeSpan timeout = Tools.Gen.Next(TimeSpan.MaxValue);
        TimeSpan delay = Tools.Gen.Next(TimeSpan.MaxValue);

        new Limiter(timeout, tries, delay).ToString().Assert().Is($"{tries}-{timeout}-{delay}");
    }

    [Fact]
    internal static void ConvertFrom_ToString()
    {
        int tries = Tools.Gen.Next(int.MaxValue);
        TimeSpan timeout = Tools.Gen.Next(TimeSpan.MaxValue);
        TimeSpan delay = Tools.Gen.Next(TimeSpan.MaxValue);

        Limiter tryLimiter = new(tries);
        Limiter tryDelayLimiter = new(tries, delay);
        Limiter timeoutLimiter = new(timeout);
        Limiter timeoutDelayLimiter = new(timeout, delay);
        Limiter fullLimiter = new(timeout, tries, delay);

        Limiter.ConvertFrom(tryLimiter.ToString(), null).Assert().Is(tryLimiter);
        Limiter.ConvertFrom(tryDelayLimiter.ToString(), null).Assert().Is(tryDelayLimiter);
        Limiter.ConvertFrom(timeoutLimiter.ToString(), null).Assert().Is(timeoutLimiter);
        Limiter.ConvertFrom(timeoutDelayLimiter.ToString(), null).Assert().Is(timeoutDelayLimiter);
        Limiter.ConvertFrom(fullLimiter.ToString(), null).Assert().Is(fullLimiter);
    }

    [Fact]
    internal static void GetMaxDurationEstimate_ThrowsWithNegative()
    {
        Limiter.Once.Assert(x => x.GetMaxDurationEstimate(0)).Throws<ArgumentOutOfRangeException>();
        Limiter.Few.Assert(x => x.GetMaxDurationEstimate(-1)).Throws<ArgumentOutOfRangeException>();
    }
}
