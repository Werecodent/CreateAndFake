using CreateAndFake.RandomizerTool.Handlers;

namespace CreateAndFake.Tests.RandomizerTool.Handlers;

public static class SystemCreateHandlersTests
{
    [Fact]
    internal static void SystemCreateHandlers_InternalOnly()
    {
        typeof(SystemCreateHandlers).IsPublic.Assert().Is(false);
    }
}
