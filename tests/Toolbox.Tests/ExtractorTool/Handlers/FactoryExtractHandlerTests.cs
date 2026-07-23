using CreateAndFake.ExtractorTool.Handlers;

namespace CreateAndFake.Tests.ExtractorTool.Handlers;

public static class FactoryExtractHandlerTests
{
    [Fact]
    internal static void FactoryExtractHandler_InternalOnly()
    {
        typeof(FactoryExtractHandler<>).IsPublic.Assert().Is(false);
    }
}
