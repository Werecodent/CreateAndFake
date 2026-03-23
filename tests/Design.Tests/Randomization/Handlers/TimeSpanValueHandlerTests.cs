using CreateAndFake.Design.Randomization.Handlers;

namespace CreateAndFake.Design.Tests.Randomization.Handlers;

public static class TimeSpanValueHandlerTests
{
    [Fact]
    internal static void TimeSpanValueHandler_InternalOnly()
    {
        typeof(TimeSpanValueHandler).IsPublic.Assert().Is(false);
    }
}
