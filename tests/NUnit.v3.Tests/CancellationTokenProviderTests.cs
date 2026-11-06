namespace Werecodent.CreateAndFake.NUnit.v3.Tests;

[TestFixture]
public sealed class CancellationTokenProviderTests() : AutoTester(typeof(CancellationTokenProvider))
{
    [Test]
    public static void Get_SuccessfullyGetsToken()
    {
        CancellationTokenProvider.Get().Assert().Is(TestContext.CurrentContext.CancellationToken);
    }
}
