namespace CreateAndFakeTests.IssueReplication;

public static class Issue104Tests
{
    [Theory, RandomData]
    internal static void Issue104_EasyVariantValues(int value)
    {
        value.CreateVariant().Assert().IsNot(value);
    }

    [Theory, RandomData]
    internal static void Issue104_EasyVariantObjects(string value)
    {
        value.CreateVariant().Assert().IsNot(value);
    }
}
