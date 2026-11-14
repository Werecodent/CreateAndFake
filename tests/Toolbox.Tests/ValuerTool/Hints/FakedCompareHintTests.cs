using Werecodent.CreateAndFake.ValuerTool.Engine;
using Werecodent.CreateAndFake.ValuerTool.Hints;

namespace Werecodent.CreateAndFake.Tests.ValuerTool.Hints;

public sealed class FakedCompareHintTests : CompareHintTestBase<FakedCompareHint>
{
    [Theory, RandomData]
    internal void Compare_MismatchedFakesDifference(
        [Stub] object stub1,
        [Stub] IEnumerable<string> stub2
    )
    {
        DifferenceHintResult result = TestInstance.TryToCompare(stub1, stub2, CreateChainer());

        result.HasData.Assert().Is(true);
        result.Data.Assert().IsNotEmpty();
    }
}
