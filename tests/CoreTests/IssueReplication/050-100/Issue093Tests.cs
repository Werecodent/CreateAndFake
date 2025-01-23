using CreateAndFake.Toolbox.AsserterTool;
using CreateAndFake.Toolbox.FakerTool.Proxy;

namespace CreateAndFakeTests.IssueReplication;

public static class Issue093Tests
{
    public abstract class Provider
    {
        public abstract string Value { get; set; }
    }

    internal sealed class Api
    {
        private readonly Provider _provider;

        internal Api(Provider provider)
        {
            _provider = provider;
        }

        public string Value => _provider.Value;
    }

    [Theory, RandomData]
    internal static void Issue093_AssertFakeCallIntegration([Fake] Provider faked, Api sample, string value)
    {
        faked.Value.SetupReturn(value);

        Tools.Asserter.Throws<FakeVerifyException>(() => faked.Assert().Called());

        sample.Value.Assert().Is(value).Also(faked).Called();

        Tools.Asserter.Throws<AssertException>(() => sample.Value.Assert().Is(""));
    }
}