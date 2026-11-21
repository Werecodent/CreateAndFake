using Werecodent.CreateAndFake.Design.Randomization.Handlers;

namespace Werecodent.CreateAndFake.Design.Tests.Randomization.Handlers;

public static class UIntPtrValueHandlerTests
{
    [Fact]
    internal static void UIntPtrValueHandler_InternalOnly()
    {
        typeof(UIntPtrValueHandler).IsPublic.Assert().Is(false);
    }

    [Theory, RandomData]
    internal static void CreateSupported_Size4IsInt([Cap(uint.MaxValue)] uint rawMax)
    {
        UIntPtr min = new(Tools.Gen.Next(rawMax));
        UIntPtr max = new(rawMax);

        UIntPtrValueHandler instance = new(4);
        instance
            .CreateSupported(Tools.Gen)
            .Assert()
            .IsA<UIntPtr>()
            .With(x => x.ToUInt32())
            .LessThanOrEqualTo(uint.MaxValue)
            .And()
            .GreaterThanOrEqualTo(uint.MinValue);

        instance
            .CreateSupported(Tools.Gen, min, max)
            .Assert()
            .IsA<UIntPtr>()
            .With(x => x.ToUInt32())
            .LessThanOrEqualTo(max.ToUInt32())
            .And()
            .GreaterThanOrEqualTo(min.ToUInt32());
    }

    [Theory, RandomData]
    internal static void CreateSupported_Size8IsLong([Cap(ulong.MaxValue)] ulong rawMax)
    {
        UIntPtr min = new(Tools.Gen.Next(rawMax));
        UIntPtr max = new(rawMax);

        UIntPtrValueHandler instance = new(8);
        instance
            .CreateSupported(Tools.Gen)
            .Assert()
            .IsA<UIntPtr>()
            .With(x => x.ToUInt64())
            .LessThanOrEqualTo(ulong.MaxValue)
            .And()
            .GreaterThanOrEqualTo(ulong.MinValue);

        instance
            .CreateSupported(Tools.Gen, min, max)
            .Assert()
            .IsA<UIntPtr>()
            .With(x => x.ToUInt64())
            .LessThanOrEqualTo(max.ToUInt64())
            .And()
            .GreaterThanOrEqualTo(min.ToUInt64());
    }
}
