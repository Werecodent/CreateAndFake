using System.Reflection;
using CreateAndFake.RandomizerTool.Hints;

namespace CreateAndFake.Tests.RandomizerTool.Hints;

public sealed class TypeInfoCreateHintTests : CreateHintTestBase<TypeInfoCreateHint>
{
    private static readonly TypeInfoCreateHint _TestInstance = new();

    private static readonly Type[] _InvalidTypes = [typeof(object)];

    public TypeInfoCreateHintTests()
        : base(_TestInstance, TypeInfoCreateHint.SupportedTypes, _InvalidTypes) { }

    [Fact]
    internal static void TryCreate_ContinuesUntilMemberFound()
    {
        for (int i = 0; i < 50; i++)
        {
            _ = _TestInstance.TryCreate(typeof(FieldInfo), CreateChainer());
        }
    }
}
