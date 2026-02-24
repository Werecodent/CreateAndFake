using CreateAndFake.RandomizerTool.Handlers;

namespace CreateAndFake.Tests.RandomizerTool.Handlers;

public static class ValueCreateHandlersTests
{
    [Fact]
    internal static void ValueCreateHandlers_InternalOnly()
    {
        typeof(ValueCreateHandlers).IsPublic.Assert().Is(false);
    }
}
