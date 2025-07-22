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
    [Fact]
    internal static Task Mutator_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<Mutator>();
    }

    [Theory, RandomData]
    internal static void Variant_AcceptsNull(string value)
    {
        Tools.Mutator.Variant<string>(null).Assert().IsNot(null);
        Tools.Mutator.Variant(value, null).Assert().IsNot(value).And.IsNot(null);
    }

    [Theory, RandomData]
    internal static void Variant_ManyValuesWorks(int value, [Size(10000)] int[] data)
    {
        int result = Tools.Mutator.Variant(value, data);
        result.Assert().IsNot(value).Also(data).ContainsNot(result);
    }

    [Theory, RandomData]
    internal static void Variant_TimesOut([Fake] IValuer fakeValuer, DataSample sample)
    {
        fakeValuer
            .Equals(Arg.Any<object>(), Arg.Any<object>(), Arg.Any<ValuerMod>())
            .SetupReturn(true);

        new Mutator(Tools.Mutator.Options with { Valuer = fakeValuer, Limiter = new Limiter(3) })
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

        new Mutator(Tools.Mutator.Options with { Valuer = fakeValuer, Limiter = new Limiter(5) })
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

        new Mutator(Tools.Mutator.Options with { Valuer = fakeValuer, Limiter = new Limiter(5) })
            .Variant(sample1, sample2)
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
    internal static void Unique_ManyValuesWorks(int value, [Size(10000)] int[] data)
    {
        int result = Tools.Mutator.Unique(value, data);
        result.Assert().IsNot(value).Also(data).ContainsNot(result);
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
                Extractor = new Extractor(
                    Tools.Extractor.Options with
                    {
                        Valuer = fakeValuer,
                        Limiter = new Limiter(3),
                    }
                ),
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

        new Mutator(Tools.Mutator.Options with { Valuer = fakeValuer, Limiter = new Limiter(5) })
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

        new Mutator(Tools.Mutator.Options with { Valuer = fakeValuer, Limiter = new Limiter(5) })
            .Unique(sample1, sample2)
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
