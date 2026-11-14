using Werecodent.CreateAndFake.ExtractorTool.Handlers;

namespace Werecodent.CreateAndFake.Tests.ExtractorTool.Handlers;

public sealed class ValueExtractHandlersTests() : AutoTester(typeof(ValueExtractHandlers))
{
    [Fact]
    internal static void ValueExtractHandlers_InternalOnly()
    {
        typeof(ValueExtractHandlers).IsPublic.Assert().Is(false);
    }
}
