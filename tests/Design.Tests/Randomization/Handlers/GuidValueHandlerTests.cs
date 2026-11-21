using Werecodent.CreateAndFake.Design.Randomization.Handlers;

namespace Werecodent.CreateAndFake.Design.Tests.Randomization.Handlers;

public static class GuidValueHandlerTests
{
    [Fact]
    internal static void GuidValueHandler_InternalOnly()
    {
        typeof(GuidValueHandler).IsPublic.Assert().Is(false);
    }

    [Theory, RandomData]
    internal static void ToBigInteger_RoundTripsWithToGuid(Guid value)
    {
        GuidValueHandler.ToGuid(GuidValueHandler.ToBigInteger(value)).Assert().Is(value);
    }
}
