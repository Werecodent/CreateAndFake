using Werecodent.CreateAndFake.ExtractorTool.Engine;

namespace Werecodent.CreateAndFake.Tests.ExtractorTool.Engine;

public sealed class ExtractHintResultTests : AutoTester<ExtractHintResult>
{
    [Fact]
    internal static void None_HasNoData()
    {
        ExtractHintResult.None.HasData.Assert().Is(false);
    }
}
