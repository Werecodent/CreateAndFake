using Werecodent.CreateAndFake.Design.Randomization.Handlers;

namespace Werecodent.CreateAndFake.Design.Tests.Randomization.Handlers;

public static class Decimal32ValueHandlerTests
{
    [Fact]
    internal static void Decimal32ValueHandler_InternalOnly()
    {
        typeof(Decimal32ValueHandler).IsPublic.Assert().Is(false);
    }

    [Fact]
    internal static void TryToCreate_NoException()
    {
        1.Assert(_ => Decimal32ValueHandler.TryToCreate()).ThrowsNo<Exception>();
    }
}
