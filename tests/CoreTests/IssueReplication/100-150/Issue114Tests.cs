namespace CreateAndFakeTests.IssueReplication;

public static class Issue114Tests
{
    [Theory, RandomData]
    internal static void Issue114_IntArraySequencesEqualRegardlessOfType(int[] sample)
    {
        sample.Assert().Is(sample.CreateDeepClone().ToList());
    }

    [Theory, RandomData]
    internal static void Issue114_ObjectArraySequencesEqualRegardlessOfType(object[] sample)
    {
        sample.Assert().Is(sample.CreateDeepClone().ToList());
    }

    [Theory, RandomData]
    internal static void Issue114_StringListSequencesEqualRegardlessOfType(IEnumerable<string> sample)
    {
        sample.Assert().Is(sample.CreateDeepClone().ToList());
    }

    [Theory, RandomData]
    internal static void Issue114_ObjectListSequencesEqualRegardlessOfType(IEnumerable<object> sample)
    {
        sample.Assert().Is(sample.CreateDeepClone().ToList());
    }

    [Theory, RandomData]
    internal static void Issue114_StringSequencesEqualRegardlessOfType(IDictionary<string, object> sample)
    {
        sample.Assert().Is(sample.CreateDeepClone().ToDictionary(e => e.Key, e => e.Value));
    }
}