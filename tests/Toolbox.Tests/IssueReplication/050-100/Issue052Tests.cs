using CreateAndFake.DuplicatorTool;
using CreateAndFake.DuplicatorTool.Engine;
using CreateAndFake.FakerTool;
using CreateAndFake.RandomizerTool;
using CreateAndFake.RandomizerTool.Engine;
using CreateAndFake.ValuerTool;
using CreateAndFake.ValuerTool.Engine;

namespace CreateAndFake.Tests.IssueReplication;

public static class Issue052Tests
{
    internal sealed class Data
    {
        public string Item { get; set; }
    }

    [Theory, RandomData]
    internal static void Issue052_RandomizerPostCustomizable(Fake<CreateHint> hint)
    {
        Randomizer randomizer = new(Tools.Randomizer.Options with { IncludeDefaultHints = false });
        Data testItem = new() { Item = "Sample" };

        hint.Setup(
            d => d.TryCreate(typeof(Data), Arg.Any<IRandomizerChainer>()),
            Behavior.Returns(new CreateHintResult(testItem), Times.Once)
        );

        randomizer
            .Create<Data>(opt => opt with { Hints = [hint.Dummy] })
            .Assert()
            .ReferenceEqual(testItem);
        hint.VerifyAll();
    }

    [Theory, RandomData]
    internal static void Issue052_DuplicatorPostCustomizable(Fake<CopyHint> hint, Data item)
    {
        Duplicator duplicator = new(Tools.Duplicator.Options with { IncludeDefaultHints = false });

        hint.Setup(
            d => d.TryCopy(item, Arg.Any<IDuplicatorChainer>()),
            Behavior.Returns(new CopyHintResult(item), Times.Once)
        );

        duplicator
            .Copy(item, opt => opt with { Hints = [hint.Dummy] })
            .Assert()
            .ReferenceEqual(item);
        hint.VerifyAll();
    }

    [Theory, RandomData]
    internal static void Issue052_ValuerPostCustomizable(
        Fake<CompareHint> hint,
        Data item1,
        Data item2
    )
    {
        Valuer valuer = new(Tools.Valuer.Options with { IncludeDefaultHints = false });

        hint.Setup(
            "Supports",
            [item1, item2, Arg.LambdaAny<IValuerChainer>()],
            Behavior.Returns(true, Times.Once)
        );
        hint.Setup(
            "Compare",
            [item1, item2, Arg.LambdaAny<IValuerChainer>()],
            Behavior.Returns(Enumerable.Empty<Difference>(), Times.Once)
        );

        valuer.Equals(item1, item2, opt => opt with { Hints = [hint.Dummy] }).Assert().Is(true);
        hint.VerifyAll();
    }
}
