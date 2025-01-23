using CreateAndFakeTests.TestSamples;

namespace CreateAndFakeTests.IssueReplication;

public static class Issue117Tests
{
    [Fact]
    internal static void Issue117_AddsTypeAssertions()
    {
        typeof(string).Assert().Inherits<object>().Also(typeof(int)).Inherits<object>();
    }

    [Theory, RandomData]
    internal static void Issue117_AddsContainsAssertions(IList<ValueEquatableSample> sample)
    {
        sample.Assert().Contains(Tools.Gen.NextItem(sample));
    }
}
