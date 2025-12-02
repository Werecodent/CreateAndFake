namespace CreateAndFake.Tests.IssueReplication;

public static class Issue001Tests
{
    internal sealed class Sample
    {
        public int Value { get; set; }

        public string Name { get; set; }
    }

    [Theory, RandomData]
    internal static void Issue001_CanCreateUniques(Sample original)
    {
        Sample unique = original.CreateUnique();
        unique.Name.Assert().IsNot(original.Name);
        unique.Value.Assert().IsNot(original.Value);
        unique.Assert().UniqueFrom(original);
    }

    [Theory, RandomData]
    internal static void Issue001_CanCreateUniqueValues(int original)
    {
        original.CreateUnique().Assert().IsNot(original);
    }
}
