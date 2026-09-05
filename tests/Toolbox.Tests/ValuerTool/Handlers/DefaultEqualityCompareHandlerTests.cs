using Werecodent.CreateAndFake.ValuerTool.Handlers;

namespace Werecodent.CreateAndFake.Tests.ValuerTool.Handlers;

public static class DefaultEqualityCompareHandlerTests
{
    [Fact]
    internal static void DefaultEqualityCompareHandler_InternalOnly()
    {
        typeof(DefaultEqualityCompareHandler).IsPublic.Assert().Is(false);
    }
}
