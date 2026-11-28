using Werecodent.CreateAndFake.DuplicatorTool.Handlers;

namespace Werecodent.CreateAndFake.Tests.DuplicatorTool.Handlers;

public sealed class LegacyCollectionCopyHandlersTests()
    : AutoTester(typeof(LegacyCollectionCopyHandlers))
{
    [Fact]
    internal static void LegacyCollectionCopyHandlers_InternalOnly()
    {
        typeof(LegacyCollectionCopyHandlers).IsPublic.Assert().Is(false);
    }
}
