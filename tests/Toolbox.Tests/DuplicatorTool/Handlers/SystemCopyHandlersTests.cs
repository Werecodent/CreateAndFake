using Werecodent.CreateAndFake.DuplicatorTool.Handlers;

namespace Werecodent.CreateAndFake.Tests.DuplicatorTool.Handlers;

public static class SystemCopyHandlersTests
{
    [Fact]
    internal static void SystemCopyHandlers_InternalOnly()
    {
        typeof(SystemCopyHandlers).IsPublic.Assert().Is(false);
    }
}
