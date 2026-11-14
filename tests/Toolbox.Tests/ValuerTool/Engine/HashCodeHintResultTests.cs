using Werecodent.CreateAndFake.ValuerTool.Engine;

namespace Werecodent.CreateAndFake.Tests.ValuerTool.Engine;

public sealed class HashCodeHintResultTests : AutoTester<HashCodeHintResult>
{
    [Fact]
    internal static void None_HasNoData()
    {
        HashCodeHintResult.None.HasData.Assert().Is(false);
    }
}
