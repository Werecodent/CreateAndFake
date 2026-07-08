using CreateAndFake.Design.Randomization.Handlers;

namespace CreateAndFake.Design.Tests.Randomization.Handlers;

public static class HalfValueHandlerTests
{
    [Fact]
    internal static void HalfValueHandler_InternalOnly()
    {
        typeof(HalfValueHandler).IsPublic.Assert().Is(false);
    }
}
