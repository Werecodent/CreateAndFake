using Werecodent.CreateAndFake.Design.Randomization.Handlers;
#if NET6_0_OR_GREATER
using System.Runtime.InteropServices;
#endif

namespace Werecodent.CreateAndFake.Design.Tests.Randomization.Handlers;

public static class NFloatValueHandlerTests
{
    [Fact]
    internal static void NFloatValueHandler_InternalOnly()
    {
        typeof(NFloatValueHandler).IsPublic.Assert().Is(false);
    }

    [Fact]
    internal static void TryToCreate_NoException()
    {
        1.Assert(_ => NFloatValueHandler.TryToCreate()).ThrowsNo<Exception>();
    }

#if NET6_0_OR_GREATER
    [Theory, RandomData]
    internal static void CreateSupported_Size4IsFloat([Cap(float.MaxValue)] float rawMax)
    {
        NFloat min = new(Tools.Gen.Next(rawMax));
        NFloat max = new(rawMax);

        NFloatValueHandler instance = NFloatValueHandler.TryToCreate(4);
        instance
            .CreateSupported(Tools.Gen)
            .Assert()
            .IsA<NFloat>()
            .With(x => x)
            .LessThanOrEqualTo((NFloat)float.MaxValue)
            .And()
            .GreaterThanOrEqualTo((NFloat)float.MinValue);

        instance
            .CreateSupported(Tools.Gen, min, max)
            .Assert()
            .IsA<NFloat>()
            .With(x => x)
            .LessThanOrEqualTo(max)
            .And()
            .GreaterThanOrEqualTo(min);
    }

    [Theory, RandomData]
    internal static void CreateSupported_Size8IsDouble([Cap(double.MaxValue)] double rawMax)
    {
        NFloat min = new(Tools.Gen.Next(rawMax));
        NFloat max = new(rawMax);

        NFloatValueHandler instance = NFloatValueHandler.TryToCreate(8);
        instance
            .CreateSupported(Tools.Gen)
            .Assert()
            .IsA<NFloat>()
            .With(x => x)
            .LessThanOrEqualTo((NFloat)double.MaxValue)
            .And()
            .GreaterThanOrEqualTo((NFloat)double.MinValue);

        instance
            .CreateSupported(Tools.Gen, min, max)
            .Assert()
            .IsA<NFloat>()
            .With(x => x)
            .LessThanOrEqualTo(max)
            .And()
            .GreaterThanOrEqualTo(min);
    }
#endif
}
