using Werecodent.CreateAndFake.ExtractorTool.Handlers;

namespace Werecodent.CreateAndFake.Tests.ExtractorTool.Handlers;

public sealed class SystemExtractHandlersTests() : AutoTester(typeof(SystemExtractHandlers))
{
    [Fact]
    internal static void SystemExtractHandlers_InternalOnly()
    {
        typeof(SystemExtractHandlers).IsPublic.Assert().Is(false);
    }
}
