using Werecodent.CreateAndFake.FakerTool.Engine;

namespace Werecodent.CreateAndFake.Tests.FakerTool.Engine;

public static class FakePriorityTests
{
    [Fact]
    internal static void Disabled_SetToMin()
    {
        ((int)FakePriority.Disabled).Assert().Is(int.MinValue);
    }

    [Fact]
    internal static void None_DefaultAtZero()
    {
        ((int)FakePriority.None).Assert().Is(0);
    }

    [Fact]
    internal static void Highest_AtCap()
    {
        ((int)FakePriority.Highest).Assert().Is(Enum.GetValues(typeof(FakePriority)).Length - 2);
    }

    [Fact]
    internal static void Values_AllPositive()
    {
        Enum.GetValues(typeof(FakePriority))
            .Cast<int>()
            .Except([(int)FakePriority.Disabled])
            .Where(v => v < 0)
            .Assert()
            .IsEmpty();
    }

    [Fact]
    internal static void Values_Unique()
    {
        Enum.GetValues(typeof(FakePriority))
            .Cast<int>()
            .ToHashSet()
            .Assert()
            .HasCount(Enum.GetValues(typeof(FakePriority)).Length);
    }
}
