using CreateAndFake.AsserterTool;
using CreateAndFake.FakerTool.Proxy;

namespace CreateAndFake.Tests.IssueReplication;

public static class Issue093Tests
{
    public interface IProvider
    {
        string Value { get; set; }
    }

    public abstract class AbstractProvider : IProvider
    {
        public string Value { get; set; }
    }

    public class UnsealedProvider : IProvider
    {
        public string Value { get; set; }
    }

    internal sealed class Api
    {
        private readonly IProvider _provider;

        internal Api(IProvider provider)
        {
            _provider = provider;
        }

        public string Value => _provider.Value;
    }

    [Theory, RandomData]
    internal static void Issue093_AssertInterfaceFakeCallIntegration(
        [Fake] IProvider faked,
        Api sample,
        string value
    )
    {
        TestFakeCallIntegration(faked, sample, value);
    }

    [Theory, RandomData]
    internal static void Issue093_AssertAbstractFakeCallIntegration(
        [Fake] AbstractProvider faked,
        Api sample,
        string value
    )
    {
        TestFakeCallIntegration(faked, sample, value);
    }

    [Theory, RandomData]
    internal static void Issue093_AssertUnsealedFakeCallIntegration(
        [Fake] UnsealedProvider faked,
        Api sample,
        string value
    )
    {
        TestFakeCallIntegration(faked, sample, value);
    }

    private static void TestFakeCallIntegration(IProvider faked, Api sample, string value)
    {
        faked.Value.SetupReturn(value);
        Tools.Asserter.Throws<FakeVerifyException>(() => faked.Assert().Called());

        sample.Value.Assert().Is(value).Also(faked).Called();
        Tools.Asserter.Throws<AssertException>(() => sample.Value.Assert().Is(""));
    }
}
