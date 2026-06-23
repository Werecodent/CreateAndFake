using CreateAndFake.Design.Content;
using CreateAndFake.DuplicatorTool.Hints;
using CreateAndFake.Samples.Scenarios;

namespace CreateAndFake.Tests.DuplicatorTool.Hints;

public sealed class ValueTaskCopyHintTests : CopyHintTestBase<ValueTaskCopyHint>
{
    private static readonly Type[] _ValidTypes =
    [
        typeof(ValueTask<DataHolderSample>),
        typeof(ValueTask<object>),
        typeof(ValueTask<string>),
        typeof(ValueTask),
        typeof(SingleCallValueTaskSource<string>),
    ];

    private static readonly Type[] _InvalidTypes = [typeof(DataHolderSample)];

    public ValueTaskCopyHintTests()
        : base(_ValidTypes, _InvalidTypes) { }
}
