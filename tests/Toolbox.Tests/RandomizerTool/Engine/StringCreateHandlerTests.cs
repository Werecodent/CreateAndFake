using CreateAndFake.RandomizerTool.Handlers;

namespace CreateAndFake.Tests.RandomizerTool.Engine;

public static class StringCreateHandlerTests
{
    [Fact]
    internal static void StringCreateHandler_InternalOnly()
    {
        typeof(StringCreateHandler).IsPublic.Assert().Is(false);
    }
}
