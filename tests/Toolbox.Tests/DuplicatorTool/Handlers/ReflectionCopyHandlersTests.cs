using Werecodent.CreateAndFake.DuplicatorTool.Handlers;

namespace Werecodent.CreateAndFake.Tests.DuplicatorTool.Handlers;

public sealed class ReflectionCopyHandlersTests() : AutoTester(typeof(ReflectionCopyHandlers))
{
    [Fact]
    internal static void ReflectionCopyHandlers_InternalOnly()
    {
        typeof(ReflectionCopyHandlers).IsPublic.Assert().Is(false);
    }
}
