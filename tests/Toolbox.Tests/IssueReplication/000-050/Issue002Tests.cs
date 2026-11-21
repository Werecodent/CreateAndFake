using Werecodent.CreateAndFake.FakerTool.Proxy;

namespace Werecodent.CreateAndFake.Tests.IssueReplication;

public static class Issue002Tests
{
    [Fact]
    internal static void Issue002_CallDataEncapsulated()
    {
        typeof(CallData).IsPublic.Assert().Is(false);
    }
}
