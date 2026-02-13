using CreateAndFake.ExtractorTool.Engine;

namespace CreateAndFake.Tests.ExtractorTool.Engine;

public static class ExtractPriorityTests
{
    [Fact]
    internal static void Disabled_SetToMin()
    {
        ((int)ExtractPriority.Disabled).Assert().Is(int.MinValue);
    }

    [Fact]
    internal static void None_DefaultAtZero()
    {
        ((int)ExtractPriority.None).Assert().Is(0);
    }

    [Fact]
    internal static void Highest_AtCap()
    {
        ((int)ExtractPriority.Highest).Assert().Is(Enum.GetValues<ExtractPriority>().Length - 2);
    }

    [Fact]
    internal static void Values_AllPositive()
    {
        Enum.GetValues<ExtractPriority>()
            .Except([ExtractPriority.Disabled])
            .Cast<int>()
            .Where(v => v < 0)
            .Assert()
            .IsEmpty();
    }

    [Fact]
    internal static void Values_Unique()
    {
        Enum.GetValues<ExtractPriority>()
            .Cast<int>()
            .ToHashSet()
            .Assert()
            .HasCount(Enum.GetValues<ExtractPriority>().Length);
    }
}
