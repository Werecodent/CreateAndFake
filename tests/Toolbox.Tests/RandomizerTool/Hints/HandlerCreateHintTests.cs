using System.Globalization;
using System.Reflection;
using System.Text;
using Werecodent.CreateAndFake.FakerTool.Proxy;
using Werecodent.CreateAndFake.RandomizerTool;
using Werecodent.CreateAndFake.RandomizerTool.Hints;
using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Tests.RandomizerTool.Hints;

public sealed class HandlerCreateHintTests : CreateHintTestBase<HandlerCreateHint>
{
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
        : base(_ValidTypes, _InvalidTypes) { }

    [Fact]
    internal void TryToCreate_ContinuesUntilMemberFound()
    {
        for (int i = 0; i < 50; i++)
        {
            _ = TestInstance.TryToCreate(typeof(FieldInfo), CreateChainer());
        }
    }
}
