using System.Collections;
using Werecodent.CreateAndFake.Design.Comparisons;
using Werecodent.CreateAndFake.RandomizerTool.Hints;
using Werecodent.CreateAndFake.Samples.Scenarios;

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

    private static readonly Type[] _InvalidTypes =
    [
        typeof(DataHolderSample),
        typeof(IEnumerable),
        typeof(IEnumerable<>),
    ];

    public ComparerCreateHintTests()
        : base(_ValidTypes, _InvalidTypes) { }
}
