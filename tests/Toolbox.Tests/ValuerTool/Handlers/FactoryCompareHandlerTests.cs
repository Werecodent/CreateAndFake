using CreateAndFake.ValuerTool.Handlers;

namespace CreateAndFake.Tests.ValuerTool.Handlers;

public static class FactoryCompareHandlerTests
{
    [Fact]
    internal static void FactoryCompareHandler_InternalOnly()
    {
        typeof(FactoryCompareHandler).IsPublic.Assert().Is(false);
    }
}
