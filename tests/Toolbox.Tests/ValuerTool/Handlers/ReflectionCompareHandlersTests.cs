using CreateAndFake.ValuerTool.Handlers;

namespace CreateAndFake.Tests.ValuerTool.Handlers;

public static class ReflectionCompareHandlersTests
{
    [Fact]
    internal static void ReflectionCompareHandlers_InternalOnly()
    {
        typeof(ReflectionCompareHandlers).IsPublic.Assert().Is(false);
    }
}
