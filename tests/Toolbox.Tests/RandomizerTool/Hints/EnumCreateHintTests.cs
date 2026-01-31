using System.Reflection;
using CreateAndFake.RandomizerTool.Hints;
using CreateAndFake.Samples.Scenarios;

namespace CreateAndFake.Tests.RandomizerTool.Hints;

public sealed class EnumCreateHintTests : CreateHintTestBase<EnumCreateHint>
{
    private static readonly EnumCreateHint _TestInstance = new();

    private static readonly Type[] _ValidTypes = [typeof(BindingFlags)];

    private static readonly Type[] _InvalidTypes = [typeof(DataHolderSample)];

    public EnumCreateHintTests()
        : base(_TestInstance, _ValidTypes, _InvalidTypes) { }
}
