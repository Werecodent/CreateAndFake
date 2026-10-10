using Werecodent.CreateAndFake.ValuerTool.Handlers;

namespace Werecodent.CreateAndFake.Tests.ValuerTool.Handlers;

public static class ValueCompareHandlersTests
{
    [Fact]
    internal static void ValueCompareHandlers_InternalOnly()
    {
        typeof(ValueCompareHandlers).IsPublic.Assert().Is(false);
    }
}
