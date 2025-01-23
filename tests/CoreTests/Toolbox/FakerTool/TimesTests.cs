using CreateAndFake.Toolbox.FakerTool;

namespace CreateAndFakeTests.Toolbox.FakerTool;

public static class TimesTests
{
    [Fact]
    internal static void Times_GuardsNulls()
    {
        Tools.Tester.PreventsNullRefException<Times>();
    }

    [Fact]
    internal static void Times_NoParameterMutation()
    {
        Tools.Tester.PreventsParameterMutation<Times>();
    }

    [Fact]
    internal static void Never_Works()
    {
        Times.Never.IsInRange(0).Assert().Is(true);
        Times.Never.IsInRange(1).Assert().Is(false);
        Times.Never.IsInRange(2).Assert().Is(false);
    }

    [Fact]
    internal static void Once_Works()
    {
        Times.Once.IsInRange(0).Assert().Is(false);
        Times.Once.IsInRange(1).Assert().Is(true);
        Times.Once.IsInRange(2).Assert().Is(false);
    }

    [Theory, RandomData]
    internal static void Exactly_Works(int value)
    {
        Times.Exactly(value).IsInRange(value - 1).Assert().Is(false);
        Times.Exactly(value).IsInRange(value).Assert().Is(true);
        Times.Exactly(value).IsInRange(value + 1).Assert().Is(false);
    }

    [Fact]
    internal static void Between_Works()
    {
        int min, max;
        do
        {
            min = Tools.Randomizer.Create<int>();
            max = Tools.Randomizer.Create<int>();
        } while (min >= max);

        Times.Between(min, max).IsInRange(min - 1).Assert().Is(false);
        Times.Between(min, max).IsInRange(min).Assert().Is(true);
        Times.Between(min, max).IsInRange(min + 1).Assert().Is(true);

        Times.Between(min, max).IsInRange(max - 1).Assert().Is(true);
        Times.Between(min, max).IsInRange(max).Assert().Is(true);
        Times.Between(min, max).IsInRange(max + 1).Assert().Is(false);
    }

    [Theory, RandomData]
    internal static void Min_Works(int value)
    {
        Times.Min(value).IsInRange(value - 1).Assert().Is(false);
        Times.Min(value).IsInRange(value).Assert().Is(true);
        Times.Min(value).IsInRange(value + 1).Assert().Is(true);
    }

    [Fact]
    internal static void Max_Works()
    {
        int value;
        do
        {
            value = Tools.Randomizer.Create<int>();
        } while (value <= 0);

        Times.Max(value).IsInRange(value - 1).Assert().Is(true);
        Times.Max(value).IsInRange(value).Assert().Is(true);
        Times.Max(value).IsInRange(value + 1).Assert().Is(false);
    }

    [Fact]
    internal static void ToString_Terse()
    {
        Times.Never.ToString().Assert().Is("0");
        Times.Once.ToString().Assert().Is("1");
        Times.Exactly(2).ToString().Assert().Is("2");
        Times.Max(1).ToString().Assert().Is("[0-1]");
        Times.Min(1).ToString().Assert().Is("[1-*]");
        Times.Between(1, 2).ToString().Assert().Is("[1-2]");
    }

    [Fact]
    internal static void Equality_MatchesValue()
    {
        int min, max;
        do
        {
            min = Tools.Randomizer.Create<int>();
            max = Tools.Randomizer.Create<int>();
        } while (min >= max);

        Times.Once.Equals(null).Assert().Is(false);
        Times.Exactly(min).Equals(Times.Between(min, min)).Assert().Is(true);
        Times.Exactly(max).Equals((object)Times.Between(max, max)).Assert().Is(true);
        Times.Exactly(min).Equals(Times.Between(min, max)).Assert().Is(false);
        Times.Exactly(max).Equals((object)Times.Between(min, max)).Assert().Is(false);

        Times.Between(min, min).GetHashCode().Assert().Is(Times.Exactly(min).GetHashCode());
        Times.Between(max, max).GetHashCode().Assert().Is(Times.Exactly(max).GetHashCode());
        Times.Between(min, max).GetHashCode().Assert().IsNot(Times.Exactly(min).GetHashCode());
        Times.Between(min, max).GetHashCode().Assert().IsNot(Times.Exactly(max).GetHashCode());
    }
}
