using System.Reflection;
using CreateAndFake.Design.Content;
using CreateAndFake.Design.Reiteration;

namespace CreateAndFake.Design.Tests.Reiteration;

#pragma warning disable xUnit1031 // Ensures blocking code works for library.

public static class LimiterBaseTests
{
    [Fact]
    internal static void Limiter_DefaultsSet()
    {
        foreach (
            PropertyInfo info in TypeDescriber
                .GetAllProperties(typeof(Limiter), BindingFlags.Public)
                .Where(p => p.PropertyType == typeof(Limiter))
        )
        {
            info.GetValue(null).Assert().IsNot(null);
        }
    }

    [Theory, RandomData]
    internal static void Equality_MatchesValue(int tries, TimeSpan elapsed)
    {
        Limiter original = new(tries, elapsed);
        Limiter dupe = new(tries, elapsed);
        Limiter variant1 = new(tries.CreateVariant(), elapsed);
        Limiter variant2 = new(tries, elapsed.CreateVariant());

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

    [Theory, RandomData]
    internal static void ToString_Readable(int tries, TimeSpan timeout, TimeSpan delay)
    {
        new Limiter(timeout, tries, delay).ToString().Assert().Is($"{tries}-{timeout}-{delay}");
    }
}

#pragma warning restore xUnit1031
