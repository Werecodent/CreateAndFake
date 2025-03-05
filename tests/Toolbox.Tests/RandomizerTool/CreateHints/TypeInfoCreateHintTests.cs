using System.Reflection;
using CreateAndFake.RandomizerTool.CreateHints;

namespace CreateAndFake.Tests.RandomizerTool.CreateHints;

public sealed class TypeInfoCreateHintTests : CreateHintTestBase<TypeInfoCreateHint>
{
    private static readonly TypeInfoCreateHint _TestInstance = new();

    private static readonly Type[] _ValidTypes =
    [
        typeof(Type),
        typeof(Type).GetType(),
        typeof(ConstructorInfo),
        typeof(PropertyInfo),
        typeof(MethodInfo),
        typeof(MemberInfo),
        typeof(MemberInfo),
        typeof(FieldInfo),
        typeof(ParameterInfo),
        typeof(MethodBase),
    ];

    private static readonly Type[] _InvalidTypes = [typeof(object)];

    public TypeInfoCreateHintTests() : base(_TestInstance, _ValidTypes, _InvalidTypes) { }

    [Fact]
    internal static void TryCreate_ContinuesUntilMemberFound()
    {
        for (int i = 0; i < 50; i++)
        {
            _ = _TestInstance.TryCreate(typeof(FieldInfo), CreateChainer());
        }
    }
}
