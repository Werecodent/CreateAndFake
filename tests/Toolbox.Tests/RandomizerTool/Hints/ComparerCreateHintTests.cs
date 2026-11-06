using Werecodent.CreateAndFake.Design.Comparisons;
using Werecodent.CreateAndFake.RandomizerTool.Hints;

namespace Werecodent.CreateAndFake.Tests.RandomizerTool.Hints;

public sealed class ComparerCreateHintTests : CreateHintTestBase<ComparerCreateHint>
{
    private static readonly Type[] _ValidTypes =
    [
        typeof(IAsyncEqualityComparer<string>),
        typeof(IAsyncEqualityComparer<int>),
        typeof(IEqualityComparer<string>),
        typeof(IEqualityComparer<int>),
    ];

    public ComparerCreateHintTests()
        : base(_ValidTypes) { }
}
