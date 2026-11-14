using Werecodent.CreateAndFake.Design.Randomization.Handlers;

namespace Werecodent.CreateAndFake.Design.Tests.Randomization.Handlers;

public static class IntPtrValueHandlerTests
{
    [Fact]
    internal static void IntPtrValueHandler_InternalOnly()
    {
        typeof(IntPtrValueHandler).IsPublic.Assert().Is(false);
    }

    [Theory, RandomData]
    internal static void CreateSupported_Size4IsInt([Cap(int.MaxValue)] int rawMax)
    {
        IntPtr min = new(Tools.Gen.Next(rawMax));
        IntPtr max = new(rawMax);

        IntPtrValueHandler instance = new(4);
        instance
            .CreateSupported(Tools.Gen)
            .Assert()
            .IsA<IntPtr>()
            .With(x => x.ToInt32())
            .LessThanOrEqualTo(int.MaxValue)
            .And()
            .GreaterThanOrEqualTo(int.MinValue);

        instance
            .CreateSupported(Tools.Gen, min, max)
            .Assert()
            .IsA<IntPtr>()
            .With(x => x.ToInt32())
            .LessThanOrEqualTo(max.ToInt32())
            .And()
            .GreaterThanOrEqualTo(min.ToInt32());
    }

    [Theory, RandomData]
    internal static void CreateSupported_Size8IsLong([Cap(long.MaxValue)] long rawMax)
    {
        IntPtr min = new(Tools.Gen.Next(rawMax));
        IntPtr max = new(rawMax);

        IntPtrValueHandler instance = new(8);
        instance
            .CreateSupported(Tools.Gen)
            .Assert()
            .IsA<IntPtr>()
            .With(x => x.ToInt64())
            .LessThanOrEqualTo(long.MaxValue)
            .And()
            .GreaterThanOrEqualTo(long.MinValue);

        instance
            .CreateSupported(Tools.Gen, min, max)
            .Assert()
            .IsA<IntPtr>()
            .With(x => x.ToInt64())
            .LessThanOrEqualTo(max.ToInt64())
            .And()
            .GreaterThanOrEqualTo(min.ToInt64());
    }
}
