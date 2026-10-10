using Werecodent.CreateAndFake.ExtractorTool.Handlers;

namespace Werecodent.CreateAndFake.Tests.ExtractorTool.Handlers;

public static class ReflectionExtractHandlersTests
{
    [Fact]
    internal static void ReflectionExtractHandlers_InternalOnly()
    {
        typeof(ReflectionExtractHandlers).IsPublic.Assert().Is(false);
    }
}
