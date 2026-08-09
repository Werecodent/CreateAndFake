using Werecodent.CreateAndFake.DuplicatorTool.Handlers;

namespace Werecodent.CreateAndFake.Tests.DuplicatorTool.Handlers;

public static class LegacyCollectionCopyHandlersTests
{
    [Fact]
    internal static void LegacyCollectionCopyHandlers_InternalOnly()
    {
        typeof(LegacyCollectionCopyHandlers).IsPublic.Assert().Is(false);
    }
}
