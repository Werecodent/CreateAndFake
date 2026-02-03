using CreateAndFake.ValuerTool.Handlers;

namespace CreateAndFake.Tests.ValuerTool.Handlers;

public static class FallbackCompareHandlerTests
{
    [Fact]
    internal static void FallbackCompareHandler_InternalOnly()
    {
        typeof(FallbackCompareHandler).IsPublic.Assert().Is(false);
    }
}
