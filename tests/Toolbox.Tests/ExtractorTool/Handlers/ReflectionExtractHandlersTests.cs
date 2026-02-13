using CreateAndFake.ExtractorTool.Handlers;

namespace CreateAndFake.Tests.ExtractorTool.Handlers;

public static class ReflectionExtractHandlersTests
{
    [Fact]
    internal static void ReflectionExtractHandlers_InternalOnly()
    {
        typeof(ReflectionExtractHandlers).IsPublic.Assert().Is(false);
    }
}
