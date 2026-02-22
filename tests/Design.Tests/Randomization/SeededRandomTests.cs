using CreateAndFake.Design.Randomization;

namespace CreateAndFake.Design.Tests.Randomization;

public sealed class SeededRandomTests()
    : ValueRandomTestBase<SeededRandom>(Tools.Randomizer.Create<SeededRandom>())
{
    [Fact]
    internal static void Seed_Deterministic()
    {
        SeededRandom random1 = new(10);
        SeededRandom random2 = new(random1.Seed);

        random1.Next<int>().Assert().Is(random2.Next<int>());
        random1.Next<int>().Assert().Is(random2.Next<int>());

        random2 = new SeededRandom(random1.Seed);

        random1.Next<int>().Assert().Is(random2.Next<int>());
        random1.Seed.Assert().Is(random2.Seed);
    }
}
