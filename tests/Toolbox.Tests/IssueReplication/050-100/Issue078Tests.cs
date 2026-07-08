namespace CreateAndFake.Tests.IssueReplication;

public static class Issue078Tests
{
    [Theory, RandomData]
    internal static void Issue078_CanChainObjects(int[] valueA, bool valueB)
    {
        valueA.Assert().IsNotEmpty().Also(valueB).IsNotNull();
    }

    [Theory, RandomData]
    internal static void Issue078_CanChainCollections(bool valueA, int[] valueB)
    {
        valueA.Assert().IsNotNull().Also(valueB).IsNotEmpty();
    }
}
