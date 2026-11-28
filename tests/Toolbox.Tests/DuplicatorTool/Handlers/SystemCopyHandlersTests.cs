using Werecodent.CreateAndFake.DuplicatorTool.Handlers;

namespace Werecodent.CreateAndFake.Tests.DuplicatorTool.Handlers;

public sealed class SystemCopyHandlersTests() : AutoTester(typeof(SystemCopyHandlers))
{
    [Fact]
    internal static void SystemCopyHandlers_InternalOnly()
    {
        typeof(SystemCopyHandlers).IsPublic.Assert().Is(false);
    }
}
