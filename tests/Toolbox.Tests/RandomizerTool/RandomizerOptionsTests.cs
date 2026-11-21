global using RandomizerMod = System.Func<
    Werecodent.CreateAndFake.RandomizerTool.RandomizerOptions,
    Werecodent.CreateAndFake.RandomizerTool.RandomizerOptions
>;
using Werecodent.CreateAndFake.RandomizerTool;

namespace Werecodent.CreateAndFake.Tests.RandomizerTool;

public sealed class RandomizerOptionsTests : AutoTester<RandomizerOptions>
{
    protected override TesterMod OptionConfiguration { get; } =
        opt => opt with { IgnorableExceptions = [typeof(ArgumentNullException)] };

    protected override bool EqualsOverrideUsesFullValueEquality { get; } = false;

    [Fact]
    internal static void RandomizerOptions_ModFormRandomizable()
    {
        typeof(RandomizerMod).Tools().CreateRandomInstance().Assert().IsNotNull();
    }

    [Fact]
    internal static void NextStringSize_PreventsOverflow()
    {
        (
            Tools.Randomizer.Options with
            {
                StringMinSize = int.MaxValue,
                StringMaxSize = int.MaxValue,
            }
        )
            .NextStringSize()
            .Assert()
            .Is(int.MaxValue);
    }

    [Fact]
    internal static void NextStringSize_InvertsSizeWhenFlipped()
    {
        (Tools.Randomizer.Options with { StringMinSize = 1, StringMaxSize = 0 })
            .NextStringSize()
            .Assert()
            .GreaterThanOrEqualTo(0)
            .And()
            .LessThanOrEqualTo(1);
    }
}
