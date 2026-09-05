using Werecodent.CreateAndFake.Design.Content;
using Werecodent.CreateAndFake.RandomizerTool.Hints;
using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Tests.RandomizerTool.Hints;

public sealed class ValueTaskCreateHintTests : CreateHintTestBase<ValueTaskCreateHint>
{
    private static readonly Type[] _ValidTypes =
    [
        typeof(ValueTask<DataHolderSample>),
        typeof(ValueTask<object>),
        typeof(ValueTask<string>),
        typeof(ValueTask<int>),
        typeof(ValueTask<bool>),
        typeof(SingleCallValueTaskSource<string>),
    ];

    private static readonly Type[] _InvalidTypes = [typeof(DataHolderSample)];

    public ValueTaskCreateHintTests()
        : base(_ValidTypes, _InvalidTypes) { }
}
