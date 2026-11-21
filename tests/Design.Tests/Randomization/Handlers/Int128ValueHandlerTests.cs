using Werecodent.CreateAndFake.Design.Randomization.Handlers;

namespace Werecodent.CreateAndFake.Design.Tests.Randomization.Handlers;

public static class Int128ValueHandlerTests
{
    [Fact]
    internal static void Int128ValueHandler_InternalOnly()
    {
        typeof(Int128ValueHandler).IsPublic.Assert().Is(false);
    }

    [Fact]
    internal static void TryToCreate_NoException()
    {
        1.Assert(_ => Int128ValueHandler.TryToCreate()).ThrowsNo<Exception>();
    }
}
