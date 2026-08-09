using Werecodent.CreateAndFake.ValuerTool.Handlers;

namespace Werecodent.CreateAndFake.Tests.ValuerTool.Handlers;

public static class ConvertCompareHandlerTests
{
    [Fact]
    internal static void ConvertCompareHandler_InternalOnly()
    {
        typeof(ConvertCompareHandler<>).IsPublic.Assert().Is(false);
    }
}
