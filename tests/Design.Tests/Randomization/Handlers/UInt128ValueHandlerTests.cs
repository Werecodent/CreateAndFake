using Werecodent.CreateAndFake.Design.Randomization.Handlers;

namespace Werecodent.CreateAndFake.Design.Tests.Randomization.Handlers;

public static class UInt128ValueHandlerTests
{
    [Fact]
    internal static void UInt128ValueHandler_InternalOnly()
    {
        typeof(UInt128ValueHandler).IsPublic.Assert().Is(false);
    }

    [Fact]
    internal static void TryToCreate_NoException()
    {
        1.Assert(_ => UInt128ValueHandler.TryToCreate()).ThrowsNo<Exception>();
    }
}
