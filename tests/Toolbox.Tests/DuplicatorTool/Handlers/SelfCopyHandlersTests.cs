using Werecodent.CreateAndFake.DuplicatorTool.Handlers;

namespace Werecodent.CreateAndFake.Tests.DuplicatorTool.Handlers;

public sealed class SelfCopyHandlersTests() : AutoTester(typeof(SelfCopyHandlers))
{
    [Fact]
    internal static void SelfCopyHandlers_InternalOnly()
    {
        typeof(SelfCopyHandlers).IsPublic.Assert().Is(false);
    }
}
