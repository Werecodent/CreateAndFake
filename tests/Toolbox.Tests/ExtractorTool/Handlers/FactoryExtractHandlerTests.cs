using Werecodent.CreateAndFake.ExtractorTool.Handlers;

namespace Werecodent.CreateAndFake.Tests.ExtractorTool.Handlers;

public static class FactoryExtractHandlerTests
{
    [Fact]
    internal static void FactoryExtractHandler_InternalOnly()
    {
        typeof(FactoryExtractHandler<>).IsPublic.Assert().Is(false);
    }
}
