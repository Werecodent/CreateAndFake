using CreateAndFake.FakerTool;

namespace CreateAndFake.Tests.IssueReplication;

public static class Issue012Tests
{
    public abstract class Sample
    {
        public abstract int Value();
    }

    [Theory, RandomData]
    internal static void Issue012_RandomizedFake(Fake<Sample> sample)
    {
        sample.Dummy.Value().Assert().IsNot(0);
    }

    [Theory, RandomData]
    internal static void Issue012_FakeImmutableResult(Fake<Sample> sample)
    {
        sample.CreateDeepClone().Dummy.Value().Assert().Is(sample.Dummy.Value());
    }
}
