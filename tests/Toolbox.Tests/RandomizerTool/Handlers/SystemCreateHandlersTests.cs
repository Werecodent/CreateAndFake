using Werecodent.CreateAndFake.RandomizerTool.Handlers;

namespace Werecodent.CreateAndFake.Tests.RandomizerTool.Handlers;

public sealed class SystemCreateHandlersTests() : AutoTester(typeof(SystemCreateHandlers))
{
    [Fact]
    internal static void SystemCreateHandlers_InternalOnly()
    {
        typeof(SystemCreateHandlers).IsPublic.Assert().Is(false);
    }
}
