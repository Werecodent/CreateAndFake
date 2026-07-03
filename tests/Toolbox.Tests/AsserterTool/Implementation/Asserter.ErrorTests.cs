using CreateAndFake.AsserterTool;
using CreateAndFake.Fluent.AssertCalls;

namespace CreateAndFake.Tests.AsserterTool.Implementation;

public sealed class AsserterErrorTests
{
    private readonly AsserterMod _config;

    private bool _configCalled;

    public AsserterErrorTests()
    {
        _configCalled = false;
        _config = opt =>
        {
            _configCalled = true;
            return opt;
        };
    }

    [Theory, RandomData]
    internal void Fail_Throws(Exception sample)
    {
        sample.Assert(d => d.Assert().Fail()).Throws<AssertException>();
        sample.Assert(d => d.Assert().Fail()).Throws<AssertException>(_config);
        _configCalled.Assert().Is(true);
    }

    [Theory, RandomData]
    internal void Fail_OnlyThrows([Stub] IAsserter asserter, Exception sample)
    {
        AssertError instance = new(asserter, sample);
        instance.Fail();
        instance.Fail(_config);
    }
}
