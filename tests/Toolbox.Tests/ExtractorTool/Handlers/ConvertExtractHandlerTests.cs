using Werecodent.CreateAndFake.ExtractorTool.Handlers;

namespace Werecodent.CreateAndFake.Tests.ExtractorTool.Handlers;

public static class ConvertExtractHandlerTests
{
    [Fact]
    internal static void ConvertExtractHandler_InternalOnly()
    {
        typeof(ConvertExtractHandler).IsPublic.Assert().Is(false);
    }
}
