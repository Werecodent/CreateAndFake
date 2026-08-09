using Werecodent.CreateAndFake.Design.Randomization.Handlers;

namespace Werecodent.CreateAndFake.Design.Tests.Randomization.Handlers;

public static class BoolValueHandlerTests
{
    [Fact]
    internal static void BoolValueHandler_InternalOnly()
    {
        typeof(BoolValueHandler).IsPublic.Assert().Is(false);
    }
}
