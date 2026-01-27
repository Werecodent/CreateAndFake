using CreateAndFake.Design.Randomization.Handlers;

namespace CreateAndFake.Design.Tests.Randomization.Handlers;

public static class FloatValueHandlerTests
{
    [Fact]
    internal static void FloatValueHandler_InternalOnly()
    {
        typeof(FloatValueHandler).IsPublic.Assert().Is(false);
    }
}
