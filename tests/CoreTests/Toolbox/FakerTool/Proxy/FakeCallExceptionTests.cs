using CreateAndFake.Toolbox.FakerTool.Proxy;

namespace CreateAndFakeTests.Toolbox.FakerTool.Proxy;

public sealed class FakeCallExceptionTests : ExceptionTestBase<FakeCallException>
{
    [Fact]
    internal static void FakeCallException_MultiNullsOkay()
    {
        new FakeCallException(null, null).Assert().IsNot(null);
    }
}
