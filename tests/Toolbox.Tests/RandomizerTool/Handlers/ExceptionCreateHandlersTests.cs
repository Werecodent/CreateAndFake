using CreateAndFake.RandomizerTool.Handlers;

namespace CreateAndFake.Tests.RandomizerTool.Handlers;

public static class ExceptionCreateHandlersTests
{
    [Fact]
    internal static void ExceptionCreateHandlers_InternalOnly()
    {
        typeof(ExceptionCreateHandlers).IsPublic.Assert().Is(false);
    }
}
