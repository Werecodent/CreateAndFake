using System.Collections;

namespace CreateAndFakeTests.IssueReplication;

public static class Issue086Tests
{
    [Theory, RandomData]
    internal static void Issue086_SizeWorks([Size(5)] string[] items)
    {
        items.Assert().HasCount(5);
    }

    [Theory, RandomData]
    internal static void Issue086_EmptyCollection([Size(0)] List<int> items)
    {
        items.Assert().IsEmpty();
    }

    [Theory, RandomData]
    internal static void Issue086_LegacyCollection([Size(3)] Hashtable items)
    {
        items.Keys.Assert().HasCount(3);
    }
}