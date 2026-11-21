using System.Collections.Frozen;
using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.RandomizerTool.Engine;
using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Tests.RandomizerTool.Engine;

public sealed class RandomizerEngineTests : AutoTester<RandomizerEngine>
{
    protected override TesterMod OptionConfiguration { get; } =
        opt =>
            opt with
            {
                InjectionValues = [Tools.Randomizer.Options],
                MethodsToIgnore = FrozenSet.ToFrozenSet(["Inject"]),
            };

    [Theory, RandomData]
    internal void Create_NoHintsUnsupported(Type type)
    {
        TestInstance
            .Assert(x => x.Create(type, CreateHintChainer(null)))
            .Throws<UnsupportedException>();
    }

    [Theory, RandomData]
    internal void Create_NullResultSafe([Stub] ICreateHint hint, Type type)
    {
        TestInstance
            .Assert(x => x.Create(type, CreateHintChainer(hint)))
            .Throws<UnsupportedException>();
    }

    [Theory, RandomData]
    internal void Inject_NoHintsUnsupported(Type type)
    {
        TestInstance
            .Assert(x => x.Inject(type, null, CreateHintChainer(null)))
            .Throws<UnsupportedException>();
    }

    [Theory, RandomData]
    internal void Inject_NullResultSafe([Stub] ICreateHint hint, Type type)
    {
        TestInstance
            .Assert(x => x.Inject(type, [], CreateHintChainer(hint)))
            .Throws<UnsupportedException>();
    }

    [Fact]
    internal void Inject_LimitedIteration()
    {
        TestInstance
            .Assert(x =>
                x.Inject(
                    typeof(GoodOrBadSampleHolder<>),
                    [],
                    new RandomizerChainer(
                        Tools.Randomizer.Options with
                        {
                            Design = Tools.Randomizer.Options.Design with { IterationLimit = 0 },
                        },
                        new RandomizerEngine()
                    )
                )
            )
            .Throws<IterationLimitException>();
    }

    private static RandomizerChainer CreateHintChainer(ICreateHint hint)
    {
        return new RandomizerChainer(
            Tools.Randomizer.Options with
            {
                IncludeFoundHints = false,
                IncludeFrameworkHints = false,
                Hints = hint != null ? [hint] : [],
            },
            new RandomizerEngine()
        );
    }
}
