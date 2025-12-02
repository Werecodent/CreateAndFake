using CreateAndFake.Design.Data;

namespace CreateAndFake.Tests.IssueReplication;

public static class Issue021Tests
{
    internal sealed class Sample
    {
        public string FirstName { get; set; }
    }

    [Theory, RandomData]
    internal static void Issue021_MutatorMutates(Sample sample)
    {
        string original = sample.FirstName;
        Tools.Mutator.Modify(sample);
        sample.FirstName.Assert().IsNot(original);
    }

    [Theory, RandomData]
    internal static void Issue021_MutatorUsesSmartData(Sample sample)
    {
        string original = sample.FirstName;
        Tools.Mutator.Modify(sample);
        NameData.Values.Assert().Contains(sample.FirstName);
    }
}
