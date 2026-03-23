using CreateAndFake.Design.Randomization.Handlers;

namespace CreateAndFake.Design.Tests.Randomization.Handlers;

public static class DateTimeOffsetValueHandlerTests
{
    [Fact]
    internal static void DateTimeOffsetValueHandler_InternalOnly()
    {
        typeof(DateTimeOffsetValueHandler).IsPublic.Assert().Is(false);
    }
}
