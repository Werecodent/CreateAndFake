using Werecodent.CreateAndFake.RandomizerTool.Handlers;

namespace Werecodent.CreateAndFake.Tests.RandomizerTool.Handlers;

public static class SystemCreateHandlersTests
{
    [Fact]
    internal static void SystemCreateHandlers_InternalOnly()
    {
        typeof(SystemCreateHandlers).IsPublic.Assert().Is(false);
    }
}
