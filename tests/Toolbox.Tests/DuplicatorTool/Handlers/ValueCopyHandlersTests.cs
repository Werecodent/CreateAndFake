using Werecodent.CreateAndFake.DuplicatorTool.Handlers;

namespace Werecodent.CreateAndFake.Tests.DuplicatorTool.Handlers;

public static class ValueCopyHandlersTests
{
    [Fact]
    internal static void ValueCopyHandlers_InternalOnly()
    {
        typeof(ValueCopyHandlers).IsPublic.Assert().Is(false);
    }
}
