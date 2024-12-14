using CreateAndFake.Design;
using CreateAndFake.Design.Randomization;
using CreateAndFakeTests.TestBases;

namespace CreateAndFakeTests.Design.Randomization;

public sealed class FastRandomTests : ValueRandomTestBase<FastRandom>
{
    private static readonly double[] _BadDoubles = [
        double.NaN,
        double.NegativeInfinity,
        double.PositiveInfinity];

    private static readonly float[] _BadFloats = [
        float.NaN,
        float.NegativeInfinity,
        float.PositiveInfinity];

    [Fact]
    internal static void Create_InvalidValuesPossible()
    {
        FastRandom random = new(false);

        Limiter limiter = new(15000);
        limiter.StallUntil("Trying to create bad double.",
            () => _BadDoubles.Contains(random.Next<double>()));
        limiter.StallUntil("Trying to create bad float.",
            () => _BadFloats.Contains(random.Next<float>()));
    }

    [Fact]
    internal static void Create_OnlyValidValuesPreventsInvalids()
    {
        FastRandom random = new(true);

        Limiter.Myriad.Repeat("Trying to avoid bad doubles.",
            () => _BadDoubles.Assert().ContainsNot(random.Next<double>()));
        Limiter.Myriad.Repeat("Trying to avoid bad floats.",
            () => _BadFloats.Assert().ContainsNot(random.Next<float>()));
    }
}
