using Werecodent.CreateAndFake.FakerTool.Proxy;

namespace Werecodent.CreateAndFake.Tests.FakerTool.Proxy;

public sealed class FakeCallExceptionTests : ExceptionTestBase<FakeCallException>
{
    [Fact]
    internal static void FakeCallException_MultiNullsOkay()
    {
        new FakeCallException(null, null).Assert().IsNotNull();
    }
}
