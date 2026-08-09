using Werecodent.CreateAndFake.Design.Randomization.Handlers;

namespace Werecodent.CreateAndFake.Design.Tests.Randomization.Handlers;

public static class TimeOnlyValueHandlerTests
{
    [Fact]
    internal static void TimeOnlyValueHandler_InternalOnly()
    {
        typeof(TimeOnlyValueHandler).IsPublic.Assert().Is(false);
    }

    [Fact]
    internal static void TryToCreate_NoException()
    {
        1.Assert(_ => TimeOnlyValueHandler.TryToCreate()).ThrowsNo<Exception>();
    }
}
