using Werecodent.CreateAndFake.Design.Randomization.Handlers;

namespace Werecodent.CreateAndFake.Design.Tests.Randomization.Handlers;

public static class DecimalValueHandlerTests
{
    [Fact]
    internal static void DecimalValueHandler_InternalOnly()
    {
        typeof(DecimalValueHandler).IsPublic.Assert().Is(false);
    }
}
