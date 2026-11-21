namespace Werecodent.CreateAndFake.Tests.IssueReplication;

public static class Issue114Tests
{
    [Theory, RandomData]
    internal static void Issue114_IntArraySequencesEqualRegardlessOfType(int[] sample)
    {
        sample.Assert().Is(sample.Tools().Copy().ToList());
    }

    [Theory, RandomData]
    internal static void Issue114_ObjectArraySequencesEqualRegardlessOfType(object[] sample)
    {
        sample.Assert().Is(sample.Tools().Copy().ToList());
    }

    [Theory, RandomData]
    internal static void Issue114_StringListSequencesEqualRegardlessOfType(IList<string> sample)
    {
        sample.Assert().Is(sample.Tools().Copy().ToList());
    }

    [Theory, RandomData]
    internal static void Issue114_ObjectListSequencesEqualRegardlessOfType(IList<object> sample)
    {
        sample.Assert().Is(sample.Tools().Copy().ToList());
    }

    [Theory, RandomData]
    internal static void Issue114_StringSequencesEqualRegardlessOfType(
        IDictionary<string, object> sample
    )
    {
        sample.Assert().Is(sample.Tools().Copy().ToDictionary(e => e.Key, e => e.Value));
    }
}
