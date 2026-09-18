using System.Reflection;
using Werecodent.CreateAndFake.RandomizerTool.Hints;
using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Tests.RandomizerTool.Hints;

public sealed class EnumCreateHintTests : CreateHintTestBase<EnumCreateHint>
{
    private static readonly Type[] _ValidTypes = [typeof(BindingFlags)];

    private static readonly Type[] _InvalidTypes = [typeof(DataHolderSample)];

    public EnumCreateHintTests()
        : base(_ValidTypes, _InvalidTypes) { }
}
