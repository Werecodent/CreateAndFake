using CreateAndFake.DuplicatorTool.Handlers;

namespace CreateAndFake.Tests.DuplicatorTool.Handlers;

public static class SystemCopyHandlersTests
{
    [Fact]
    internal static void SystemCopyHandlers_InternalOnly()
    {
        typeof(SystemCopyHandlers).IsPublic.Assert().Is(false);
    }
}
