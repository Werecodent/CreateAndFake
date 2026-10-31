using Werecodent.CreateAndFake.ExtractorTool.Handlers;

namespace Werecodent.CreateAndFake.Tests.ExtractorTool.Handlers;

public static class ValueExtractHandlersTests
{
    [Fact]
    internal static void ValueExtractHandlers_InternalOnly()
    {
        typeof(ValueExtractHandlers).IsPublic.Assert().Is(false);
    }
}
