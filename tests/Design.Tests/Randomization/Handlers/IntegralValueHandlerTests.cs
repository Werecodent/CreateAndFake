using CreateAndFake.Design.Randomization.Handlers;

namespace CreateAndFake.Design.Tests.Randomization.Handlers;

public static class IntegralValueHandlerTests
{
    [Fact]
    internal static void IntegralValueHandler_InternalOnly()
    {
        typeof(IntegralValueHandler<>).IsPublic.Assert().Is(false);
    }
}
