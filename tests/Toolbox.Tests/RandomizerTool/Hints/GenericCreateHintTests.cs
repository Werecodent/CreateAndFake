using Werecodent.CreateAndFake.RandomizerTool.Hints;
using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Tests.RandomizerTool.Hints;

public sealed class GenericCreateHintTests : CreateHintTestBase<GenericCreateHint>
{
    private static readonly Type[] _ValidTypes =
    [
        typeof(IList<>),
        typeof(KeyValuePair<,>),
        typeof(GenericSample<>),
        typeof(ConstraintSample<,>),
    ];

    private static readonly Type[] _InvalidTypes =
    [
        typeof(DataHolderSample),
        typeof(IList<string>),
        typeof(KeyValuePair<int, int>),
    ];

    public GenericCreateHintTests()
        : base(_ValidTypes, _InvalidTypes) { }
}
