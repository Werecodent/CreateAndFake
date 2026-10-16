using Werecodent.CreateAndFake.DuplicatorTool.Engine;

namespace Werecodent.CreateAndFake.Tests.DuplicatorTool.Engine;

public static class CopyPriorityTests
{
    [Fact]
    internal static void Disabled_SetToMin()
    {
        ((int)CopyPriority.Disabled).Assert().Is(int.MinValue);
    }

    [Fact]
    internal static void None_DefaultAtZero()
    {
        ((int)CopyPriority.None).Assert().Is(0);
    }

    [Fact]
    internal static void Highest_AtCap()
    {
        ((int)CopyPriority.Highest).Assert().Is(Enum.GetValues(typeof(CopyPriority)).Length - 2);
    }

    [Fact]
    internal static void Values_AllPositive()
    {
        Enum.GetValues(typeof(CopyPriority))
            .Cast<int>()
            .Except([(int)CopyPriority.Disabled])
            .Where(v => v < 0)
            .Assert()
            .IsEmpty();
    }

    [Fact]
    internal static void Values_Unique()
    {
        Enum.GetValues(typeof(CopyPriority))
            .Cast<int>()
            .ToHashSet()
            .Assert()
            .HasCount(Enum.GetValues(typeof(CopyPriority)).Length);
    }
}
