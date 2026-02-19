using CreateAndFake.DuplicatorTool.Handlers;

namespace CreateAndFake.Tests.DuplicatorTool.Handlers;

public static class SelfCopyHandlersTests
{
    [Fact]
    internal static void SelfCopyHandlers_InternalOnly()
    {
        typeof(SelfCopyHandlers).IsPublic.Assert().Is(false);
    }
}
