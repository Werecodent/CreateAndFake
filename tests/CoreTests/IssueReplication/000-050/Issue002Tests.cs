using CreateAndFake.Toolbox.FakerTool.Proxy;

namespace CreateAndFakeTests.IssueReplication;

public static class Issue002Tests
{
    [Fact]
    internal static void Issue002_CallDataEncapsulated()
    {
        typeof(CallData).IsPublic.Assert().Is(false);
    }
}
