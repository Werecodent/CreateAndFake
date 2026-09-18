using Werecodent.CreateAndFake.ValuerTool.Handlers;

namespace Werecodent.CreateAndFake.Tests.ValuerTool.Handlers;

public static class ReflectionCompareHandlersTests
{
    [Fact]
    internal static void ReflectionCompareHandlers_InternalOnly()
    {
        typeof(ReflectionCompareHandlers).IsPublic.Assert().Is(false);
    }
}
