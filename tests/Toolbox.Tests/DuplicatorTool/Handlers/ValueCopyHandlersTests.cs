using CreateAndFake.DuplicatorTool.Handlers;

namespace CreateAndFake.Tests.DuplicatorTool.Handlers;

public static class ValueCopyHandlersTests
{
    [Fact]
    internal static void ValueCopyHandlers_InternalOnly()
    {
        typeof(ValueCopyHandlers).IsPublic.Assert().Is(false);
    }
}
