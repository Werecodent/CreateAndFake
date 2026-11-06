using Werecodent.CreateAndFake.Design.Randomization.Handlers;

namespace Werecodent.CreateAndFake.Design.Tests.Randomization.Handlers;

public static class DateOnlyValueHandlerTests
{
    [Fact]
    internal static void DateOnlyValueHandler_InternalOnly()
    {
        typeof(DateOnlyValueHandler).IsPublic.Assert().Is(false);
    }

    [Fact]
    internal static void TryToCreate_NoException()
    {
        1.Assert(_ => DateOnlyValueHandler.TryToCreate()).ThrowsNo<Exception>();
    }
}
