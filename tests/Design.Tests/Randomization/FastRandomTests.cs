using CreateAndFake.Design.Randomization;
using CreateAndFake.Design.Reiteration;

namespace CreateAndFake.Design.Tests.Randomization;

public sealed class FastRandomTests : ValueRandomTestBase<FastRandom>
{
    private static readonly double[] _BadDoubles =
    [
        double.NaN,
        double.NegativeInfinity,
        double.PositiveInfinity,
    ];

    private static readonly float[] _BadFloats =
    [
        float.NaN,
        float.NegativeInfinity,
        float.PositiveInfinity,
    ];

    [Fact]
    internal static void Create_InvalidValuesPossible()
    {
        FastRandom random = new(false);

        new Limiter(20000).StallUntil(
            "Trying to create bad double.",
            () => _BadDoubles.Contains(random.Next<double>()),
            TestContext.Current.CancellationToken
        );
        Limiter.Myriad.StallUntil(
            "Trying to create bad float.",
            () => _BadFloats.Contains(random.Next<float>()),
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static void Create_OnlyValidValuesPreventsInvalids()
    {
        FastRandom random = new(true);

        Limiter.Myriad.Repeat(
            "Trying to avoid bad doubles.",
            () => _BadDoubles.Assert().ContainsNot(random.Next<double>()),
            TestContext.Current.CancellationToken
        );
        Limiter.Myriad.Repeat(
            "Trying to avoid bad floats.",
            () => _BadFloats.Assert().ContainsNot(random.Next<float>()),
            TestContext.Current.CancellationToken
        );
    }
}
