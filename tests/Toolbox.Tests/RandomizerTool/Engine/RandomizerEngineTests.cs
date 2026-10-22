using System.Collections.Frozen;
using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.RandomizerTool.Engine;

namespace Werecodent.CreateAndFake.Tests.RandomizerTool.Engine;

public static class RandomizerEngineTests
{
    private static readonly RandomizerEngine _TestInstance = new();

    [Fact]
    internal static Task RandomizerEngine_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            new RandomizerEngine(),
            TestContext.Current.CancellationToken,
            opt => opt with { MethodsToIgnore = FrozenSet.ToFrozenSet(["Inject"]) }
        );
    }

    [Fact]
    internal static Task RandomizerEngine_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            new RandomizerEngine(),
            TestContext.Current.CancellationToken,
            opt =>
                opt with
                {
                    InjectionValues = [Tools.Randomizer.Options],
                    MethodsToIgnore = FrozenSet.ToFrozenSet(["Inject"]),
                }
        );
    }

    [Theory, RandomData]
    internal static void Create_NoHintsUnsupported(Type type)
    {
        _TestInstance
            .Assert(x => x.Create(type, CreateHintChainer(null)))
            .Throws<UnsupportedException>();
    }

    [Theory, RandomData]
    internal static void Create_NullResultSafe([Stub] ICreateHint hint, Type type)
    {
        _TestInstance
            .Assert(x => x.Create(type, CreateHintChainer(hint)))
            .Throws<UnsupportedException>();
    }

    [Theory, RandomData]
    internal static void Inject_NoHintsUnsupported(Type type)
    {
        _TestInstance
            .Assert(x => x.Inject(type, null, CreateHintChainer(null)))
            .Throws<UnsupportedException>();
    }

    [Theory, RandomData]
    internal static void Inject_NullResultSafe([Stub] ICreateHint hint, Type type)
    {
        _TestInstance
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
