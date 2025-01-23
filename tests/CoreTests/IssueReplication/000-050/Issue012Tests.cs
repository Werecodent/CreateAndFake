using CreateAndFake.Toolbox.FakerTool;

namespace CreateAndFakeTests.IssueReplication;

public static class Issue012Tests
{
    public abstract class Sample
    {
        public abstract double Value();
    }

    [Theory, RandomData]
    internal static void Issue012_RandomizedFake(Fake<Sample> sample)
    {
        sample.Dummy.Value().Assert().IsNot(sample.Dummy.Value());
    }
}
