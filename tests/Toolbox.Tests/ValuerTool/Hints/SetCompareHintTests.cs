using CreateAndFake.Samples.Scenarios;
using CreateAndFake.ValuerTool.Hints;

namespace CreateAndFake.Tests.ValuerTool.Hints;

public sealed class SetCompareHintTests : CompareHintTestBase<SetCompareHint>
{
    private static readonly SetCompareHint _TestInstance = new();

    private static readonly Type[] _ValidTypes =
    [
        typeof(ISet<int>),
        typeof(HashSet<string>),
        typeof(HashSet<DataSample>),
        typeof(ISet<FieldSample>),
    ];

    private static readonly Type[] _InvalidTypes = [typeof(DataHolderSample), typeof(int)];

    public SetCompareHintTests()
        : base(_TestInstance, _ValidTypes, _InvalidTypes) { }
}
