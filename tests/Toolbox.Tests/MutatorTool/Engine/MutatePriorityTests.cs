using CreateAndFake.MutatorTool.Engine;

namespace CreateAndFake.Tests.MutatorTool.Engine;

public static class MutatePriorityTests
{
    [Fact]
    internal static void Disabled_SetToMin()
    {
        ((int)MutatePriority.Disabled).Assert().Is(int.MinValue);
    }

    [Fact]
    internal static void None_DefaultAtZero()
    {
        ((int)MutatePriority.None).Assert().Is(0);
    }

    [Fact]
    internal static void Highest_AtCap()
    {
        ((int)MutatePriority.Highest).Assert().Is(Enum.GetValues<MutatePriority>().Length - 2);
    }

    [Fact]
    internal static void Values_AllPositive()
    {
        Enum.GetValues<MutatePriority>()
            .Except([MutatePriority.Disabled])
            .Cast<int>()
            .Where(v => v < 0)
            .Assert()
            .IsEmpty();
    }

    [Fact]
    internal static void Values_Unique()
    {
        Enum.GetValues<MutatePriority>()
            .Cast<int>()
            .ToHashSet()
            .Assert()
            .HasCount(Enum.GetValues<MutatePriority>().Length);
    }
}
