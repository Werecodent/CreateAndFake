using Werecodent.CreateAndFake.ValuerTool.Engine;

namespace Werecodent.CreateAndFake.Tests.ValuerTool.Engine;

public sealed class DifferenceHintAsyncResultTests : AutoTester<DifferenceHintAsyncResult>
{
    [Fact]
    internal static void None_HasNoData()
    {
        DifferenceHintAsyncResult.None.HasData.Assert().Is(false);
    }
}
