using Werecodent.CreateAndFake.RandomizerTool.Hints;
using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Tests.RandomizerTool.Hints;

public sealed class NullableCreateHintTests : CreateHintTestBase<NullableCreateHint>
{
    private static readonly Type[] _ValidTypes = [typeof(ValueTask<DataSample>?)];

    public NullableCreateHintTests()
        : base(_ValidTypes) { }
}
