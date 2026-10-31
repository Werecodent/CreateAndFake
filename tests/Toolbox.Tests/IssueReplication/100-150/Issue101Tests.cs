namespace Werecodent.CreateAndFake.Tests.IssueReplication;

public static class Issue101Tests
{
    [Theory, RandomData]
    internal static void Issue101_EasyCloneValues(int value)
    {
        value.Tools().Copy().Assert().Is(value);
    }

    [Theory, RandomData]
    internal static void Issue101_EasyCloneObjects(string value)
    {
        value.Tools().Copy().Assert().Is(value);
    }
}
