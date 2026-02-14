using CreateAndFake.ValuerTool.Handlers;

namespace CreateAndFake.Tests.ValuerTool.Handlers;

public static class DefaultEqualityCompareHandlerTests
{
    [Fact]
    internal static void DefaultEqualityCompareHandler_InternalOnly()
    {
        typeof(DefaultEqualityCompareHandler).IsPublic.Assert().Is(false);
    }
}
