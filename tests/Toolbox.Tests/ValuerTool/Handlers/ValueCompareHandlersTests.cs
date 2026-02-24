using CreateAndFake.ValuerTool.Handlers;

namespace CreateAndFake.Tests.ValuerTool.Handlers;

public static class ValueCompareHandlersTests
{
    [Fact]
    internal static void ValueCompareHandlers_InternalOnly()
    {
        typeof(ValueCompareHandlers).IsPublic.Assert().Is(false);
    }
}
