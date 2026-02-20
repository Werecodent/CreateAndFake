using System.Globalization;
using System.Reflection;
using System.Text;
using CreateAndFake.FakerTool.Proxy;
using CreateAndFake.RandomizerTool;
using CreateAndFake.RandomizerTool.Hints;
using CreateAndFake.Samples.Scenarios;

namespace CreateAndFake.Tests.RandomizerTool.Hints;

public sealed class HandlerCreateHintTests : CreateHintTestBase<HandlerCreateHint>
{
    private static readonly HandlerCreateHint _TestInstance = new();

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
        typeof(IFaked),
        typeof(RandomizerOptions),
    ];

    private static readonly Type[] _InvalidTypes = [typeof(DataHolderSample)];

    public HandlerCreateHintTests()
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
