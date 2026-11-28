using Werecodent.CreateAndFake.DuplicatorTool.Handlers;

namespace Werecodent.CreateAndFake.Tests.DuplicatorTool.Handlers;

public sealed class ValueCopyHandlersTests() : AutoTester(typeof(ValueCopyHandlers))
{
    [Fact]
    internal static void ValueCopyHandlers_InternalOnly()
    {
        typeof(ValueCopyHandlers).IsPublic.Assert().Is(false);
    }
}
