using Werecodent.CreateAndFake.ExtractorTool.Handlers;

namespace Werecodent.CreateAndFake.Tests.ExtractorTool.Handlers;

public sealed class ReflectionExtractHandlersTests() : AutoTester(typeof(ReflectionExtractHandlers))
{
    [Fact]
    internal static void ReflectionExtractHandlers_InternalOnly()
    {
        typeof(ReflectionExtractHandlers).IsPublic.Assert().Is(false);
    }
}
