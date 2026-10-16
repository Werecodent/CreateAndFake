using Werecodent.CreateAndFake.Design.Randomization.Handlers;

namespace Werecodent.CreateAndFake.Design.Tests.Randomization.Handlers;

public static class RuneValueHandlerTests
{
    [Fact]
    internal static void RuneValueHandler_InternalOnly()
    {
        typeof(RuneValueHandler).IsPublic.Assert().Is(false);
    }

    [Fact]
    internal static void TryToCreate_NoException()
    {
        1.Assert(_ => RuneValueHandler.TryToCreate()).ThrowsNo<Exception>();
    }
}
