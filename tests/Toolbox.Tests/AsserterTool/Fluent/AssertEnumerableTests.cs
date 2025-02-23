using CreateAndFake.AsserterTool;
using CreateAndFake.AsserterTool.Fluent;
using CreateAndFake.Tests.TestSamples;

namespace CreateAndFake.Tests.AsserterTool.Fluent;

public static class AssertEnumerableTests
{
    [Fact]
    internal static void AssertEnumerable_GuardsNulls()
    {
        Tools.Tester.PreventsNullRefException<AssertEnumerable>();
    }

    [Fact]
    internal static void AssertEnumerable_NoParameterMutation()
    {
        Tools.Tester.PreventsParameterMutation<AssertEnumerable>();
    }

    [Theory, RandomData]
    internal static void IsEmpty_NoItems([Size(0)] IEnumerable<DataSample> items)
    {
        items.Assert().IsEmpty();
    }

    [Theory, RandomData]
    internal static void IsEmpty_WithItems(IEnumerable<DataSample> items)
    {
        items.Assert(d => d.Assert().IsEmpty()).Throws<AssertException>();
    }

    [Theory, RandomData]
    internal static void IsNotEmpty_WithItems(IEnumerable<DataSample> items)
    {
        items.Assert().IsNotEmpty();
    }

    [Theory, RandomData]
    internal static void IsNotEmpty_NoItems([Size(0)] IEnumerable<DataSample> items)
    {
        items.Assert(d => d.Assert().IsNotEmpty()).Throws<AssertException>();
    }

    [Theory, RandomData]
    internal static void HasCount_SameSize(ICollection<DataSample> items)
    {
        items.Assert().HasCount(items.Count);
    }

    [Theory, RandomData]
    internal static void HasCount_MismatchedSize(ICollection<DataSample> items)
    {
        items.Assert(d => d.Assert().HasCount(items.Count.CreateVariant())).Throws<AssertException>();
    }

    [Theory, RandomData]
    internal static void Contains_UsingSubitem(ICollection<DataSample> items)
    {
        items.Assert().Contains(Tools.Gen.NextItem(items));
    }

    [Theory, RandomData]
    internal static void Contains_RandomOther(ICollection<DataSample> items)
    {
        items
            .Assert(d => d.Assert().Contains(Tools.Mutator.Variant(items)))
            .Throws<AssertException>();
    }

    [Theory, RandomData]
    internal static void ContainsNot_RandomOther(ICollection<DataSample> items)
    {
        items.Assert().ContainsNot(Tools.Mutator.Variant(items));
    }

    [Theory, RandomData]
    internal static void ContainsNot_UsingSubitem(ICollection<DataSample> items)
    {
        items
            .Assert(d => d.Assert().ContainsNot(Tools.Gen.NextItem(items)))
            .Throws<AssertException>();
    }

    [Theory, RandomData]
    internal static void Fail_Throws(IEnumerable<DataSample> items)
    {
        items.Assert(d => d.Assert().Fail()).Throws<AssertException>();
    }
}
