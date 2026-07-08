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
}
