using CreateAndFake.AsserterTool;
using CreateAndFake.FakerTool;
using CreateAndFake.Samples.Scenarios;

namespace CreateAndFake.Tests.AsserterTool;

public sealed class AsserterObjectTests
{
    private readonly AsserterMod _config;

    private bool _configCalled;

    public AsserterObjectTests()
    {
        _configCalled = false;
        _config = opt =>
        {
            _configCalled = true;
            return opt;
        };
    }

    private readonly Asserter _testInstance = new(Tools.Asserter.Options);

    [Theory, RandomData]
    internal void ReferenceEqual_NotByValue([Stub] object fake)
    {
        fake.Equals(Arg.Any<object>()).SetupReturn(true, Times.Never);

        _testInstance.ReferenceEqual(fake, fake);
        _testInstance.Assert(t => t.ReferenceEqual(fake, fake.CreateDeepClone()));

        fake.Assert().Called(Times.Never);
    }

    [Theory, RandomData]
    internal void ReferenceNotEqual_NotByValue([Stub] object fake)
    {
        fake.Equals(Arg.Any<object>()).SetupReturn(false, Times.Never);

        _testInstance.ReferenceNotEqual(fake, fake.CreateDeepClone());
        _testInstance.Assert(t => t.ReferenceNotEqual(fake, fake)).Throws<AssertException>();

        fake.Assert().Called(Times.Never);
    }

    [Theory, RandomData]
    internal void ValuesEqual_EqualValid(object value)
    {
        _testInstance.ValuesEqual(value, value.CreateDeepClone());
    }

    [Theory, RandomData]
    internal void ValuesEqual_UnequalInvalid(string value)
    {
        _testInstance
            .Assert(t => t.ValuesEqual(value, value.CreateVariant()))
            .Throws<AssertException>();
        _testInstance.Assert(t => t.ValuesEqual(null, value)).Throws<AssertException>();
    }

    [Theory, RandomData]
    internal void ValuesEqual_CanHandleNullsNotEqual(object value)
    {
        _testInstance.Assert(t => t.ValuesEqual(value, null)).Throws<AssertException>();
        _testInstance.Assert(t => t.ValuesEqual(null, value)).Throws<AssertException>();
    }

    [Theory, RandomData]
    internal void ValuesNotEqual_UnequalValid(string value)
    {
        _testInstance.ValuesNotEqual(value, value.CreateVariant());
        _testInstance.ValuesNotEqual(null, value);
        _testInstance.ValuesNotEqual(value, null);
    }

    [Theory, RandomData]
    internal void ValuesNotEqual_EqualInvalid(string value)
    {
        _testInstance
            .Assert(t => t.ValuesNotEqual(value, value.CreateDeepClone()))
            .Throws<AssertException>();
        _testInstance.Assert(t => t.ValuesNotEqual(value, value)).Throws<AssertException>();
        _testInstance.Assert(t => t.ValuesNotEqual(null, null)).Throws<AssertException>();
    }

    [Theory, RandomData]
    internal void AreUnique_UnequalValid(string value)
    {
        _testInstance.AreUnique(value, value.CreateVariant());
        _testInstance.AreUnique(null, value);
        _testInstance.AreUnique(value, null);
    }

    [Theory, RandomData]
    internal void AreUnique_EqualInvalid(string value)
    {
        _testInstance
            .Assert(t => t.AreUnique(value, value.CreateDeepClone()))
            .Throws<AssertException>();
        _testInstance.Assert(t => t.AreUnique(value, value)).Throws<AssertException>();
        _testInstance.Assert(t => t.AreUnique(null, null)).Throws<AssertException>();
    }

    [Theory, RandomData]
    internal void Is_UsesValueQualityPass(DataSample sample)
    {
        sample.Assert().Is(sample.CreateDeepClone());
        sample.Assert().Is(sample.CreateDeepClone(), _config);
        _configCalled.Assert().Is(true);
    }

    [Theory, RandomData]
    internal void Is_UsesValueQualityFail(DataSample sample)
    {
        sample.Assert(s => s.Assert().Is(sample.CreateVariant())).Throws<AssertException>();
        sample.Assert(s => s.Assert().Is(sample.CreateVariant())).Throws<AssertException>(_config);
        _configCalled.Assert().Is(true);
    }

