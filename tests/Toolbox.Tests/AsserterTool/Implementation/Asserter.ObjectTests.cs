using CreateAndFake.AsserterTool;
using CreateAndFake.FakerTool;
using CreateAndFake.Samples.Scenarios;

namespace CreateAndFake.Tests.AsserterTool.Implementation;

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
        _testInstance.Assert(x => x.ReferenceEqual(fake, fake.Tools().Copy()));

        fake.Assert().Called(Times.Never);
    }

    [Theory, RandomData]
    internal void ReferenceNotEqual_NotByValue([Stub] object fake)
    {
        fake.Equals(Arg.Any<object>()).SetupReturn(false, Times.Never);

        _testInstance.ReferenceNotEqual(fake, fake.Tools().Copy());
        _testInstance.Assert(x => x.ReferenceNotEqual(fake, fake)).Throws<AssertException>();

        fake.Assert().Called(Times.Never);
    }

    [Theory, RandomData]
    internal void ValuesEqual_EqualValid(object value)
    {
        _testInstance.ValuesEqual(value, value.Tools().Copy());
    }

    [Theory, RandomData]
    internal void ValuesEqual_UnequalInvalid(string value)
    {
        _testInstance
            .Assert(x => x.ValuesEqual(value, value.Tools().Variant()))
            .Throws<AssertException>();
        _testInstance.Assert(x => x.ValuesEqual(null, value)).Throws<AssertException>();
    }

    [Theory, RandomData]
    internal void ValuesEqual_CanHandleNullsNotEqual(object value)
    {
        _testInstance.Assert(x => x.ValuesEqual(value, null)).Throws<AssertException>();
        _testInstance.Assert(x => x.ValuesEqual(null, value)).Throws<AssertException>();
    }

    [Theory, RandomData]
    internal void ValuesNotEqual_UnequalValid(string value)
    {
        _testInstance.ValuesNotEqual(value, value.Tools().Variant());
        _testInstance.ValuesNotEqual(null, value);
        _testInstance.ValuesNotEqual(value, null);
    }

    [Theory, RandomData]
    internal void ValuesNotEqual_EqualInvalid(string value)
    {
        _testInstance
            .Assert(x => x.ValuesNotEqual(value, value.Tools().Copy()))
            .Throws<AssertException>();
        _testInstance.Assert(x => x.ValuesNotEqual(value, value)).Throws<AssertException>();
        _testInstance.Assert(x => x.ValuesNotEqual(null, null)).Throws<AssertException>();
    }

    [Theory, RandomData]
    internal void AreUnique_UnequalValid(string value)
    {
        _testInstance.AreUnique(value, value.Tools().Variant());
        _testInstance.AreUnique(null, value);
        _testInstance.AreUnique(value, null);
    }

    [Theory, RandomData]
    internal void AreUnique_EqualInvalid(string value)
    {
        _testInstance
            .Assert(x => x.AreUnique(value, value.Tools().Copy()))
            .Throws<AssertException>();
        _testInstance.Assert(x => x.AreUnique(value, value)).Throws<AssertException>();
        _testInstance.Assert(x => x.AreUnique(null, null)).Throws<AssertException>();
    }

    [Theory, RandomData]
    internal void Is_UsesValueQualityPass(DataSample sample)
    {
        sample.Assert().Is(sample.Tools().Copy());
        sample.Assert().Is(sample.Tools().Copy(), _config);
        _configCalled.Assert().Is(true);
    }

    [Theory, RandomData]
    internal void Is_UsesValueQualityFail(DataSample sample)
    {
        sample.Assert(x => x.Assert().Is(sample.Tools().Variant())).Throws<AssertException>();
        sample
            .Assert(x => x.Assert().Is(sample.Tools().Variant()))
            .Throws<AssertException>(_config);
        _configCalled.Assert().Is(true);
    }

    [Theory, RandomData]
    internal void IsNot_UsesValueQualityPass(DataSample sample)
    {
        sample.Assert().IsNot(sample.Tools().Variant());
        sample.Assert().IsNot(sample.Tools().Variant(), _config);
        _configCalled.Assert().Is(true);
    }

    [Theory, RandomData]
    internal void IsNot_UsesValueQualityFail(DataSample sample)
    {
        sample.Assert(x => x.Assert().IsNot(sample.Tools().Copy())).Throws<AssertException>();
        sample
            .Assert(x => x.Assert().IsNot(sample.Tools().Copy()))
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
            .Assert(x => x.Assert().ReferenceEqual(sample.Tools().Copy()))
            .Throws<AssertException>();
        sample
            .Assert(x => x.Assert().ReferenceEqual(sample.Tools().Copy()))
            .Throws<AssertException>(_config);
        _configCalled.Assert().Is(true);
    }

    [Theory, RandomData]
    internal void ReferenceNotEqual_DifferentObject(DataSample sample)
    {
        sample.Assert().ReferenceNotEqual(sample.Tools().Variant());
        sample.Assert().ReferenceNotEqual(sample.Tools().Variant(), _config);
        _configCalled.Assert().Is(true);
    }

    [Theory, RandomData]
    internal void ReferenceNotEqual_SameObject(DataSample sample)
    {
        sample.Assert(x => x.Assert().ReferenceNotEqual(sample)).Throws<AssertException>();
        sample.Assert(x => x.Assert().ReferenceNotEqual(sample)).Throws<AssertException>(_config);
        _configCalled.Assert().Is(true);
    }

    [Theory, RandomData]
    internal void ValuesEqual_UsesValueQualityPass(DataSample sample)
    {
        sample.Assert().ValuesEqual(sample.Tools().Copy());
        sample.Assert().ValuesEqual(sample.Tools().Copy(), _config);
        _configCalled.Assert().Is(true);
    }

    [Theory, RandomData]
    internal void ValuesEqual_UsesValueQualityFail(DataSample sample)
    {
        sample
            .Assert(x => x.Assert().ValuesEqual(sample.Tools().Variant()))
            .Throws<AssertException>();
        sample
            .Assert(x => x.Assert().ValuesEqual(sample.Tools().Variant()))
            .Throws<AssertException>(_config);
        _configCalled.Assert().Is(true);
    }

    [Theory, RandomData]
    internal void ValuesNotEqual_UsesValueQualityPass(DataSample sample)
    {
        sample.Assert().ValuesNotEqual(sample.Tools().Variant());
        sample.Assert().ValuesNotEqual(sample.Tools().Variant(), _config);
        _configCalled.Assert().Is(true);
    }

    [Theory, RandomData]
    internal void ValuesNotEqual_UsesValueQualityFail(DataSample sample)
    {
        sample
            .Assert(x => x.Assert().ValuesNotEqual(sample.Tools().Copy()))
            .Throws<AssertException>();
        sample
            .Assert(x => x.Assert().ValuesNotEqual(sample.Tools().Copy()))
            .Throws<AssertException>(_config);
        _configCalled.Assert().Is(true);
    }

    [Theory, RandomData]
    internal void AreUnique_NoSharedPass(DataSample sample)
    {
        sample.Assert().UniqueFrom(sample.Tools().Variant());
        sample.Assert().UniqueFrom(sample.Tools().Variant(), _config);
        _configCalled.Assert().Is(true);
    }

    [Theory, RandomData]
    internal void AreUnique_SharedFail(DataSample sample)
    {
        sample.Assert(x => x.Assert().UniqueFrom(sample.Tools().Copy())).Throws<AssertException>();
        sample
            .Assert(x => x.Assert().UniqueFrom(sample.Tools().Copy()))
            .Throws<AssertException>(_config);
        _configCalled.Assert().Is(true);
    }
}
