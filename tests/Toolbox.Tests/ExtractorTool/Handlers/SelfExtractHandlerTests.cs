using CreateAndFake.ExtractorTool.Handlers;

namespace CreateAndFake.Tests.ExtractorTool.Handlers;

public static class SelfExtractHandlerTests
{
    [Fact]
    internal static void SelfExtractHandler_InternalOnly()
    {
        typeof(SelfExtractHandler).IsPublic.Assert().Is(false);
    }
}
