using Werecodent.CreateAndFake.DuplicatorTool.Engine;

namespace Werecodent.CreateAndFake.Tests.DuplicatorTool.Engine;

public sealed class CopyHintResultTests : AutoTester<CopyHintResult>
{
    [Fact]
    internal static void None_HasNoData()
    {
        CopyHintResult.None.HasData.Assert().Is(false);
    }
}
