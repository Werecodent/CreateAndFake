using Werecodent.CreateAndFake.ValuerTool.Handlers;

namespace Werecodent.CreateAndFake.Tests.ValuerTool.Handlers;

public sealed class DefaultEqualityCompareHandlerTests()
    : AutoTester(typeof(DefaultEqualityCompareHandler))
{
    [Fact]
    internal static void DefaultEqualityCompareHandler_InternalOnly()
    {
        typeof(DefaultEqualityCompareHandler).IsPublic.Assert().Is(false);
    }
}
