using CreateAndFake.Design.Data;

namespace CreateAndFakeTests.IssueReplication;

public static class Issue009Tests
{
    internal sealed class Sample(string lastName)
    {
        public string FirstName { get; set; }

        public string _middleName = null;

        public string LastName { get; } = lastName;
    }

    [Theory, RandomData]
    internal static void Issue009_SmartDataIsUsed(Sample sample)
    {
        NameData.Values.Assert().Contains(sample.FirstName);
        NameData.Values.Assert().Contains(sample._middleName);
        NameData.Values.Assert().Contains(sample.LastName);
    }
}