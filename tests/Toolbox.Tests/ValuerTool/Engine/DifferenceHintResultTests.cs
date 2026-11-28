using Werecodent.CreateAndFake.ValuerTool.Engine;

namespace Werecodent.CreateAndFake.Tests.ValuerTool.Engine;

public sealed class DifferenceHintResultTests : AutoTester<DifferenceHintResult>
{
    [Fact]
    internal static void None_HasNoData()
    {
        DifferenceHintResult.None.HasData.Assert().Is(false);
    }
}
