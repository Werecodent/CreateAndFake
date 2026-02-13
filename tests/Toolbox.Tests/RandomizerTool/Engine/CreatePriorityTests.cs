using CreateAndFake.RandomizerTool.Engine;

namespace CreateAndFake.Tests.RandomizerTool.Engine;

public static class CreatePriorityTests
{
    [Fact]
    internal static void Disabled_SetToMin()
    {
        ((int)CreatePriority.Disabled).Assert().Is(int.MinValue);
    }

    [Fact]
    internal static void None_DefaultAtZero()
    {
        ((int)CreatePriority.None).Assert().Is(0);
    }

    [Fact]
    internal static void Highest_AtCap()
    {
        ((int)CreatePriority.Highest).Assert().Is(Enum.GetValues<CreatePriority>().Length - 2);
    }

    [Fact]
    internal static void Values_AllPositive()
    {
        Enum.GetValues<CreatePriority>()
            .Except([CreatePriority.Disabled])
            .Cast<int>()
            .Where(v => v < 0)
            .Assert()
            .IsEmpty();
    }

    [Fact]
    internal static void Values_Unique()
    {
        Enum.GetValues<CreatePriority>()
            .Cast<int>()
            .ToHashSet()
            .Assert()
            .HasCount(Enum.GetValues<CreatePriority>().Length);
    }
}
