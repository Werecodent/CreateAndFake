using Werecodent.CreateAndFake.FakerTool.Engine;

namespace Werecodent.CreateAndFake.Tests.FakerTool.Engine;

public sealed class SetupHintResultTests : AutoTester<SetupHintResult>
{
    [Fact]
    internal static void None_HasNoData()
    {
        SetupHintResult.None.HasData.Assert().Is(false);
    }
}