    [Theory, RandomData]
    internal void IsNot_UsesValueQualityPass(DataSample sample)
    {
        sample.Assert().IsNot(sample.CreateVariant());
        sample.Assert().IsNot(sample.CreateVariant(), _config);
        _configCalled.Assert().Is(true);
    }

    [Theory, RandomData]
    internal void IsNot_UsesValueQualityFail(DataSample sample)
    {
        sample.Assert(s => s.Assert().IsNot(sample.CreateDeepClone())).Throws<AssertException>();
        sample
            .Assert(s => s.Assert().IsNot(sample.CreateDeepClone()))
            .Throws<AssertException>(_config);
        _configCalled.Assert().Is(true);
    }

    [Theory, RandomData]
    internal void ReferenceEqual_SameObject(DataSample sample)
    {
        sample.Assert().ReferenceEqual(sample);
        sample.Assert().ReferenceEqual(sample, _config);
        _configCalled.Assert().Is(true);
    }

    [Theory, RandomData]
    internal void ReferenceEqual_DifferentObject(DataSample sample)
    {
        sample
            .Assert(s => s.Assert().ReferenceEqual(sample.CreateDeepClone()))
            .Throws<AssertException>();
        sample
            .Assert(s => s.Assert().ReferenceEqual(sample.CreateDeepClone()))
            .Throws<AssertException>(_config);
        _configCalled.Assert().Is(true);
    }

    [Theory, RandomData]
    internal void ReferenceNotEqual_DifferentObject(DataSample sample)
    {
        sample.Assert().ReferenceNotEqual(sample.CreateVariant());
        sample.Assert().ReferenceNotEqual(sample.CreateVariant(), _config);
        _configCalled.Assert().Is(true);
    }

    [Theory, RandomData]
    internal void ReferenceNotEqual_SameObject(DataSample sample)
    {
        sample.Assert(s => s.Assert().ReferenceNotEqual(sample)).Throws<AssertException>();
        sample.Assert(s => s.Assert().ReferenceNotEqual(sample)).Throws<AssertException>(_config);
        _configCalled.Assert().Is(true);
    }

    [Theory, RandomData]
    internal void ValuesEqual_UsesValueQualityPass(DataSample sample)
    {
        sample.Assert().ValuesEqual(sample.CreateDeepClone());
        sample.Assert().ValuesEqual(sample.CreateDeepClone(), _config);
        _configCalled.Assert().Is(true);
    }

    [Theory, RandomData]
    internal void ValuesEqual_UsesValueQualityFail(DataSample sample)
    {
        sample
            .Assert(s => s.Assert().ValuesEqual(sample.CreateVariant()))
            .Throws<AssertException>();
        sample
            .Assert(s => s.Assert().ValuesEqual(sample.CreateVariant()))
            .Throws<AssertException>(_config);
        _configCalled.Assert().Is(true);
    }

    [Theory, RandomData]
    internal void ValuesNotEqual_UsesValueQualityPass(DataSample sample)
    {
        sample.Assert().ValuesNotEqual(sample.CreateVariant());
        sample.Assert().ValuesNotEqual(sample.CreateVariant(), _config);
        _configCalled.Assert().Is(true);
    }

    [Theory, RandomData]
    internal void ValuesNotEqual_UsesValueQualityFail(DataSample sample)
    {
        sample
            .Assert(s => s.Assert().ValuesNotEqual(sample.CreateDeepClone()))
            .Throws<AssertException>();
        sample
            .Assert(s => s.Assert().ValuesNotEqual(sample.CreateDeepClone()))
            .Throws<AssertException>(_config);
        _configCalled.Assert().Is(true);
    }

    [Theory, RandomData]
    internal void AreUnique_NoSharedPass(DataSample sample)
    {
        sample.Assert().UniqueFrom(sample.CreateVariant());
        sample.Assert().UniqueFrom(sample.CreateVariant(), _config);
        _configCalled.Assert().Is(true);
    }

    [Theory, RandomData]
    internal void AreUnique_SharedFail(DataSample sample)
    {
        sample
            .Assert(s => s.Assert().UniqueFrom(sample.CreateDeepClone()))
            .Throws<AssertException>();
        sample
            .Assert(s => s.Assert().UniqueFrom(sample.CreateDeepClone()))
            .Throws<AssertException>(_config);
        _configCalled.Assert().Is(true);
    }
}
