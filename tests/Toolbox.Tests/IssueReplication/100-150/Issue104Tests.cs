namespace Werecodent.CreateAndFake.Tests.IssueReplication;

public static class Issue104Tests
{
    [Theory, RandomData]
    internal static void Issue104_EasyVariantValues(int value)
    {
        value.Tools().Variant().Assert().IsNot(value);
    }

    [Theory, RandomData]
    internal static void Issue104_EasyVariantObjects(string value)
    {
        value.Tools().Variant().Assert().IsNot(value);
    }
}
