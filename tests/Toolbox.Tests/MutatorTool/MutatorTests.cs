using CreateAndFake.Design.Reiteration;
using CreateAndFake.Design.Tooling;
using CreateAndFake.ExtractorTool;
using CreateAndFake.FakerTool;
using CreateAndFake.MutatorTool;
using CreateAndFake.Samples.Scenarios;
using CreateAndFake.ValuerTool;

namespace CreateAndFake.Tests.MutatorTool;

public static class MutatorTests
{
    private static readonly TesterMod config = opt =>
        opt with
        {
            IgnorableExceptions = [typeof(ToolException)],
        };

    [Fact]
    internal static Task Mutator_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<Mutator>(
            TestContext.Current.CancellationToken,
            config
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
        fakeValuer
            .Equals(Arg.Any<object>(), Arg.Any<object>(), Arg.Any<ValuerMod>())
            .SetupReturn(true);

        new Mutator(
            Tools.Mutator.Options with
            {
                Valuer = fakeValuer,
                VariantAttempts = new Limiter(3),
            }
        )
            .Assert(t => t.Variant(sample))
            .Throws<ToolException>();

        fakeValuer.Assert().Called();
    }

    [Theory, RandomData]
    internal static void Variant_RepeatsUntilUnequal([Fake] IValuer fakeValuer, DataSample sample)
    {
        fakeValuer
            .Equals(Arg.Any<object>(), Arg.Any<object>(), Arg.Any<ValuerMod>())
            .SetupCall(Behavior.Series(true, true, true, false));

        new Mutator(
            Tools.Mutator.Options with
            {
                Valuer = fakeValuer,
                VariantAttempts = new Limiter(5),
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
        fakeValuer
            .Equals(Arg.Any<object>(), Arg.Any<object>(), Arg.Any<ValuerMod>())
            .SetupCall(Behavior.Series(false, true, true, false, true, true, false, false));

        new Mutator(
            Tools.Mutator.Options with
            {
                Valuer = fakeValuer,
                VariantAttempts = new Limiter(5),
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
                VariantAttempts = new Limiter(5),
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
                VariantAttempts = new Limiter(5),
            }
        )
            .UniqueOf([sample1, sample2])
            .Assert()
            .IsNot(null);
    }

    [Theory, RandomData]
    internal static void Modify_DataChanged(DataHolderSample data)
    {
        DataHolderSample dupe = data.CreateDeepClone();

        Tools.Mutator.Modify(data).Assert().Is(true);

        data.Assert().IsNot(dupe);
    }

    [Theory, RandomData]
    internal static void Modify_StatelessUnchanged(StatelessSample data)
    {
        Tools.Mutator.Modify(data).Assert().Is(false);
    }
}
