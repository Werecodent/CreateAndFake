using Werecodent.CreateAndFake.Design.Randomization.Handlers;

namespace Werecodent.CreateAndFake.Design.Tests.Randomization.Handlers;

public static class HalfValueHandlerTests
{
    [Fact]
    internal static void HalfValueHandler_InternalOnly()
    {
        typeof(HalfValueHandler).IsPublic.Assert().Is(false);
    }

    [Fact]
    internal static void TryToCreate_NoException()
    {
        1.Assert(_ => HalfValueHandler.TryToCreate()).ThrowsNo<Exception>();
    }
}
