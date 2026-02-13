using CreateAndFake.ValuerTool.Engine;

namespace CreateAndFake.Tests.ValuerTool.Engine;

public static class ExtractPriorityTests
{
    [Fact]
    internal static void Disabled_SetToMin()
    {
        ((int)ComparePriority.Disabled).Assert().Is(int.MinValue);
    }

    [Fact]
    internal static void None_DefaultAtZero()
    {
        ((int)ComparePriority.None).Assert().Is(0);
    }

    [Fact]
    internal static void Highest_AtCap()
    {
        ((int)ComparePriority.Highest)
            .Assert()
            .Is(Enum.GetValues(typeof(ComparePriority)).Length - 2);
    }

    [Fact]
    internal static void Values_AllPositive()
    {
        Enum.GetValues(typeof(ComparePriority))
            .Cast<int>()
            .Except([(int)ComparePriority.Disabled])
            .Where(v => v < 0)
            .Assert()
            .IsEmpty();
    }

    [Fact]
    internal static void Values_Unique()
    {
        Enum.GetValues(typeof(ComparePriority))
            .Cast<int>()
            .ToHashSet()
            .Assert()
            .HasCount(Enum.GetValues(typeof(ComparePriority)).Length);
    }
}
