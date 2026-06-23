using CreateAndFake.Design.Content;
using CreateAndFake.RandomizerTool.Hints;
using CreateAndFake.Samples.Scenarios;

namespace CreateAndFake.Tests.RandomizerTool.Hints;

public sealed class ValueTaskCreateHintTests : CreateHintTestBase<ValueTaskCreateHint>
{
    private static readonly ValueTaskCreateHint _TestInstance = new();

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
        : base(_TestInstance, _ValidTypes, _InvalidTypes) { }
}
