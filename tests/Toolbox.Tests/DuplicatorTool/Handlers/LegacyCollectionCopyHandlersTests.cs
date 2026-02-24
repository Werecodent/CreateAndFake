using CreateAndFake.DuplicatorTool.Handlers;

namespace CreateAndFake.Tests.DuplicatorTool.Handlers;

public static class LegacyCollectionCopyHandlersTests
{
    [Fact]
    internal static void LegacyCollectionCopyHandlers_InternalOnly()
    {
        typeof(LegacyCollectionCopyHandlers).IsPublic.Assert().Is(false);
    }
}
