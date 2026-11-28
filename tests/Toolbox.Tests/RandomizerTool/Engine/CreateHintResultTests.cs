using Werecodent.CreateAndFake.RandomizerTool.Engine;

namespace Werecodent.CreateAndFake.Tests.RandomizerTool.Engine;

public sealed class CreateHintResultTests : AutoTester<CreateHintResult>
{
    [Fact]
    internal static void None_HasNoData()
    {
        CreateHintResult.None.HasData.Assert().Is(false);
    }
}
