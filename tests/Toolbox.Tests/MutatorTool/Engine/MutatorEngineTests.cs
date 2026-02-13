using CreateAndFake.Design.Exceptions;
using CreateAndFake.Design.Reiteration;
using CreateAndFake.ExtractorTool;
using CreateAndFake.FakerTool;
using CreateAndFake.MutatorTool;
using CreateAndFake.MutatorTool.Engine;
using CreateAndFake.Samples.Scenarios;
using CreateAndFake.ValuerTool;

namespace CreateAndFake.Tests.MutatorTool.Engine;

public static class MutatorEngineTests
{
    private static readonly MutatorEngine _TestInstance = new();

    [Fact]
    internal static Task MutatorEngine_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<MutatorEngine>(
            TestContext.Current.CancellationToken
        );
    }

    [Theory, RandomData]
    internal static void Variant_AcceptsNull(string value)
    {
        Tools.Mutator.Variant<string>(null).Assert().IsNot(null);
        Tools.Mutator.Variant(value, null).Assert().IsNot(value).And.IsNot(null);
    }

    [Theory, RandomData]
    internal static void VariantOf_ManyValuesWorks([Size(10000)] int[] data)
    {
        int result = Tools.Mutator.VariantOf(data);
        data.Assert().ContainsNot(result);
    }

    [Theory, RandomData]
    internal static void Variant_TimesOut([Fake] IValuer fakeValuer, DataSample sample)
    {
        fakeValuer.Options.SetupReturn(Tools.Valuer.Options);
        fakeValuer
            .Equals(Arg.Any<object>(), Arg.Any<object>(), Arg.Any<ValuerMod>())
            .SetupReturn(true);

        new Mutator(
            Tools.Mutator.Options with
            {
                Valuer = fakeValuer,
                CreateVariantAttemptLimit = new Limiter(3),
            }
        )
            .Assert(t => t.Variant(sample))
            .Throws<ToolException>();

        fakeValuer.Assert().Called();
    }

    [Theory, RandomData]
    internal static void Variant_RepeatsUntilUnequal([Fake] IValuer fakeValuer, DataSample sample)
    {
        fakeValuer.Options.SetupReturn(Tools.Valuer.Options);
        fakeValuer
            .Equals(Arg.Any<object>(), Arg.Any<object>(), Arg.Any<ValuerMod>())
            .SetupCall(Behavior.Series(true, true, true, false));

        new Mutator(
            Tools.Mutator.Options with
            {
                Valuer = fakeValuer,
                CreateVariantAttemptLimit = new Limiter(5),
            }
        )
            .Variant(sample)
            .Assert()
            .IsNot(null);

        fakeValuer.Assert().Called();
    }

    [Theory, RandomData]
    internal static void Variant_RepeatsUntilBothUnequal(
        [Fake] IValuer fakeValuer,
        DataSample sample1,
        DataSample sample2
    )
    {
        fakeValuer.Options.SetupReturn(Tools.Valuer.Options);
        fakeValuer
            .Equals(Arg.Any<object>(), Arg.Any<object>(), Arg.Any<ValuerMod>())
            .SetupCall(Behavior.Series(false, true, true, false, true, true, false, false));

        new Mutator(
            Tools.Mutator.Options with
            {
                Valuer = fakeValuer,
                CreateVariantAttemptLimit = new Limiter(5),
            }
        )
            .VariantOf([sample1, sample2])
            .Assert()
            .IsNot(null);

        fakeValuer.Assert().Called();
    }

    [Theory, RandomData]
    internal static void Unique_AcceptsNull(string value)
    {
        Tools.Mutator.Unique<string>(null).Assert().IsNot(null);
        Tools.Mutator.Unique(value, null).Assert().IsNot(value).And.IsNot(null);
    }

    [Theory, RandomData]
    internal static void UniqueOf_ManyValuesWorks([Size(100)] int[] data)
    {
        int result = Tools.Mutator.UniqueOf(data);
        data.Assert().ContainsNot(result);
    }

    [Theory, RandomData]
    internal static void Unique_TimesOut([Fake] IValuer fakeValuer, DataSample sample)
    {
        fakeValuer.Equals(Arg.Any<object>(), Arg.Any<object>()).SetupReturn(true);
        fakeValuer.GetHashCode(Arg.Any<object>()).SetupReturn(0);

        new Mutator(
            Tools.Mutator.Options with
            {
                Valuer = fakeValuer,
                Extractor = new Extractor(Tools.Extractor.Options with { Valuer = fakeValuer }),
            }
        )
            .Assert(t => t.Unique(sample))
            .Throws<ToolException>();
    }

    [Theory, RandomData]
    internal static void Unique_RepeatsUntilUnequal([Fake] IValuer fakeValuer, string sample)
    {
        fakeValuer
            .Equals(Arg.Any<object>(), Arg.Any<object>())
            .SetupCall(Behavior.Series(true, true, true, false));
        fakeValuer.GetHashCode(Arg.Any<object>()).SetupReturn(0);

        new Mutator(
            Tools.Mutator.Options with
            {
                Valuer = fakeValuer,
                CreateUniqueAttemptLimit = new Limiter(5),
            }
        )
            .Unique(sample)
            .Assert()
            .IsNot(null);
    }

    [Theory, RandomData]
    internal static void Unique_RepeatsUntilBothUnequal(
        [Fake] IValuer fakeValuer,
        string sample1,
        string sample2
    )
    {
        fakeValuer
            .Equals(Arg.Any<object>(), Arg.Any<object>())
            .SetupCall(Behavior.Series(false, true, true, false, true, true, false, false));
        fakeValuer.GetHashCode(Arg.Any<object>()).SetupReturn(0);

        new Mutator(
            Tools.Mutator.Options with
            {
                Valuer = fakeValuer,
                CreateUniqueAttemptLimit = new Limiter(5),
            }
        )
            .UniqueOf([sample1, sample2])
            .Assert()
            .IsNot(null);
    }

    [Theory, RandomData]
    internal static void Modify_NoHintsUnsupported(object data)
    {
        _TestInstance
            .Assert(d => d.Modify(data, CreateHintChainer(null)))
            .Throws<NotSupportedException>();
    }

    [Theory, RandomData]
    internal static void Modify_NullResultSafe([Stub] IMutateHint hint, object data)
    {
        _TestInstance
            .Assert(d => d.Modify(data, CreateHintChainer(hint)))
            .Throws<NotSupportedException>();
    }

    [Theory, RandomData]
    internal static void Modify_WrapsError([Stub] IMutateHint hint, object data)
    {
        hint.TryModifying(data, Arg.Any<IMutatorChainer>())
            .SetupCall(Behavior<MutateHintResult>.Throw<InvalidOperationException>());

        _TestInstance.Assert(d => d.Modify(data, CreateHintChainer(hint))).Throws<ToolException>();
    }

    private static MutatorChainer CreateHintChainer(IMutateHint hint)
    {
        return new MutatorChainer(
            Tools.Mutator.Options with
            {
                IncludeFoundHints = false,
                IncludeFrameworkHints = false,
                Hints = hint != null ? [hint] : [],
            },
            new MutatorEngine()
        );
    }
}
