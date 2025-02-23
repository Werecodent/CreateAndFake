using System.Reflection;
using CreateAndFake.RandomizerTool.CreateHints;

namespace CreateAndFake.Tests.RandomizerTool.CreateHints;

public sealed class EnumCreateHintTests : CreateHintTestBase<EnumCreateHint>
{
    private static readonly EnumCreateHint _TestInstance = new();

    private static readonly Type[] _ValidTypes = [typeof(BindingFlags)];

    private static readonly Type[] _InvalidTypes = [typeof(object)];

    public EnumCreateHintTests() : base(_TestInstance, _ValidTypes, _InvalidTypes) { }
}
