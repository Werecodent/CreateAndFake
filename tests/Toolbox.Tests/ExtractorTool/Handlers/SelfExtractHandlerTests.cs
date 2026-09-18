using Werecodent.CreateAndFake.ExtractorTool.Handlers;

namespace Werecodent.CreateAndFake.Tests.ExtractorTool.Handlers;

public static class SelfExtractHandlerTests
{
    [Fact]
    internal static void SelfExtractHandler_InternalOnly()
    {
        typeof(SelfExtractHandler).IsPublic.Assert().Is(false);
    }
}
