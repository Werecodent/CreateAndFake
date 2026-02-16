using CreateAndFake.Design.Randomization.Handlers;

namespace CreateAndFake.Design.Tests.Randomization.Handlers;

public static class FactoryValueHandlerTests
{
    [Fact]
    internal static void FactoryValueHandler_InternalOnly()
    {
        typeof(FactoryValueHandler<>).IsPublic.Assert().Is(false);
    }
}
