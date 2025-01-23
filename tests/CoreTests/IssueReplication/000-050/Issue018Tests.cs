using CreateAndFake.Toolbox.FakerTool;

namespace CreateAndFakeTests.IssueReplication;

public static class Issue018Tests
{
    internal sealed class Sample(IInternalSample internalSample)
    {
        public IInternalSample InternalSample { get; } = internalSample;
    }

    public interface IInternalSample
    {
        string Value { get; }
    }

    [Theory, RandomData]
    internal static void Issue018_RandomizerInjects(Injected<Sample> sample)
    {
        sample.Dummy.InternalSample.Assert().Is(sample.Fake<IInternalSample>().Dummy);
    }
}