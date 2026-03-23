using CreateAndFake.Design.Randomization.Handlers;

namespace CreateAndFake.Design.Tests.Randomization.Handlers;

public static class RuneValueHandlerTests
{
    [Fact]
    internal static void RuneValueHandler_InternalOnly()
    {
        typeof(RuneValueHandler).IsPublic.Assert().Is(false);
    }
}
