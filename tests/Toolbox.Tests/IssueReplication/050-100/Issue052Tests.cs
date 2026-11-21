using Werecodent.CreateAndFake.DuplicatorTool;
using Werecodent.CreateAndFake.DuplicatorTool.Engine;
using Werecodent.CreateAndFake.FakerTool;
using Werecodent.CreateAndFake.RandomizerTool;
using Werecodent.CreateAndFake.RandomizerTool.Engine;
using Werecodent.CreateAndFake.ValuerTool;
using Werecodent.CreateAndFake.ValuerTool.Engine;

namespace Werecodent.CreateAndFake.Tests.IssueReplication;

public static class Issue052Tests
{
    internal sealed class Data
    {
        public string Item { get; set; }
    }

    [Theory, RandomData]
    internal static void Issue052_RandomizerPostCustomizable(Fake<CreateHint> hint)
    {
        Randomizer randomizer = new(
            Tools.Randomizer.Options with
            {
                IncludeFrameworkHints = false,
            }
        );
        Data testItem = new() { Item = "Sample" };

        hint.Setup(
            d => d.TryToCreate(typeof(Data), Arg.Any<IRandomizerChainer>()),
            Behavior.Returns(new CreateHintResult(testItem), Times.Once)
        );

        randomizer
            .Create<Data>(opt => opt with { Hints = [hint.Dummy] })
            .Assert()
            .ReferenceEqual(testItem);
        hint.Verify();
    }

    [Theory, RandomData]
    internal static void Issue052_DuplicatorPostCustomizable(Fake<CopyHint> hint, Data item)
    {
        Duplicator duplicator = new(
            Tools.Duplicator.Options with
            {
                IncludeFrameworkHints = false,
            }
        );

        hint.Setup(
            d => d.TryCopy(item, Arg.Any<IDuplicatorChainer>()),
            Behavior.Returns(new CopyHintResult(item), Times.Once)
        );

        duplicator
            .Copy(item, opt => opt with { Hints = [hint.Dummy] })
            .Assert()
            .ReferenceEqual(item);
        hint.Verify();
    }

    [Theory, RandomData]
    internal static void Issue052_ValuerPostCustomizable(
        [Stub] ICompareHint hint,
        Data item1,
        Data item2
    )
    {
        Valuer valuer = new(
            Tools.Valuer.Options with
            {
                IncludeFrameworkHints = false,
                IncludeValueHashInComparison = false,
            }
        );

        hint.TryToCompare(item1, item2, Arg.Any<IValuerChainer>()).SetupReturn(new([]), Times.Once);

        valuer.Equals(item1, item2, opt => opt with { Hints = [hint] }).Assert().Is(true);
        hint.Assert().Called();
    }
}
