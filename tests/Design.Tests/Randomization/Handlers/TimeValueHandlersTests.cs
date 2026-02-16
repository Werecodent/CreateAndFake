using CreateAndFake.Design.Randomization.Handlers;

namespace CreateAndFake.Design.Tests.Randomization.Handlers;

public static class TimeValueHandlersTests
{
    [Fact]
    internal static void TimeValueHandlers_InternalOnly()
    {
        typeof(TimeValueHandlers).IsPublic.Assert().Is(false);
    }
}
