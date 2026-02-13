using CreateAndFake.ExtractorTool.Handlers;

namespace CreateAndFake.Tests.ExtractorTool.Handlers;

public static class SelfExtractHandler_T_Tests
{
    [Fact]
    internal static void SelfExtractHandler_T__InternalOnly()
    {
        typeof(SelfExtractHandler<>).IsPublic.Assert().Is(false);
    }
}
