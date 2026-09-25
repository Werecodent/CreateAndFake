using Werecodent.CreateAndFake.Design.Randomization.Handlers;

namespace Werecodent.CreateAndFake.Design.Tests.Randomization.Handlers;

public static class FloatValueHandlerTests
{
    [Fact]
    internal static void FloatValueHandler_InternalOnly()
    {
        typeof(FloatValueHandler).IsPublic.Assert().Is(false);
    }
}
