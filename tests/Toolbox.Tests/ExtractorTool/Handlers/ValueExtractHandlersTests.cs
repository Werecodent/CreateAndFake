using CreateAndFake.ExtractorTool.Handlers;

namespace CreateAndFake.Tests.ExtractorTool.Handlers;

public static class ValueExtractHandlersTests
{
    [Fact]
    internal static void ValueExtractHandlers_InternalOnly()
    {
        typeof(ValueExtractHandlers).IsPublic.Assert().Is(false);
    }
}
