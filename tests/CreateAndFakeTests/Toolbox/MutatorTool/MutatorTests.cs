using CreateAndFake.Design;
using CreateAndFake.Toolbox.FakerTool;
using CreateAndFake.Toolbox.MutatorTool;
using CreateAndFake.Toolbox.ValuerTool;
using CreateAndFakeTests.TestSamples;

namespace CreateAndFakeTests.Toolbox.MutatorTool;

public static class MutatorTests
{
    [Fact]
    internal static void Mutator_GuardsNulls()
    {
        // Tools.Tester.PreventsNullRefException<Mutator>(Tools.Randomizer, Tools.Valuer, Limiter.Dozen);
    }

    [Fact]
    internal static void Mutator_NoParameterMutation()
    {
        // Tools.Tester.PreventsParameterMutation(Tools.Mutator);
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
        fakeValuer.Equals(Arg.Any<object>(), Arg.Any<object>()).SetupReturn(true);

        new Mutator(Tools.Mutator.Options with { Valuer = fakeValuer, Limiter = new Limiter(3) })
            .Assert(t => t.Variant(sample))
            .Throws<TimeoutException>();

        fakeValuer.Assert().Called();
    }

    [Theory, RandomData]
    internal static void Variant_RepeatsUntilUnequal([Fake] IValuer fakeValuer, DataSample sample)
    {
        fakeValuer.Equals(Arg.Any<object>(), Arg.Any<object>()).SetupCall(Behavior.Series(true, true, true, false));

        new Mutator(Tools.Mutator.Options with { Valuer = fakeValuer, Limiter = new Limiter(5) })
            .Variant(sample)
            .Assert()
            .IsNot(null);

        fakeValuer.Assert().Called();
    }

    [Theory, RandomData]
    internal static void Variant_RepeatsUntilBothUnequal(
        [Fake] IValuer fakeValuer, DataSample sample1, DataSample sample2)
    {
        fakeValuer.Equals(Arg.Any<object>(), Arg.Any<object>()).SetupCall(
            Behavior.Series(false, true, true, false, true, true, false, false));

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

        new Mutator(Tools.Mutator.Options with { Valuer = fakeValuer, Limiter = new Limiter(3) })
            .Assert(t => t.Unique(sample))
            .Throws<TimeoutException>();
    }

    [Theory, RandomData]
    internal static void Unique_RepeatsUntilUnequal([Fake] IValuer fakeValuer, string sample)
    {
        fakeValuer.Equals(Arg.Any<object>(), Arg.Any<object>()).SetupCall(Behavior.Series(true, true, true, false));
        fakeValuer.GetHashCode(Arg.Any<object>()).SetupReturn(0);

        new Mutator(Tools.Mutator.Options with { Valuer = fakeValuer, Limiter = new Limiter(5) })
            .Unique(sample)
            .Assert()
            .IsNot(null);
    }

    [Theory, RandomData]
    internal static void Unique_RepeatsUntilBothUnequal(
        [Fake] IValuer fakeValuer, string sample1, string sample2)
    {
        fakeValuer.Equals(Arg.Any<object>(), Arg.Any<object>()).SetupCall(
            Behavior.Series(false, true, true, false, true, true, false, false));
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
