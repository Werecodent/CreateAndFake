using CreateAndFake.Design.Randomization.Handlers;

namespace CreateAndFake.Design.Tests.Randomization.Handlers;

public static class DecimalValueHandlerTests
{
    [Fact]
    internal static void DecimalValueHandler_InternalOnly()
    {
        typeof(DecimalValueHandler).IsPublic.Assert().Is(false);
    }
}
