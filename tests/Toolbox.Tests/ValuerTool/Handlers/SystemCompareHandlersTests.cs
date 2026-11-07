using Werecodent.CreateAndFake.ValuerTool.Handlers;

namespace Werecodent.CreateAndFake.Tests.ValuerTool.Handlers;

public sealed class SystemCompareHandlersTests() : AutoTester(typeof(SystemCompareHandlers))
{
    [Fact]
    internal static void SystemCompareHandlers_InternalOnly()
    {
        typeof(SystemCompareHandlers).IsPublic.Assert().Is(false);
    }
}
