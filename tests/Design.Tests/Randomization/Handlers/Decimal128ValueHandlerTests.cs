using Werecodent.CreateAndFake.Design.Randomization.Handlers;

namespace Werecodent.CreateAndFake.Design.Tests.Randomization.Handlers;

public static class Decimal128ValueHandlerTests
{
    [Fact]
    internal static void Decimal128ValueHandler_InternalOnly()
    {
        typeof(Decimal128ValueHandler).IsPublic.Assert().Is(false);
    }

    [Fact]
    internal static void TryToCreate_NoException()
    {
        1.Assert(_ => Decimal128ValueHandler.TryToCreate()).ThrowsNo<Exception>();
    }
}
