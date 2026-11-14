using Werecodent.CreateAndFake.FakerTool.Engine;

namespace Werecodent.CreateAndFake.Tests.FakerTool.Engine;

public sealed class FakeHintResultTests : AutoTester<FakeHintResult>
{
    [Fact]
    internal static void None_HasNoData()
    {
        FakeHintResult.None.HasData.Assert().Is(false);
    }
}
