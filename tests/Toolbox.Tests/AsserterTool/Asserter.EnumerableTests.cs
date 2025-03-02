using CreateAndFake.AsserterTool;
using CreateAndFake.AsserterTool.Fluent;
using CreateAndFake.Tests.TestSamples;

namespace CreateAndFake.Tests.AsserterTool;

public sealed class AsserterEnumerableTests
{
    private readonly Asserter _testInstance = new(Tools.Asserter.Options);

    private readonly AsserterMod _config;

    private bool _configCalled;

    public AsserterEnumerableTests()
    {
        _configCalled = false;
        _config = opt =>
        {
            _configCalled = true;
            return opt;
        };
    }

    [Theory, RandomData]
    internal void IsEmpty_Works(IEnumerable<string> data)
    {
        _testInstance.IsEmpty(Array.Empty<string>());

        _testInstance.Assert(t => t.IsEmpty(null)).Throws<AssertException>();
        _testInstance.Assert(t => t.IsEmpty(data)).Throws<AssertException>();
    }

    [Theory, RandomData]
    internal void IsNotEmpty_Works(IEnumerable<string> data)
    {
        _testInstance.IsNotEmpty(data);

        _testInstance.Assert(t => t.IsNotEmpty(null)).Throws<AssertException>();
        _testInstance.Assert(t => t.IsNotEmpty(Array.Empty<string>())).Throws<AssertException>();
    }

    [Theory, RandomData]
    internal void HasCount_Works(IEnumerable<string> data)
    {
        _testInstance.HasCount(data.Count(), data);

        _testInstance.Assert(t => t.HasCount(data.Count(), null)).Throws<AssertException>();
        _testInstance.Assert(t => t.HasCount(data.Count() - 1, data)).Throws<AssertException>();
        _testInstance.Assert(t => t.HasCount(data.Count() + 1, data)).Throws<AssertException>();
    }

    [Theory, RandomData]
    internal void IsEmpty_NoItems([Size(0)] IEnumerable<DataSample> items)
    {
        items.Assert().IsEmpty();
        items.Assert().IsEmpty(_config);
        _configCalled.Assert().Is(true);
    }

    [Theory, RandomData]
    internal void IsEmpty_WithItems(IEnumerable<DataSample> items)
    {
        items.Assert(d => d.Assert().IsEmpty()).Throws<AssertException>();
        items.Assert(d => d.Assert().IsEmpty()).Throws<AssertException>(_config);
        _configCalled.Assert().Is(true);
    }

    [Theory, RandomData]
    internal void IsNotEmpty_WithItems(IEnumerable<DataSample> items)
    {
        items.Assert().IsNotEmpty();
        items.Assert().IsNotEmpty(_config);
        _configCalled.Assert().Is(true);
    }

    [Theory, RandomData]
    internal void IsNotEmpty_NoItems([Size(0)] IEnumerable<DataSample> items)
    {
        items.Assert(d => d.Assert().IsNotEmpty()).Throws<AssertException>();
        items.Assert(d => d.Assert().IsNotEmpty()).Throws<AssertException>(_config);
        _configCalled.Assert().Is(true);
    }

    [Theory, RandomData]
    internal void HasCount_SameSize(ICollection<DataSample> items)
    {
        items.Assert().HasCount(items.Count);
        items.Assert().HasCount(items.Count, _config);
        _configCalled.Assert().Is(true);
    }

    [Theory, RandomData]
    internal void HasCount_MismatchedSize(ICollection<DataSample> items)
    {
        items.Assert(d => d.Assert().HasCount(items.Count.CreateVariant())).Throws<AssertException>();
        items.Assert(d => d.Assert().HasCount(items.Count.CreateVariant())).Throws<AssertException>(_config);
        _configCalled.Assert().Is(true);
    }

    [Theory, RandomData]
    internal void Contains_UsingSubitem(ICollection<DataSample> items)
    {
        items.Assert().Contains(Tools.Gen.NextItem(items));
        items.Assert().Contains(Tools.Gen.NextItem(items), _config);
        _configCalled.Assert().Is(true);
    }

    [Theory, RandomData]
    internal void Contains_RandomOther(ICollection<DataSample> items)
    {
        items
            .Assert(d => d.Assert().Contains(Tools.Mutator.Variant(items)))
            .Throws<AssertException>();
        items
            .Assert(d => d.Assert().Contains(Tools.Mutator.Variant(items)))
            .Throws<AssertException>(_config);
        _configCalled.Assert().Is(true);
    }

    [Theory, RandomData]
    internal void ContainsNot_RandomOther(ICollection<DataSample> items)
    {
        items.Assert().ContainsNot(Tools.Mutator.Variant(items));
        items.Assert().ContainsNot(Tools.Mutator.Variant(items), _config);
        _configCalled.Assert().Is(true);
    }

    [Theory, RandomData]
    internal void ContainsNot_UsingSubitem(ICollection<DataSample> items)
    {
        items
            .Assert(d => d.Assert().ContainsNot(Tools.Gen.NextItem(items)))
            .Throws<AssertException>();
        items
            .Assert(d => d.Assert().ContainsNot(Tools.Gen.NextItem(items)))
            .Throws<AssertException>(_config);
        _configCalled.Assert().Is(true);
    }

    [Theory, RandomData]
    internal void Fail_Throws(IEnumerable<DataSample> items)
    {
        items.Assert(d => d.Assert().Fail()).Throws<AssertException>();
        items.Assert(d => d.Assert().Fail()).Throws<AssertException>(_config);
        _configCalled.Assert().Is(true);
    }

    [Theory, RandomData]
    internal void Fail_OnlyThrows([Stub] IAsserter asserter, IEnumerable<DataSample> items)
    {
        AssertEnumerable instance = new(asserter, items);
        instance.Fail();
        instance.Fail(_config);
    }
}
