using System.Globalization;
using System.Reflection;
using System.Text;
using CreateAndFake.RandomizerTool.Hints;

namespace CreateAndFake.Tests.RandomizerTool.Hints;

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
        typeof(DateTimeOffset),
        typeof(Uri),
        typeof(UriBuilder),
        typeof(StringBuilder),
    ];

    private static readonly Type[] _InvalidTypes = [typeof(object)];

    public CommonSystemCreateHintTests()
        : base(_TestInstance, _ValidTypes, _InvalidTypes) { }

    [Fact]
    internal static void TryCreate_ContinuesUntilMemberFound()
    {
        for (int i = 0; i < 50; i++)
        {
            _ = _TestInstance.TryCreate(typeof(FieldInfo), CreateChainer());
        }
    }
}
