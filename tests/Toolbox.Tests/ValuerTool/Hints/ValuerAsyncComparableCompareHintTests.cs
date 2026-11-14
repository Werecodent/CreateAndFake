using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.ValuerTool;
using Werecodent.CreateAndFake.ValuerTool.Hints;

namespace Werecodent.CreateAndFake.Tests.ValuerTool.Hints;

public sealed class ValuerAsyncComparableCompareHintTests
    : CompareHintTestBase<ValuerAsyncComparableCompareHint>
{
    protected override bool RequiresAsync { get; } = true;

    [Theory, RandomData]
    internal void TryToCompare_BlocksComparison(IValuerAsyncComparable data)
    {
        TestInstance
            .Assert(x => x.TryToCompare(data, data, CreateChainer()))
            .Throws<EngineException>();
    }

    [Theory, RandomData]
    internal void TryToGetHashCode_BlocksHashing(IValuerAsyncComparable data)
    {
        TestInstance
            .Assert(x => x.TryToGetHashCode(data, CreateChainer()))
            .Throws<EngineException>();
    }
}
