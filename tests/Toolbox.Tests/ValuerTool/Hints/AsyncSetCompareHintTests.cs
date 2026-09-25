using System.Collections;
using Werecodent.CreateAndFake.Design.Content;
using Werecodent.CreateAndFake.Samples.Scenarios;
using Werecodent.CreateAndFake.ValuerTool.Hints;

namespace Werecodent.CreateAndFake.Tests.ValuerTool.Hints;

public sealed class AsyncSetCompareHintTests : CompareHintTestBase<AsyncSetCompareHint>
{
    private static readonly AsyncSetCompareHint _TestInstance = new();

    private static readonly Type[] _ValidTypes =
    [
        typeof(IAsyncSet<int>),
        typeof(IAsyncSet<string>),
        typeof(IAsyncSet<object>),
        typeof(AsyncHashSet<int>),
        typeof(AsyncHashSet<string>),
        typeof(AsyncHashSet<object>),
    ];

    private static readonly Type[] _InvalidTypes = [typeof(IEnumerable), typeof(DataHolderSample)];

    public AsyncSetCompareHintTests()
        : base(_TestInstance, _ValidTypes, _InvalidTypes) { }
}
