using CreateAndFake.Design.Randomization.Handlers;

namespace CreateAndFake.Design.Tests.Randomization.Handlers;

public static class TimeOnlyValueHandlerTests
{
    [Fact]
    internal static void TimeOnlyValueHandler_InternalOnly()
    {
        typeof(TimeOnlyValueHandler).IsPublic.Assert().Is(false);
    }
}
