namespace CreateAndFakeTests.IssueReplication;

public static class Issue021Tests
{
    internal sealed class Sample
    {
        public string Value { get; set; }
    }

    [Theory, RandomData]
    internal static void Issue021_MutatorMutates(Sample sample)
    {
        string original = sample.Value;
        Tools.Mutator.Modify(sample);
        sample.Value.Assert().IsNot(original);
    }
}