using Werecodent.CreateAndFake.ExtractorTool.Handlers;

namespace Werecodent.CreateAndFake.Tests.ExtractorTool.Handlers;

public sealed class SelfExtractHandlerTests() : AutoTester(typeof(SelfExtractHandler))
{
    [Fact]
    internal static void SelfExtractHandler_InternalOnly()
    {
        typeof(SelfExtractHandler).IsPublic.Assert().Is(false);
    }
}
