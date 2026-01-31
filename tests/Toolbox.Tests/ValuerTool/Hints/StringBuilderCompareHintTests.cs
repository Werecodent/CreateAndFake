using System.Text;
using CreateAndFake.Samples.Scenarios;
using CreateAndFake.ValuerTool.Hints;

namespace CreateAndFake.Tests.ValuerTool.Hints;

public sealed class StringBuilderCompareHintTests : CompareHintTestBase<StringBuilderCompareHint>
{
    private static readonly StringBuilderCompareHint _TestInstance = new();

    private static readonly Type[] _ValidTypes = [typeof(StringBuilder)];

    private static readonly Type[] _InvalidTypes = [typeof(DataHolderSample)];

    public StringBuilderCompareHintTests()
        : base(_TestInstance, _ValidTypes, _InvalidTypes) { }
}
