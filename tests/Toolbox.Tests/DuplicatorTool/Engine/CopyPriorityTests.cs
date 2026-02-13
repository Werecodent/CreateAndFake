using CreateAndFake.DuplicatorTool.Engine;

namespace CreateAndFake.Tests.DuplicatorTool.Engine;

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
        ((int)CopyPriority.Highest).Assert().Is(Enum.GetValues<CopyPriority>().Length - 2);
    }

    [Fact]
    internal static void Values_AllPositive()
    {
        Enum.GetValues<CopyPriority>()
            .Except([CopyPriority.Disabled])
            .Cast<int>()
            .Where(v => v < 0)
            .Assert()
            .IsEmpty();
    }

    [Fact]
    internal static void Values_Unique()
    {
        Enum.GetValues<CopyPriority>()
            .Cast<int>()
            .ToHashSet()
            .Assert()
            .HasCount(Enum.GetValues<CopyPriority>().Length);
    }
}
