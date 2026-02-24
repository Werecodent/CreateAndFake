using CreateAndFake.RandomizerTool.Handlers;

namespace CreateAndFake.Tests.RandomizerTool.Handlers;

public static class ReflectionCreateHandlersTests
{
    [Fact]
    internal static void ReflectionCreateHandlers_InternalOnly()
    {
        typeof(ReflectionCreateHandlers).IsPublic.Assert().Is(false);
    }
}
