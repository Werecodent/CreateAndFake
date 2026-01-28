using System.Reflection;
using CreateAndFake.Design.Content;
using CreateAndFake.Design.Reiteration;

namespace CreateAndFake.Design.Tests.Reiteration;

public static class LimiterBaseTests
{
    [Fact]
    internal static void Limiter_DefaultsSet()
    {
        foreach (
            PropertyInfo info in TypeDescriber
                .GetAllProperties(typeof(Limiter), true)
                .Where(p => p.PropertyType == typeof(Limiter))
        )
        {
            info.GetValue(null).Assert().IsNot(null);
        }
    }

    [Fact]
    internal static void Equality_MatchesValue()
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
}
