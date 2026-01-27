using CreateAndFake.Design.Randomization.Handlers;

namespace CreateAndFake.Design.Tests.Randomization.Handlers;

public static class DoubleValueHandlerTests
{
    [Fact]
    internal static void DoubleValueHandler_InternalOnly()
    {
        typeof(DoubleValueHandler).IsPublic.Assert().Is(false);
    }
}
