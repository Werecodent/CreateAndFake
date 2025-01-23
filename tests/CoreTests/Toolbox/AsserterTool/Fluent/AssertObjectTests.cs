using CreateAndFake.Toolbox.AsserterTool;
using CreateAndFake.Toolbox.AsserterTool.Fluent;
using CreateAndFakeTests.TestSamples;

namespace CreateAndFakeTests.Toolbox.AsserterTool.Fluent;

public static class AssertObjectTests
{
    [Fact]
    internal static void AssertObject_GuardsNulls()
    {
        Tools.Tester.PreventsNullRefException<AssertObject>();
    }

    [Fact]
    internal static void AssertObject_NoParameterMutation()
    {
        Tools.Tester.PreventsParameterMutation<AssertObject>();
    }

    [Theory, RandomData]
    internal static void Is_UsesValueQualityPass(DataSample sample)
    {
        sample.Assert().Is(sample.CreateDeepClone());
    }

    [Theory, RandomData]
    internal static void Is_UsesValueQualityFail(DataSample sample)
    {
        sample.Assert(s => s.Assert().Is(sample.CreateVariant())).Throws<AssertException>();
    }

    [Theory, RandomData]
    internal static void IsNot_UsesValueQualityPass(DataSample sample)
    {
        sample.Assert().IsNot(sample.CreateVariant());
    }

    [Theory, RandomData]
    internal static void IsNot_UsesValueQualityFail(DataSample sample)
    {
        sample.Assert(s => s.Assert().IsNot(sample.CreateDeepClone())).Throws<AssertException>();
    }

    [Theory, RandomData]
    internal static void ReferenceEqual_SameObject(DataSample sample)
    {
        sample.Assert().ReferenceEqual(sample);
    }

    [Theory, RandomData]
    internal static void ReferenceEqual_DifferentObject(DataSample sample)
    {
        sample.Assert(s => s.Assert().ReferenceEqual(sample.CreateDeepClone())).Throws<AssertException>();
    }

    [Theory, RandomData]
    internal static void ReferenceNotEqual_DifferentObject(DataSample sample)
    {
        sample.Assert().ReferenceNotEqual(sample.CreateVariant());
    }

    [Theory, RandomData]
    internal static void ReferenceNotEqual_SameObject(DataSample sample)
    {
        sample.Assert(s => s.Assert().ReferenceNotEqual(sample)).Throws<AssertException>();
    }

    [Theory, RandomData]
    internal static void ValuesEqual_UsesValueQualityPass(DataSample sample)
    {
        sample.Assert().ValuesEqual(sample.CreateDeepClone());
    }

    [Theory, RandomData]
    internal static void ValuesEqual_UsesValueQualityFail(DataSample sample)
    {
        sample.Assert(s => s.Assert().ValuesEqual(sample.CreateVariant())).Throws<AssertException>();
    }

    [Theory, RandomData]
    internal static void ValuesNotEqual_UsesValueQualityPass(DataSample sample)
    {
        sample.Assert().ValuesNotEqual(sample.CreateVariant());
    }

    [Theory, RandomData]
    internal static void ValuesNotEqual_UsesValueQualityFail(DataSample sample)
    {
        sample.Assert(s => s.Assert().ValuesNotEqual(sample.CreateDeepClone())).Throws<AssertException>();
    }

    [Theory, RandomData]
    internal static void UniqueFrom_NoSharedPass(string sample)
    {
        sample.Assert().UniqueFrom(sample.CreateVariant());
    }

    [Theory, RandomData]
    internal static void UniqueFrom_SharedFail(string sample)
    {
        sample.Assert(s => s.Assert().UniqueFrom(sample.CreateDeepClone())).Throws<AssertException>();
    }

    [Theory, RandomData]
    internal static void Fail_Throws(IEnumerable<DataSample> items)
    {
        items.Assert(d => d.Assert().Fail()).Throws<AssertException>();
    }

    [Theory, RandomData]
    internal static void Pass_Works(object item)
    {
        item.Assert().Pass();
    }
}
