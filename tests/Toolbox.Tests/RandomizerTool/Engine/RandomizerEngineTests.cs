using System.Collections.Frozen;
using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.RandomizerTool.Engine;

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
