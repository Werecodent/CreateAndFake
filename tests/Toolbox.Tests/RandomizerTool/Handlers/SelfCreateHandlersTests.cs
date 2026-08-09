using Werecodent.CreateAndFake.RandomizerTool.Handlers;

namespace Werecodent.CreateAndFake.Tests.RandomizerTool.Handlers;

public static class SelfCreateHandlersTests
{
    [Fact]
    internal static void SelfCreateHandlers_InternalOnly()
    {
        typeof(SelfCreateHandlers).IsPublic.Assert().Is(false);
    }
}
