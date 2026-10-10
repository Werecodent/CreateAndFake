using Werecodent.CreateAndFake.Design.Randomization.Handlers;

namespace Werecodent.CreateAndFake.Design.Tests.Randomization.Handlers;

public static class DateTimeValueHandlerTests
{
    [Fact]
    internal static void DateTimeValueHandler_InternalOnly()
    {
        typeof(DateTimeValueHandler).IsPublic.Assert().Is(false);
    }
}
