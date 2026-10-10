using Werecodent.CreateAndFake.Design.Content;
using Werecodent.CreateAndFake.DuplicatorTool.Hints;

namespace Werecodent.CreateAndFake.Tests.DuplicatorTool.Hints;

public sealed class AsyncDesignCopyHintTests : CopyHintTestBase<AsyncDesignCopyHint>
{
    [Theory, RandomData]
    internal static Task TryCopy_Empty([Size(0)] AsyncList<int> items)
    {
        return Tools.Asserter.IsAsync(
            items,
            items.Tools().Copy(),
            TestContext.Current.CancellationToken
        );
    }
}
