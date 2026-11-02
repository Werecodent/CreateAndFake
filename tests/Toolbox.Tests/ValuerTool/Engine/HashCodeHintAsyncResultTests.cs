using Werecodent.CreateAndFake.ValuerTool.Engine;

namespace Werecodent.CreateAndFake.Tests.ValuerTool.Engine;

public sealed class HashCodeHintAsyncResultTests : AutoTester<HashCodeHintAsyncResult>
{
    [Fact]
    internal static void None_HasNoData()
    {
        HashCodeHintAsyncResult.None.HasData.Assert().Is(false);
    }
}
