using System.Collections;
using Werecodent.CreateAndFake.Design.Content;
using Werecodent.CreateAndFake.DuplicatorTool.Hints;
using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Tests.DuplicatorTool.Hints;

public sealed class AsyncDesignCopyHintTests : CopyHintTestBase<AsyncDesignCopyHint>
{
    private static readonly Type[] _ValidTypes =
    [
        typeof(AsyncList<int>),
        typeof(AsyncList<string>),
        typeof(AsyncList<object>),
    ];

    private static readonly Type[] _InvalidTypes = [typeof(DataHolderSample), typeof(IEnumerable)];

    public AsyncDesignCopyHintTests()
        : base(_ValidTypes, _InvalidTypes) { }

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
