namespace CreateAndFakeTests.IssueReplication;

public static class Issue065Tests
{
    internal sealed class InfiniteA
    {
        public ICollection<InfiniteB> Start { get; set; }
    }

    internal sealed class InfiniteB
    {
        public InfiniteC Nested { get; set; }
    }

    internal sealed class InfiniteC
    {
        public InfiniteA Back { get; set; }
    }

    [Theory, RandomData]
    internal static void Issue065_HandlesInfiniteChainsA(InfiniteA sample)
    {
        TestInfiniteSample(sample);
    }

    [Theory, RandomData]
    internal static void Issue065_HandlesInfiniteChainsB(InfiniteB sample)
    {
        TestInfiniteSample(sample);
    }

    [Theory, RandomData]
    internal static void Issue065_HandlesInfiniteChainsC(InfiniteC sample)
    {
        TestInfiniteSample(sample);
    }

    private static void TestInfiniteSample<T>(T sample) where T : new()
    {
        sample.Assert()
            .IsNot(null).And
            .IsNot(new T()).And
            .IsNot(sample.CreateVariant());

        T dupe = sample.CreateDeepClone();

        dupe.Assert().Is(sample);
        Tools.Valuer.GetHashCode(dupe).Assert().Is(Tools.Valuer.GetHashCode(sample));
    }
}
