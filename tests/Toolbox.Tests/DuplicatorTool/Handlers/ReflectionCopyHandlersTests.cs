using Werecodent.CreateAndFake.DuplicatorTool.Handlers;

namespace Werecodent.CreateAndFake.Tests.DuplicatorTool.Handlers;

public static class ReflectionCopyHandlersTests
{
    [Fact]
    internal static void ReflectionCopyHandlers_InternalOnly()
    {
        typeof(ReflectionCopyHandlers).IsPublic.Assert().Is(false);
    }
}
