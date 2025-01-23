namespace CreateAndFakeTests.IssueReplication;

public static class Issue078Tests
{
    [Theory, RandomData]
    internal static void Issue078_CanChainObjects(int[] valueA, bool valueB)
    {
        valueA.Assert().IsNotEmpty().Also(valueB).IsNot(null);
    }

    [Theory, RandomData]
    internal static void Issue078_CanChainCollections(bool valueA, int[] valueB)
    {
        valueA.Assert().IsNot(null).Also(valueB).IsNotEmpty();
    }
}