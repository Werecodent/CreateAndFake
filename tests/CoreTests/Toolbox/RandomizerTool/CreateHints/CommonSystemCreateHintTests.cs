using System.Globalization;
using System.Reflection;
using System.Text;
using CreateAndFake.Toolbox.RandomizerTool.CreateHints;

namespace CreateAndFakeTests.Toolbox.RandomizerTool.CreateHints;

public sealed class CommonSystemCreateHintTests : CreateHintTestBase<CommonSystemCreateHint>
{
    private static readonly CommonSystemCreateHint _TestInstance = new();

    private static readonly Type[] _ValidTypes =
    [
        typeof(CultureInfo),
        typeof(TimeSpan),
        typeof(DateTime),
        typeof(Assembly),
        typeof(AssemblyName),
        typeof(Guid),
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
        typeof(DateTimeOffset),
        typeof(Uri),
        typeof(UriBuilder),
        typeof(StringBuilder)
    ];

    private static readonly Type[] _InvalidTypes = [typeof(object)];

    public CommonSystemCreateHintTests() : base(_TestInstance, _ValidTypes, _InvalidTypes) { }

    [Fact]
    internal static void TryCreate_ContinuesUntilMemberFound()
    {
        for (int i = 0; i < 50; i++)
        {
            _ = _TestInstance.TryCreate(typeof(FieldInfo), CreateChainer());
        }
    }
}
