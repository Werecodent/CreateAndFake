using Werecodent.CreateAndFake.Design.Randomization.Handlers;

namespace Werecodent.CreateAndFake.Design.Tests.Randomization.Handlers;

public static class BFloat16ValueHandlerTests
{
    [Fact]
    internal static void BFloat16ValueHandler_InternalOnly()
    {
        typeof(BFloat16ValueHandler).IsPublic.Assert().Is(false);
    }

    [Fact]
    internal static void TryToCreate_NoException()
    {
        1.Assert(_ => BFloat16ValueHandler.TryToCreate()).ThrowsNo<Exception>();
    }
}
