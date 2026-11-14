using Werecodent.CreateAndFake.Design.Randomization.Handlers;

namespace Werecodent.CreateAndFake.Design.Tests.Randomization.Handlers;

public static class Decimal64ValueHandlerTests
{
    [Fact]
    internal static void Decimal64ValueHandler_InternalOnly()
    {
        typeof(Decimal64ValueHandler).IsPublic.Assert().Is(false);
    }

    [Fact]
    internal static void TryToCreate_NoException()
    {
        1.Assert(_ => Decimal64ValueHandler.TryToCreate()).ThrowsNo<Exception>();
    }
}
