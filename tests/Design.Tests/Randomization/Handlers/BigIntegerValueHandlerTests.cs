using Werecodent.CreateAndFake.Design.Randomization.Handlers;

namespace Werecodent.CreateAndFake.Design.Tests.Randomization.Handlers;

public static class BigIntegerValueHandlerTests
{
    [Fact]
    internal static void BigIntegerValueHandler_InternalOnly()
    {
        typeof(BigIntegerValueHandler).IsPublic.Assert().Is(false);
    }
}
